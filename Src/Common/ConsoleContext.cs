using System;

namespace Common
{
    /// <summary>
    /// Class for setting console properties that restores the entry
    /// values on disposing.
    /// </summary>
    public class ConsoleContext : IDisposable
    {
        public ConsoleColor EntryForegroudColor { get; private set; }

        public ConsoleContext()
        {
            EntryForegroudColor = Console.ForegroundColor;
        }

        public void Dispose()
        {
            Console.ForegroundColor = EntryForegroudColor;
        }
    }
}