using DeckNameSpace;
using CardNamespace;
using FluentAssertions;

namespace UnitTests


{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]

        public void DeckShrinks_OnCardDraw()
        {
            Deck deck = new();

            int initSize = deck.Size();
            deck.DrawCard();

            deck.Size().Should().Be(initSize - 1, "a card was drawn");
            //Assert.IsTrue((deck.Size() == initSize - 1), "Deck did not shrink on card draw");
        }

        [TestMethod]
        public void CardDrawReturnsNull_WhenDeckIsEmpty() {
            Deck deck = new();

            deck.DrawMultipleCards(52);

            Card? c = deck.DrawCard();
            c.Should().BeNull("there are no more cards left in the deck");
        }

        [TestMethod]
        [DataRow(5)]
        public void CardsWereDrawn_FromDeckXTimes(int count) {
            Deck deck = new();
            List<Card>? cards = new();

            cards = deck.DrawMultipleCards(count);

            cards!.Count().Should().Be(count);
        }

        [TestMethod]
        [DataRow(5)]
        public void CardsWereDrawn_FromNearEmptyDeckXTimes(int count) {
            Deck deck = new();
            List<Card>? cards = new();
            
            deck.DrawMultipleCards(49);
            int initDeckSize = deck.Size();

            cards = deck.DrawMultipleCards(count);

            cards!.Count.Should().Be(initDeckSize, $"only {initDeckSize} cards were left in the deck");
        }
    }
}