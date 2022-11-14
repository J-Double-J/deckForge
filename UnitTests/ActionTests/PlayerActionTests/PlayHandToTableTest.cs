using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;
using DeckForge.PlayerConstruction;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.ActionTests.PlayerActionTests
{
    [TestClass]
    public class PlayHandToTableTest
    {
        [TestMethod]
        public void PlayersHandIsOnTable()
        {
            IGameMediator gm = new BaseGameMediator(1);
            IPlayer player = new TestPlayerMock(gm, 0);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 1);
            Table table = new(gm, new List<TableZone>() { zone });
            PlayerGameAction action = new PlayHandToTable();
            List<PlayingCard> cards = new() { new PlayingCard(10, "J"), new PlayingCard(2, "J") };

            player.AddCardToHand(cards[0]);
            player.AddCardToHand(cards[1]);

            action.Execute(player);
            action.Execute(player);

            table.GetCardsInZone(TablePlacementZoneType.PlayerZone)[0].Should().BeEquivalentTo(cards, "the player played these cards from their hand");
        }

        [TestMethod]
        public void PlayerPlaysTheirEmptyHand()
        {
            IGameMediator gm = new BaseGameMediator(1);
            IPlayer player = new TestPlayerMock(gm, 0);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 1);
            Table table = new(gm, new List<TableZone>() { zone });
            PlayerGameAction action = new PlayHandToTable();
            List<PlayingCard> cards = new();

            action.Execute(player);
            action.Execute(player);

            table.GetCardsInZone(TablePlacementZoneType.PlayerZone)[0].Should().BeEquivalentTo(cards, "the player didn't have any cards to play");
            table.GetCardsInZone(TablePlacementZoneType.PlayerZone)[0].Count.Should().Be(0, "no cards were added to the table");
        }
    }
}
