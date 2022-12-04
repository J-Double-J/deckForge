using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.Dominion;
using DeckForge.GameConstruction.PresetGames.Dominion.Actions;
using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameConstruction.PresetGames.Dominion.Table;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.DominionTests.CardTests
{
    /// <summary>
    /// Test class for Dominion cards with simpler rules without many or any edge cases specific to the card itself.
    /// </summary>
    [TestClass]
    public class SimplerDominionCardTests
    {
        private Table table;
        private IGameMediator gm;

        [TestInitialize]
        public void TestInitialize()
        {
            gm = new BaseGameMediator(1);
            DominionPlayerTableArea presetArea = new DominionPlayerTableArea(0);
            table = new(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }) });
        }

        [TestMethod]
        public void VillageCardExecutesAsExpected()
        {
            PlayerGameAction interestedAction = new PlayCardAction();

            DominionPlayer player = new(new ConsoleInputMock(new() { "0" }), new ConsoleOutputMock(), gm, 0);

            player.AddCardToHand(new VillageCard());
            player.PlayCard();

            player.Actions[interestedAction.Name].ActionCount.Should().Be(2, "2 actions were gained from playing the Village");
            player.HandSize.Should().Be(1, "player drew a card from playing the Village");
        }

        [TestMethod]
        public void SmithyCardExecutesAsExpected()
        {
            DominionPlayer player = new(new ConsoleInputMock(new() { "0" }), new ConsoleOutputMock(), gm, 0);

            player.AddCardToHand(new SmithyCard());
            player.PlayCard();

            player.HandSize.Should().Be(3, "player drew 3 cards from playing the Smithy");
        }

        [TestMethod]
        public void WoodcutterCardExecutesAsExpected()
        {
            PlayerGameAction interestedAction = new BuyAction();
            DominionPlayer player = new(new ConsoleInputMock(new() { "0" }), new ConsoleOutputMock(), gm, 0);

            player.AddCardToHand(new WoodcutterCard());
            player.PlayCard();

            player.Actions[interestedAction.Name].ActionCount.Should().Be(2, "a buy action was gained from playing the Woodcutter");
            player.Coins.Should().Be(2);
        }

        [TestMethod]
        public void LaboratoryCardExecutresAsExpected()
        {
            PlayerGameAction interestedAction = new PlayCardAction();
            DominionPlayer player = new(new ConsoleInputMock(new() { "0" }), new ConsoleOutputMock(), gm, 0);

            player.AddCardToHand(new LaboratoryCard());
            player.PlayCard();

            player.Actions[interestedAction.Name].ActionCount.Should().Be(1, "a play action was gained from playing the Laboratory");
            player.HandSize.Should().Be(2);
        }

    }
}
