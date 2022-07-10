using deckForge.PhaseActions;
using FluentAssertions;
using deckForge.GameConstruction;
using deckForge.PlayerConstruction;

namespace UnitTests.ActionTests
{
    [TestClass]
    public class DrawCardsTest
    {
        [TestMethod]
        public void DrawAction_MakesPlayerDrawCard()
        {
            IGameMediator gm = new BaseGameMediator(0);
            IPlayer p = new BasePlayer(gm);
            PlayerGameAction action = new DrawCardsAction();



            int initHandSize = p.HandSize;


            action.execute(p);
            p.HandSize.Should().Be(initHandSize + 1, "player was told to draw a card.");

            PlayerGameAction specifiedDraw = new DrawCardsAction(drawCount: 5);
            specifiedDraw.execute(p);
            p.HandSize.Should().Be(initHandSize + 6, "player was told to draw 5 more cards");
        }

        [TestMethod]
        public void DrawAction_CantDrawFromEmptyDeck()
        {
            IGameMediator gm = new BaseGameMediator(0);
            IPlayer p = new BasePlayer(gm);
            PlayerGameAction action = new DrawCardsAction(drawCount: 5);


            //TODO: Player on init draws 5 cards, so this adds to 52, test might break when init changes
            for (int i = 0; i < 47; i++)
            {
                p.DrawCard();
            }

            int initHandSize = p.HandSize;

            action.execute(p);
            p.HandSize.Should().Be(52, "the deck was completely drawn from, so there should be no more cards to gain");
        }

        [TestMethod]
        public void UnsupportedExecutes_ThrowErrors()
        {
            IGameMediator gm = new BaseGameMediator(0);
            IPlayer p = new BasePlayer(gm);
            IPlayer p2 = new BasePlayer(gm);
            IPlayer p3 = new BasePlayer(gm);
            List<IPlayer> targetPlayers = new List<IPlayer>{
                p2, p3
            };

            PlayerGameAction action = new DrawCardsAction(drawCount: 5);

            Action a = () => action.execute(p, p2);
            Action b = () => action.execute(p, targetPlayers);

            a.Should().Throw<NotSupportedException>("this method does not allow Players to target draws against one another");
            b.Should().Throw<NotSupportedException>("this method does not allow Players to target draws against one another");
        }
    }
}
