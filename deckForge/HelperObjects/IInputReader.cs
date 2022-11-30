namespace DeckForge.HelperObjects
{
    /// <summary>
    /// An interface for objects that can read user input and format it as a string.
    /// </summary>
    public interface IInputReader
    {
        /// <summary>
        /// Reads an input from some source.
        /// </summary>
        /// <returns>A string of input.</returns>
        public string? GetInput();
    }
}
