namespace DeckForge.HelperObjects
{
    /// <summary>
    /// An interface with objects that display output somewhere.
    /// </summary>
    public interface IOutputDisplay
    {
        /// <summary>
        /// Displays some given output.
        /// </summary>
        /// <param name="output">Output to display.</param>
        public void Display(string output);

        /// <summary>
        /// Clears all output from display.
        /// </summary>
        public void Clear();
    }
}
