using DeckForge.GameConstruction;
using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
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
            ITable table = new Table(gm, 3, new DeckOfPlayingCards());
            List<IPlayer> players = new()
            {
                new BasePlayer(gm, 0),
                new BasePlayer(gm, 1),
                new BasePlayer(gm, 2)
            };

            IGameAction action = new DealCardsFromTableDeckToPlayers(gm, 0, 2, "Test Deal", "Test");
            action.Execute();

            foreach (IPlayer player in players)
            {
                player.HandSize.Should().Be(2, "each player was dealt 2 cards");
            }
        }
    }
}
