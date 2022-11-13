using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.ActionTests
{
    [TestClass]
    public class DealCardsFromTableDeckActionTests
    {
        [TestMethod]
        public void CardsAreDealtFromTableDeckToEachPlayer()
        {
            IGameMediator gm = new BaseGameMediator(1);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 3, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            List<IPlayer> players = new()
            {
                new BasePlayer(gm, 0),
                new BasePlayer(gm, 1),
                new BasePlayer(gm, 2)
            };

            IGameAction action = new DealCardsFromTableDeckToPlayers(gm, 2, TablePlacementZoneType.PlayerZone);
            action.Execute();

            foreach (IPlayer player in players)
            {
                player.HandSize.Should().Be(2, "each player was dealt 2 cards");
            }
        }
    }
}
