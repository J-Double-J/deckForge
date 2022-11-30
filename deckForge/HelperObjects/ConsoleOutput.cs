namespace DeckForge.HelperObjects
{
    /// <summary>
    /// Displays output using the console.
    /// </summary>
    public class ConsoleOutput : IOutputDisplay
    {
        /// <inheritdoc/>
        public void Display(string output)
        {
            Console.WriteLine(output);
        }
    }
}
