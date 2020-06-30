using System;
using System.Collections.Generic;
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
            DisplayBoard();
        }

        private void DisplayBoard()
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

        private void DisplayRow(IEnumerable<LightState> row, char rowLabel)
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