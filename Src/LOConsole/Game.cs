using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using LODomain;

namespace LOConsole
{
    public class Game
    {
        private const ConsoleColor _labelColor = ConsoleColor.Cyan;

        private Board Board { get; set; }

        public Game()
        {
            Board = new Board();
            Board.NewGame();
        }

        public void Run()
        {
            var firstTurn = true;
            do
            {
                if (!firstTurn) Console.WriteLine();

                DisplayBoard();

                var command = RequestCommand();

                if (command.Exit)
                {
                    return;
                }

                PerformCommand(command);
                firstTurn = false;
            } while (!Board.IsComplete());

            DisplaySuccess();
        }

        private static void DisplaySuccess()
        {
            using (var ctx = new ConsoleContext())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Congratulations. You won!. Run the program again for a new board.");
            }
        }

        private void PerformCommand(GameCommand command)
        {
            Board.MakeMove(command.Location);
        }

        /// <summary>
        /// Request a command until we get one that we recognise.
        /// Display brief help if we don't recognise a command.
        /// </summary>
        /// <returns></returns>
        private GameCommand RequestCommand()
        {
            LightLocation location = null;
            var firstRequest = true;
            while (location == null)
            {
                if (!firstRequest)
                {
                    DisplayCommandSummary();
                }

                Console.Write("Please enter a location or 'exit': ");
                var command = Console.ReadLine().Trim().ToLower();

                var exitCommands = new List<string> { "exit", "quit", "i've had enough" };
                if (exitCommands.Contains(command))
                {
                    return new GameCommand(location: null, exit: true);
                }

                location = ParseLocation(command);
                firstRequest = false;
            }

            return new GameCommand(location, exit: false);
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
                    .Where(c => !stripChars.Contains(c))
                    .ToArray()
            );

            var validRows = "abcde";
            var validCols = "12345";
            if (
                stripped.Length != 2
                || !validRows.Contains(stripped[0])
                || !validCols.Contains(stripped[1])
            )
            {
                return null;
            }

            return new LightLocation(
                row: stripped[0] - (int)'a',
                column: Int16.Parse(stripped[1].ToString()) - 1
            );
        }

        private void DisplayCommandSummary()
        {
            using (var ctx = new ConsoleContext())
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("I didn't recognise that command. Please enter a light location, e.g. 'A1' or 'a1' for the top-left, or 'exit' to quit.");
            }
        }

        private void DisplayBoard()
        {
            using (var ctx = new ConsoleContext())
            {

                Console.ForegroundColor = _labelColor;
                Console.WriteLine("   1 2 3 4 5");
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