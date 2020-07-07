using System;
using System.Collections.Generic;
using System.IO;
using Common;

namespace LOConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var helpArgs = new List<string> {"-h", "--help"};
            if (args.Length > 0 && args[0].IsContainedIn(helpArgs)) {
                DisplayInstructions();
            }

            GameConfiguration config;
            try
            {
                config = new GameConfiguration(args);
            }
            catch (InvalidDataException ex)
            {
                DisplayProgramHelp(ex);
                return;
            }


            DisplayInstructions();

            var game = new Game(config);
            game.Run();
        }

        private static void DisplayProgramHelp(Exception ex)
        {
            using (var ctx = new ConsoleContext())
            {
                Console.WriteLine("Usage: LOConsole [board-size: 1-10]");

                if (ex != null)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine();
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void DisplayInstructions()
        {
            string[] paras = {
                "The game displays a 5x5 grid of lights. Initially some of the lights will be turned on",
                "For each turn the game will display the current state of the grid and asks you to specify a light. That light and any horizonal or vertical neighbours will be toggled",
                "The goal is to turn all the lights off.",
                "To specify a light enter its row and column, e.g. 'A1' refers to to the top left light (lowercase letters are also acceptable).",
                "To toggle edit mode on or off type 'edit'.",
                "To exit type 'exit'."
             };

            // Console.ForegroundColor = ConsoleColor.Green;

            foreach (var para in paras)
            {
                Console.WriteLine(para);
                Console.WriteLine();
            }
        }
    }
}
