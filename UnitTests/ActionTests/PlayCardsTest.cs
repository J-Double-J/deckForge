using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.ActionTests
{
    [TestClass]
    public class PlayCardsTest
    {
        [TestMethod]
        public void PlayAction_MakesPlayerPlayCards()
        {
            bool eventRaised = false;
            IGameMediator gm = new BaseGameMediator(1);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            IPlayer p = new BasePlayer(gm);
            PlayerGameAction action = new PlayCardAction();
            p.DrawStartingHand(TablePlacementZoneType.PlayerZone);

            // StringWriter and Reader are for the console.
            var sr = new StringReader("1");

            Console.SetIn(sr);

            p.PlayerPlayedCard += (sender, e) => eventRaised = true;

            action.Execute(p);
            p.HandSize.Should().Be(4, "player played a card from their hand");
            eventRaised.Should().Be(true, "player should raise an event whenever it plays a card");
        }

        [TestMethod]
        public void UnsupportedExecutes_ThrowErrors()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 1, decks);
            Table table = new(gm, new List<TableZone>() { zone });
            IPlayer p = new BasePlayer(gm);
            IPlayer p2 = new BasePlayer(gm);
            IPlayer p3 = new BasePlayer(gm);
            List<IPlayer> targetPlayers = new List<IPlayer>
            {
                p2, p3
            };

            PlayerGameAction action = new PlayCardAction();

            Action a = () => action.Execute(p, p2);
            Action b = () => action.Execute(p, targetPlayers);

            a.Should().Throw<NotSupportedException>("this method does not allow Players to target plays against one another");
            b.Should().Throw<NotSupportedException>("this method does not allow Players to target plays against one another");
        }
    }
}
