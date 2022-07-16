using FluentAssertions;
using deckForge.GameElements.Resources;

namespace UnitTests.GameElements


{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void DeckShrinks_OnCardDraw()
        {
            Deck deck = new();

            int initSize = deck.Count;
            deck.DrawCard();

            deck.Count.Should().Be(initSize - 1, "a card was drawn");
            //Assert.IsTrue((deck.Size() == initSize - 1), "Deck did not shrink on card draw");
        }

        [TestMethod]
        public void CardDrawReturnsNull_WhenDeckIsEmpty()
        {
            Deck deck = new();

            deck.DrawMultipleCards(52);

            Card? c = deck.DrawCard();
            c.Should().BeNull("there are no more cards left in the deck");
        }

        [TestMethod]
        [DataRow(5)]
        public void CardsWereDrawn_FromDeckXTimes(int count)
        {
            Deck deck = new();
            List<Card>? cards = new();

            cards = deck.DrawMultipleCards(count);

            cards!.Count().Should().Be(count);
        }

        [TestMethod]
        [DataRow(5)]
        public void CardsWereDrawn_FromNearEmptyDeckXTimes(int count)
        {
            Deck deck = new();
            List<Card>? cards = new();

            deck.DrawMultipleCards(49);
            int initDeckSize = deck.Count;

            cards = deck.DrawMultipleCards(count);

            cards!.Count.Should().Be(initDeckSize, $"only {initDeckSize} cards were left in the deck");
        }

        [TestMethod]
        public void AddCardToTopOfDeck()
        {
            Deck deck = new();
            Card c = new(99, "W");

            deck.AddCardToDeck(c, pos: "top");
            Card drawn = deck.DrawCard()!;

            drawn.val.Should().Be(99, "the special card was added to the top of the deck");
        }

        [TestMethod]
        public void AddCardToBottomOfDeck()
        {
            Deck deck = new();
            Card c = new(99, "W");

            deck.AddCardToDeck(c, pos: "bottom");
            for (var i = 0; i < 52; i++)
            {
                deck.DrawCard();
            }
            Card drawn = deck.DrawCard()!;

            drawn.val.Should().Be(99, "the special card was added to the bottom of the deck");
        }

        [TestMethod]
        public void AddCardToMiddleOfDeck()
        {
            Deck deck = new();
            Card c = new(99, "W");

            deck.AddCardToDeck(c, pos: "middle");
            for (var i = 0; i < 26; i++)
            {
                deck.DrawCard();
            }
            Card drawn = deck.DrawCard()!;

            drawn.val.Should().Be(99, "the special card was added to the middle of the deck");
        }

        [TestMethod]
        public void AddCardAtSpecificPosition()
        {
            Deck deck = new();
            Card c = new(99, "W");

            deck.AddCardToDeck(c, pos: "50");
            for (var i = 0; i < 2; i++)
            {
                deck.DrawCard();
            }
            Card drawn = deck.DrawCard()!;
            drawn.val.Should().Be(99, "the special card was put 2 cards from the top");
        }

        [TestMethod]
        public void AddMultipleCardsToTopOfDeck()
        {
            Deck deck = new();
            List<Card> cards = new List<Card> { new Card(100, "W"), new Card(101, "W"), new Card(102, "W") };
            bool match = true;

            deck.AddMultipleCardsToDeck(cards, pos: "top");
            List<Card> drawnCards = new();
            for (var i = 0; i < 3; i++)
            {
                drawnCards.Add(deck.DrawCard()!);
            }

            for (var i = 0; i < 3; i++)
            {
                if (drawnCards[i] != cards[2 - i])
                {
                    match = false;
                }
            }

            match.Should().Be(true, "all the cards should be drawn from the top in the reverse order they were placed");
        }

        //Testing IResourceCollection Implementation

        [TestMethod]
        public void AddResouceToDeck() {
            Card c = new(21, "J");
            Deck d = new Deck(defaultAddCardPos: "top");

            d.AddResource(c);
            var cardDrawn = d.DrawCard();

            c.val.Should().Be(21, "the card '21J' was added to the top of the deck and then drawn");
        }

        [TestMethod]
        public void RemoveResourceFromDeck() {
            Card c = new(21, "J");
            Deck d = new Deck(defaultAddCardPos: "top");

            d.AddResource(c);
            d.RemoveResource(c);

            d.Count.Should().Be(52, "a card resource was added and then removed from the deck");
        }

        [TestMethod]
        public void DeckCannotIncrementResourceCollection() {
            Deck d = new Deck();

            Action a = () => { d.IncrementResourceCollection(); };

            a.Should().Throw<NotImplementedException>("it doesn't make sense to increment the deck size without a card");
        }

        [TestMethod]
        public void DeckDecrementsDeckSize() {
            Deck d = new Deck();

            d.DecrementResourceCollection();

            d.Count.Should().Be(51, "a card was removed from the deck");
        }
    }
}
