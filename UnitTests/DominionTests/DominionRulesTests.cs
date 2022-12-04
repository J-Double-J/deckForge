using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.Dominion;
using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameConstruction.PresetGames.Dominion.Rules;
using DeckForge.GameConstruction.PresetGames.Dominion.Table;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.HelperObjects;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.DominionTests
{
    [TestClass]
    public class DominionRulesTests
    {
        [TestMethod]
        public void DominionGameGoesThroughRulesCorrectly()
        {
            ConsoleOutputMock gmOutput = new();
            DominionGameMediator gm = new DominionGameMediator(new ConsoleInputMock(new()), gmOutput, 2);
            gm.RegisterTurnHandler(new TurnHandler(2));
            List<IDeck> marketDecks = new()
            {
                new MonotoneDeck(typeof(CopperCard), 6),
                new MonotoneDeck(typeof(SilverCard), 4),
                new MonotoneDeck(typeof(GoldCard), 3),
                new MonotoneDeck(typeof(ProvinceCard), 1),
            };
            List<TableArea> playerAreas = new()
            {
                new DominionPlayerTableArea(0), new DominionPlayerTableArea(1)
            };
            Table table = new(
                gm,
                new()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, playerAreas),
                    new TableZone(TablePlacementZoneType.NeutralZone, new() { new DominionMarketTableArea(marketDecks) })
                });
            IRoundRules round = new DominionRound(gm, new() { 0, 1 });

            // Play first card, then go buy silver card, then end turn.
            ConsoleInputMock playerOneInput = new(new() { "1", "1", "2", "2", "3" });
            ConsoleOutputMock playerOneOutput = new();

            // Play first three cards, then buy province card, then end turn.
            ConsoleInputMock playerTwoInput = new(new() { "1", "1", "1", "1", "1", "1", "2", "4", "3" });
            ConsoleOutputMock playerTwoOutput = new();

            DominionPlayer playerOne = new(playerOneInput, playerOneOutput, gm, 0);
            DominionPlayer playerTwo = new(playerTwoInput, playerTwoOutput, gm, 1);

            playerOne.AddCardToHand(new GoldCard());
            playerTwo.AddCardsToHand(new() { new GoldCard(), new GoldCard(), new GoldCard() });

            gm.StartGame();

            gmOutput.CompleteOutput.Contains("Player 1 wins!\n").Should().BeTrue();
        }
    }
}
