using DeckForge.HelperObjects;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;
using System;

namespace DeckForge.GameConstruction.PresetGames.Dominion
{
    /// <summary>
    /// Action prompter with special rules for Dominion.
    /// </summary>
    public class DominionPlayerActionChoicePrompter : PlayerActionChoicePrompter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionPlayerActionChoicePrompter"/> class.
        /// </summary>
        /// <param name="actions">Dictionary of actions with action names as the key, and stores the action type and its count.</param>
        public DominionPlayerActionChoicePrompter(Dictionary<string, (IGameAction<IPlayer> Action, int ActionCount)> actions)
            : this(new ConsoleReader(), new ConsoleOutput(), actions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DominionPlayerActionChoicePrompter"/> class.
        /// </summary>
        /// <param name="reader">Specifies where to get user input.</param>
        /// <param name="output">Specifies where to display any output.</param>
        /// <param name="actions">Dictionary of actions with action names as the key, and stores the action type and its count.</param>
        public DominionPlayerActionChoicePrompter(
            IInputReader reader,
            IOutputDisplay output,
            Dictionary<string, (IGameAction<IPlayer> Action, int ActionCount)> actions)
            : base(reader, output, actions)
        {
        }

        /// <inheritdoc/>
        protected override Dictionary<int, string> GeneratePromptDictionary()
        {
            Dictionary<int, string> prompt = new() { { 0, "Which action would you like to do?" } };

            var keysList = Actions.Keys.ToList();
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[keysList[i]].ActionCount != 0)
                {
                    prompt[i + 1] = $"{keysList[i]} [{Actions[keysList[i]].ActionCount} left]";
                }
                else if (Actions[keysList[i]].ActionCount == 0 && keysList[i] == new PlayCardAction().Name)
                {
                    prompt[i + 1] = $"{keysList[i]} [No More Action Card Plays Allowed]";
                }
            }

            return prompt;
        }
    }
}
