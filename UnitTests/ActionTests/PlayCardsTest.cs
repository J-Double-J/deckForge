using deckForge.PhaseActions;
using FluentAssertions;
using deckForge.GameConstruction;
using deckForge.PlayerConstruction;

namespace UnitTests.ActionTests
{
    [TestClass]
    public class PlayCardsTest
    {
        [TestMethod]
        public void PlayAction_MakesPlayerPlayCards()
        {
            bool eventRaised = false;
            GameMediator gm = new(1);
            Player p = new Player(gm);
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
            GameMediator gm = new(0);
            Player p = new Player(gm);
            Player p2 = new Player(gm);
            Player p3 = new Player(gm);
            List<Player> targetPlayers = new List<Player>{
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
