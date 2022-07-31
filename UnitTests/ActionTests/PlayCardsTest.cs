using DeckForge.PhaseActions;
using FluentAssertions;
using DeckForge.GameConstruction;
using DeckForge.PlayerConstruction;
using DeckForge.GameElements;
using DeckForge.GameElements.Resources;

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
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 1, decks);
            IPlayer p = new BasePlayer(gm);
            p.DrawStartingHand();
            PlayerGameAction action = new PlayCardsAction();

            //StringWriter and Reader are for the console.
            var sr = new StringReader("0");

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
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 0, decks);
            IPlayer p = new BasePlayer(gm);
            IPlayer p2 = new BasePlayer(gm);
            IPlayer p3 = new BasePlayer(gm);
            List<IPlayer> targetPlayers = new List<IPlayer>{
                p2, p3
            };

            PlayerGameAction action = new PlayCardsAction();

            Action a = () => action.Execute(p, p2);
            Action b = () => action.Execute(p, targetPlayers);

            a.Should().Throw<NotSupportedException>("this method does not allow Players to target plays against one another");
            b.Should().Throw<NotSupportedException>("this method does not allow Players to target plays against one another");
        }
    }
}
