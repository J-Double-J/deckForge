using DeckForge.HelperObjects;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;

namespace DeckForge.PlayerConstruction
{
    /// <summary>
    /// Prompts the user to pick an <see cref="IGameAction{T}"/>.
    /// </summary>
    public class PlayerActionChoicePrompter
    {
        private Dictionary<string, (IGameAction<IPlayer> Action, int ActionCount)> actions;

        // TODO: Could this be static?

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerActionChoicePrompter"/> class.
        /// </summary>
        /// <param name="actions">Dictionary of actions with action names as the key, and stores the action type and its count.</param>
        public PlayerActionChoicePrompter(Dictionary<string, (IGameAction<IPlayer> Action, int ActionCount)> actions)
            : this(new ConsoleReader(), new ConsoleOutput(), actions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerActionChoicePrompter"/> class.
        /// </summary>
        /// <param name="reader">Specifies where to get user input.</param>
        /// <param name="output">Specifies where to display any output.</param>
        /// <param name="actions">Dictionary of actions with action names as the key, and stores the action type and its count.</param>
        public PlayerActionChoicePrompter(
            IInputReader reader,
            IOutputDisplay output,
            Dictionary<string, (IGameAction<IPlayer> Action, int ActionCount)> actions)
        {
            this.actions = actions;
            InputReader = reader;
            OutputDisplay = output;
        }

        /// <summary>
        /// Gets where to read any input.
        /// </summary>
        protected IInputReader InputReader { get; }

        /// <summary>
        /// Gets where to display any output.
        /// </summary>
        protected IOutputDisplay OutputDisplay { get; }

        /// <summary>
        /// Prompts the <see cref="IPlayer"/> to select an action to do.
        /// </summary>
        /// <returns>The selected <see cref="IGameAction{IPlayer}"/>.</returns>
        public IGameAction<IPlayer> Prompt()
        {
            var promptDisplayDict = GeneratePromptDictionary();

            if (!PromptDisplayDictIsEmpty(promptDisplayDict))
            {
                var pp = new PlayerPrompter(InputReader, OutputDisplay, promptDisplayDict);
                int response = pp.Prompt();
                return actions[actions.Keys.ToList()[response - 1]].Action; // Responses are offset by 1 for prompt header.
            }

            return new EndTurnAction();
        }

        /// <summary>
        /// Creates the display for all the actions <see cref="IPlayer"/>'s can do on their turn.
        /// </summary>
        /// <returns>A dictionary that can be used in a <see cref="PlayerPrompter"/>.</returns>
        protected virtual Dictionary<int, string> GeneratePromptDictionary()
        {
            Dictionary<int, string> prompt = new() { { 0, "Which action would you like to do?" } };

            var keysList = actions.Keys.ToList();
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[keysList[i]].ActionCount != 0)
                {
                    prompt[i + 1] = $"{keysList[i]} [{actions[keysList[i]].ActionCount} left]";
                }
            }

            return prompt;
        }

        private static bool PromptDisplayDictIsEmpty(Dictionary<int, string> prompt)
        {
            return prompt.Count <= 1;
        }
    }
}