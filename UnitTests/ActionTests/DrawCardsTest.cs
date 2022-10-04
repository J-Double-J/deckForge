using DeckForge.PhaseActions;
using FluentAssertions;
using DeckForge.GameConstruction;
using DeckForge.PlayerConstruction;
using DeckForge.GameElements;
using DeckForge.GameElements.Resources;

namespace UnitTests.ActionTests
{
    [TestClass]
    public class DrawCardsTest
    {
        [TestMethod]
        public void DrawAction_MakesPlayerDrawCard()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            Table table = new(gm, 0, decks);
            IPlayer p = new BasePlayer(gm);
            PlayerGameAction action = new DrawCardsAction();

            int initHandSize = p.HandSize;

            action.Execute(p);
            p.HandSize.Should().Be(initHandSize + 1, "player was told to draw a card.");

            PlayerGameAction specifiedDraw = new DrawCardsAction(drawCount: 5);
            specifiedDraw.Execute(p);
            p.HandSize.Should().Be(initHandSize + 6, "player was told to draw 5 more cards");
        }

        [TestMethod]
        public void DrawAction_CantDrawFromEmptyDeck()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            Table table = new(gm, 0, decks);
            IPlayer p = new BasePlayer(gm);
            PlayerGameAction action = new DrawCardsAction(drawCount: 5);

            // TODO: Player on init draws 5 cards, so this adds to 52, test might break when init changes
            for (int i = 0; i < 47; i++)
            {
                p.DrawCard();
            }

            int initHandSize = p.HandSize;

            action.Execute(p);
            p.HandSize.Should().Be(52, "the deck was completely drawn from, so there should be no more cards to gain");
        }

        [TestMethod]
        public void UnsupportedExecutes_ThrowErrors()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            Table table = new(gm, 0, decks);
            IPlayer p = new BasePlayer(gm);
            IPlayer p2 = new BasePlayer(gm);
            IPlayer p3 = new BasePlayer(gm);
            List<IPlayer> targetPlayers = new List<IPlayer>
            {
                p2, p3
            };

            PlayerGameAction action = new DrawCardsAction(drawCount: 5);

            Action a = () => action.Execute(p, p2);
            Action b = () => action.Execute(p, targetPlayers);

            a.Should().Throw<NotSupportedException>("this method does not allow Players to target draws against one another");
            b.Should().Throw<NotSupportedException>("this method does not allow Players to target draws against one another");
        }
    }
}
