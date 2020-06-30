using System;

namespace LOConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayInstructions();

            var game = new Game();
            game.Run();
        }

        private static void DisplayInstructions()
        {
            string[] paras = {
                "The game displays a 5x5 grid of lights. Initially some of the lights will be turned on",
                "For each turn the game will display the current state of the grid and asks you to specify a light. That light and any horizonal or vertical neighbours will be toggled",
                "The goal is to turn all the lights off.",
                "To specify a light enter its row and column, e.g. 'A1' refers to to the top left light (lowercase letters are also acceptable). To exit type 'exit'"
             };

            // Console.ForegroundColor = ConsoleColor.Green;

             foreach(var para in paras) {
                 Console.WriteLine(para);
                 Console.WriteLine();
             }
        }
    }
}
