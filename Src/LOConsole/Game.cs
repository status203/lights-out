using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;
using LODomain;

namespace LOConsole
{
    public class Game
    {
        private const ConsoleColor _labelColor = ConsoleColor.Cyan;

        public Game(GameConfiguration configuration, Board board, GameMode mode, bool stateChanged)
        {
            this.Configuration = configuration;
            this.Board = board;
            this.Mode = mode;
            this.StateChanged = stateChanged;

        }
        private GameConfiguration Configuration { get; }

        private Board Board { get; set; }

        private GameMode Mode { get; set; }


        /// <summary>
        /// Whether the last command changed the state. If not, then don't 
        /// redisplay state.
        /// </summary>
        private bool StateChanged { get; set; }

        public Game(GameConfiguration config)
        {
            Configuration = config;
            Board = new Board(Configuration.BoardSize);
            Board.NewGame();
            Mode = GameMode.Play;
            StateChanged = true;
        }

        public void Run()
        {
            var firstTurn = true;
            do
            {
                if (!firstTurn) Console.WriteLine();

                if (StateChanged)
                {
                    DisplayBoard();
                }

                var command = RequestCommand();

                ExecuteCommand(command);

                firstTurn = false;
            } while (Mode != GameMode.Exit);

            if (Board.IsComplete())
            {
                DisplaySuccess();
            }
        }

        private static void DisplaySuccess()
        {
            using (var ctx = new ConsoleContext())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Congratulations. You won!.");
                Console.WriteLine("Run the program again for a new board.");
            }
        }

        /// <summary>
        /// Executes the supplied game command.
        /// </summary>
        /// <param name="command"></param>
        private void ExecuteCommand(GameCommand command)
        {
            // Most cases will change state.
            StateChanged = true;

            switch (command.Command)
            {
                case Commands.MakeMove:
                    Board.MakeMove(command.Location);
                    if (Board.IsComplete()) Mode = GameMode.Exit;
                    break;

                case Commands.ToggleLight:
                    Board.ToggleLight(command.Location);
                    break;

                case Commands.ToggleEdit:
                    Mode = Mode == GameMode.Play
                        ? GameMode.Edit
                        : GameMode.Play;
                    break;

                case Commands.Exit:
                    Mode = GameMode.Exit;
                    break;

                default:
                    // Unknown command
                    DisplayCommandSummary();
                    StateChanged = false;
                    break;
            }

        }

        /// <summary>
        /// Request a command until we get one that we recognise.
        /// Display brief help if we don't recognise a command.
        /// </summary>
        /// <returns></returns>
        private GameCommand RequestCommand()
        {
            Console.Write("Please enter a location or command: ");
            var command = Console.ReadLine().Trim().ToLower();

            // Plain commands
            switch (command)
            {
                case "exit":
                    return new GameCommand(Commands.Exit);

                case "edit":
                    return new GameCommand(Commands.ToggleEdit);
            }

            // Commands that take a location (based on mode)
            var location = ParseLocation(command);
            if (location != null)
            {
                return new GameCommand(
                    Mode == GameMode.Play ? Commands.MakeMove : Commands.ToggleLight,
                    location
                );
            }

            // Unknown command
            return new GameCommand(Commands.Unknown);
        }

        /// <summary>
        /// Parse a command to a light location, or return null if unable to do so.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private LightLocation ParseLocation(string command)
        {
            // Be generous on input. Allow but strip out some 'likely' punctuation.
            var stripChars = ".-,";
            var stripped = new string(
                command
                    .ToList()
                    .Where(c => !c.IsContainedIn(stripChars))
                    .ToArray()
            );

            // TODO: Calculate and store valid labels in constructor.
            var validRows = CalculateValidRows(Configuration.BoardSize);
            var validCols = CalculateValidColumns(Configuration.BoardSize);
            if (
                stripped.Length != 2
                || !stripped[0].IsContainedIn(validRows)
                || !stripped[1].IsContainedIn(validCols)
            )
            {
                return null;
            }

            return new LightLocation(
                row: stripped[0] - 'a',
                column: Int16.Parse(stripped[1].ToString()) - 1
            );
        }

        private IEnumerable<char> CalculateValidColumns(int boardSize) =>
            Enumerable.Range(0, boardSize).Select(i => (char)('1' + i));

        private IEnumerable<char> CalculateValidRows(int boardSize) =>
            Enumerable.Range(0, boardSize).Select(i => (char)('a' + i));

        private void DisplayCommandSummary()
        {
            using (var ctx = new ConsoleContext())
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("I didn't recognise that command.");
                Console.WriteLine("Please enter a light location, e.g. 'A1' or 'a1' for the top-left, 'edit' to toggle edit mode on or off, or 'exit' to quit.");
            }
        }

        private void DisplayBoard()
        {
            using (var ctx = new ConsoleContext())
            {
                if (Mode == GameMode.Edit)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("*** Board is in EDIT mode. Lights will be toggled individually ***");
                }

                Console.ForegroundColor = _labelColor;
                var rowLabels = "   " + string.Join(
                    ' ',
                    Enumerable.Range(1, Configuration.BoardSize)
                );
                    
                Console.WriteLine(rowLabels);
                Console.WriteLine();

                var rowLabel = 'A';
                foreach (var row in Board.Lights)
                {
                    DisplayRow(row, rowLabel);

                    rowLabel = GetNextRowLabel(rowLabel);
                }
            }
        }

        private void DisplayRow(IEnumerable<LightState> row, char rowLabel)
        {
            using (var ctx = new ConsoleContext())
            {
                Console.ForegroundColor = _labelColor;
                Console.Write(rowLabel);
                Console.Write(" ");

                foreach (var light in row)
                {
                    Console.Write(" ");
                    var lightColor = GetLightColor(light);
                    var lightChar = GetLightChar(light);

                    Console.ForegroundColor = lightColor;
                    Console.Write(lightChar);
                }

                Console.WriteLine();
            }
        }

        private ConsoleColor GetLightColor(LightState light) =>
            light == LightState.On
                ? ConsoleColor.Yellow
                : ConsoleColor.Gray;

        private char GetLightChar(LightState light) =>
            light == LightState.On ? 'O' : '.';

        private char GetNextRowLabel(char currentLabel) =>
            (char)((int)currentLabel + 1);
    }
}