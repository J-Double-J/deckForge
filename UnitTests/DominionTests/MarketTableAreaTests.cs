using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameConstruction.PresetGames.Dominion.Table;
using DeckForge.GameElements.Resources;
using DeckForge.HelperObjects;
using DeckForge.PlayerConstruction;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.DominionTests
{
    [TestClass]
    public class MarketTableAreaTests
    {
        [TestMethod]
        public void DecksAreDisplayedCorrectlyThroughPrompt()
        {
            ConsoleOutputMock output = new();
            List<IDeck> decks = new()
            {
                new MonotoneDeck(typeof(CopperCard), 6),
                new MonotoneDeck(typeof(SilverCard), 4),
                new MonotoneDeck(typeof(GoldCard), 3)
            };
            DominionMarketTableArea market = new(decks);

            List<string> marketList = market.GetMarketAreaAsStringList();
            Dictionary<int, string> prompt = new() { { 0, "Market:" } };
            for (int i = 0; i < marketList.Count; i++)
            {
                prompt.Add(i + 1, marketList[i]);
            }

            PlayerPrompter pp = new(new ConsoleInputMock(new() { "1" }), output, prompt);
            pp.Prompt();

            List<string> expectedOutput = new()
            {
                "Market:",
                "\t1) Copper - [6 Remaining]",
                "\t2) Silver - [4 Remaining]",
                "\t3) Gold - [3 Remaining]"
            };
            output.CompleteOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
