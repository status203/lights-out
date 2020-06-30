using LODomain;

namespace LOConsole
{
    internal class GameCommand
    {
        /// <summary>
        /// Light location specified by user. May be null if user has specified
        /// something other than a move.
        /// </summary>
        /// <value></value>
        internal GameCommand(LightLocation location, bool exit)
        {
            this.Location = location;
            this.Exit = exit;

        }
        public LightLocation Location { get; private set; }

        /// <summary>
        /// Whether the user has requested to end the game.
        /// </summary>
        /// <value></value>
        public bool Exit { get; private set; }
    }
}