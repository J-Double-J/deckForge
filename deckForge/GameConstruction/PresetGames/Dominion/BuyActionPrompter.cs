using DeckForge.GameConstruction.PresetGames.Dominion.Table;
using DeckForge.HelperObjects;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.Dominion
{
    /// <summary>
    /// Prompter for Dominion for whenever a <see cref="DominionPlayer"/> wants to buy a <see cref="ICard"/> from the marketplace.
    /// </summary>
    public class BuyActionPrompter : IPrompter
    {
        private PlayerPrompter prompter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyActionPrompter"/> class. Specifies input and output destinations.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="reader">Specifies where to get user input.</param>
        /// <param name="output">Specifies where to display any output.</param>
        public BuyActionPrompter(IGameMediator gm, IInputReader reader, IOutputDisplay output)
        {
            DominionGameMediator? domGm = gm as DominionGameMediator;
            List<string> marketList = domGm!.Market.GetMarketAreaAsStringList();

            var prompt = CreatePromptFromMarketList(marketList);
            prompter = new(reader, output, prompt, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuyActionPrompter"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        public BuyActionPrompter(IGameMediator gm)
            : this(gm, new ConsoleReader(), new ConsoleOutput())
        {
        }

        /// <inheritdoc/>
        public int Prompt()
        {
            return prompter.Prompt();
        }

        private static Dictionary<int, string> CreatePromptFromMarketList(List<string> marketList)
        {
            Dictionary<int, string> prompt = new() { { 0, "Market:" } };

            for (int i = 0; i < marketList.Count; i++)
            {
                prompt.Add(i + 1, marketList[i]);
            }

            prompt.Add(-1, "Cancel");

            return prompt;
        }
    }
}
