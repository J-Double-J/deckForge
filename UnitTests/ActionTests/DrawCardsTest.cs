using GameNamespace;
using deckForge.PhaseActions;
using FluentAssertions;
using PlayerNamespace;

namespace UnitTests.ActionTests
{
    [TestClass]
    public class DrawCardsTest
    {
        [TestMethod]
        public void DrawAction_MakesPlayerDrawCard()
        {
            GameAction action = new DrawCardsAction();
            GameMediator gm = new(0);

            Player p = new Player(gm);


            int initHandSize = p.HandSize();


            action.execute(p);
            p.HandSize().Should().Be(initHandSize + 1, "player was told to draw a card.");

            GameAction specifiedDraw = new DrawCardsAction(drawCount: 5);
            specifiedDraw.execute(p);
            p.HandSize().Should().Be(initHandSize + 6, "player was told to draw 5 more cards");
        }

        [TestMethod]
        public void DrawAction_CantDrawFromEmptyDeck()
        {
            GameAction action = new DrawCardsAction(drawCount: 5);
            GameMediator gm = new(0);

            Player p = new Player(gm);

            //TODO: Player on init draws 5 cards, so this adds to 52, test might break when init changes
            for (int i = 0; i < 47; i++)
            {
                p.DrawCard();
            }

            int initHandSize = p.HandSize();

            action.execute(p);
            p.HandSize().Should().Be(52, "the deck was completely drawn from, so there should be no more cards to gain");

        }
    }
}
