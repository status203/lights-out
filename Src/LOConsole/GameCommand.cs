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
        internal GameCommand(Commands command, LightLocation location = null)
        {
            this.Location = location;
            Command = command;
        }
        /// <summary>
        /// What type of action to take.
        /// </summary>
        /// <value></value>
        public Commands Command { get; private set; }

        /// <summary>
        /// What location to use with the action if appropriate, null otherwise.
        /// </summary>
        /// <value></value>
        public LightLocation Location { get; private set; }

    }
}