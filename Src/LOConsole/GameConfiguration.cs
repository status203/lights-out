using System.IO;
using Common;

namespace LOConsole
{
    public class GameConfiguration
    {
        public int BoardSize { get; }

        /// <summary>
        /// Constructs a GameConfiguration from the commandline parameters
        /// </summary>
        /// <param name="args"></param>
        public GameConfiguration(string[] args)
        {
            // Default to the 5x5 board.
            if (args.Length == 0) {
                BoardSize = 5;
                return;
            }

            if (int.TryParse(args[0], out var boardSize)) {
                BoardSize = boardSize;
            }

            if (BoardSize.Between(1, 11)) {
                return;
            }

            throw new InvalidDataException("If specified, board size must be an integer between 1 & 10 inclusive");
        }
    }
}