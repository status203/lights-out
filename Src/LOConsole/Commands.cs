namespace LOConsole
{
    public enum Commands
    {
        Unknown = 0, // Unknown command.

        MakeMove, // Make a move (only in Play mode)
        ToggleLight, // Toggle a single light (only in Edit mode)
        
        ToggleEdit, // Toggle Edit mode (single light switching)

        Exit, // Exit game
    }
}