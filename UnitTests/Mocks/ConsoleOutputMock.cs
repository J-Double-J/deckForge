using DeckForge.HelperObjects;

namespace UnitTests.Mocks
{
    /// <summary>
    /// Mocks the output from a console. Tracks the currently displayed string, as well as all the outputs displayed to this mock.
    /// </summary>
    public class ConsoleOutputMock : IOutputDisplay
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleOutputMock"/> class.
        /// </summary>
        public ConsoleOutputMock()
        {
            CompleteOutput = new();
        }

        /// <summary>
        /// Gets the currently "displayed" output.
        /// </summary>
        public string? CurrentOutput { get; private set; }

        /// <summary>
        /// Gets the list of all outputs that were displayed.
        /// </summary>
        public List<string> CompleteOutput { get; }

        /// <inheritdoc/>
        public void Display(string output)
        {
            CurrentOutput = output;
            CompleteOutput.Add(output);
        }

        /// <inheritdoc/>
        public void Clear()
        {
        }
    }
}
