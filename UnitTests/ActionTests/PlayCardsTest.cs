using deckForge.PhaseActions;
using FluentAssertions;
using deckForge.GameConstruction;
using deckForge.PlayerConstruction;
using deckForge.GameElements;
using deckForge.GameElements.Resources;

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
            PlayerGameAction action = new PlayCardsAction();

            //StringWriter and Reader are for the console.
            var sr = new StringReader("0");

            Console.SetIn(sr);

            p.PlayerPlayedCard += (sender, e) => eventRaised = true;

            action.execute(p);
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

            Action a = () => action.execute(p, p2);
            Action b = () => action.execute(p, targetPlayers);

            a.Should().Throw<NotSupportedException>("this method does not allow Players to target plays against one another");
            b.Should().Throw<NotSupportedException>("this method does not allow Players to target plays against one another");
        }
    }
}
