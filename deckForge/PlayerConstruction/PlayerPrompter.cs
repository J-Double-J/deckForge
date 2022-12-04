using DeckForge.HelperObjects;

namespace DeckForge.PlayerConstruction
{
    /// <summary>
    /// Prompts the <see cref="IPlayer"/> for some input and returns a validated input response.
    /// </summary>
    public class PlayerPrompter : IPrompter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPrompter"/> class.
        /// </summary>
        /// <param name="prompt">The prompt the player will be shown when they need to make a choice. First option
        /// starts at 1. For any headers in the prompt set it to key 0. Prompt is displayed in order of the dictionary.</param>
        /// <param name="optionCount">The number of valid options for the <see cref="IPlayer"/> to choose from.</param>
        public PlayerPrompter(Dictionary<int, string> prompt)
            : this(prompt, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPrompter"/> class. Allows going back and cancelling.
        /// </summary>
        /// <param name="prompt">The prompt the player will be shown when they need to make a choice. First option
        /// starts at 1. For any headers in the prompt set it to key 0. For any cancel text set it to key -1. Prompt is displayed in order of
        /// the dictionary.</param>
        /// /// <param name="cancelAllowed">Sets whether or not this prompt will allow a user to cancel. Default is <c>false</c>.</param>
        public PlayerPrompter(Dictionary<int, string> prompt, bool cancelAllowed)
            : this(new ConsoleReader(), new ConsoleOutput(), prompt, cancelAllowed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPrompter"/> class. Can specify whether cancelling is allowed, as well
        /// as input and output.
        /// </summary>
        /// <param name="reader">Specifies where to get user input.</param>
        /// <param name="output">Specifies where to display any output.</param>
        /// <param name="prompt">The prompt the player will be shown when they need to make a choice. First option
        /// starts at 1. For any headers in the prompt set it to key 0. For any cancel text set it to key -1. Prompt is displayed in order of
        /// the dictionary.</param>
        /// /// <param name="cancelAllowed">Sets whether or not this prompt will allow a user to cancel. Default is <c>false</c>.</param>
        public PlayerPrompter(IInputReader reader, IOutputDisplay output, Dictionary<int, string> prompt, bool cancelAllowed = false)
        {
            PromptDict = prompt;
            CancelAllowed = cancelAllowed;
            InputReader = reader;
            OutputDisplay = output;
        }

        /// <summary>
        /// Gets a value indicating whether this prompt allows a user to cancel.
        /// </summary>
        public bool CancelAllowed { get; }

        /// <summary>
        /// Gets the prompt to display.
        /// </summary>
        protected Dictionary<int, string> PromptDict { get; }

        private IInputReader InputReader { get; }

        private IOutputDisplay OutputDisplay { get; }

        /// <summary>
        /// Presents the Player with the given prompt.
        /// </summary>
        /// <returns>An integer that is the response that the <see cref="IPlayer"/> chose to execute.</returns>
        public int Prompt()
        {
            string? response;
            do
            {
                DisplayPrompt();
                response = InputReader.GetInput();
            }
            while (!IsValidResponse(response));

            return int.Parse(response!);
        }

        private void DisplayPrompt()
        {
            foreach (var entry in PromptDict)
            {
                OutputPromptEntry(entry);
            }
        }

        private void OutputPromptEntry(KeyValuePair<int, string> entry)
        {
            if (entry.Key != 0)
            {
                OutputDisplay.Display($"\t{entry.Key}) {entry.Value}");
            }
            else
            {
                OutputDisplay.Display(entry.Value);
            }
        }

        private bool IsValidResponse(string? response)
        {
            if (int.TryParse(response, out int numericResponse))
            {
                return ValidateWhetherIntegerIsInBounds(numericResponse);
            }
            else
            {
                return false;
            }
        }

        private bool ValidateWhetherIntegerIsInBounds(int response)
        {
            if (response > 0 && PromptDict.ContainsKey(response))
            {
                return true;
            }
            else if (response == -1 && CancelAllowed && PromptDict.ContainsKey(-1))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
