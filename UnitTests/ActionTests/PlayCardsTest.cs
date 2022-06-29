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
    }
}
