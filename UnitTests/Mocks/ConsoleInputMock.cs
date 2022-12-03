using DeckForge.HelperObjects;

namespace UnitTests.Mocks
{
    /// <summary>
    /// A console mock that gives an input of strings that are iterated over after each call of <see cref="GetInput"/>.
    /// </summary>
    public class ConsoleInputMock : IInputReader
    {
        private readonly List<string> input;
        private int nextInput = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleInputMock"/> class.
        /// </summary>
        /// <param name="input">List of strings that represent input from a user source.</param>
        public ConsoleInputMock(List<string> input)
        {
            this.input = input;
        }

        /// <inheritdoc/>
        public string? GetInput()
        {
            var inputString = input[nextInput];
            nextInput++;
            return inputString;
        }
    }
}
