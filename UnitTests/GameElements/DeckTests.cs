using FluentAssertions;
using DeckForge.GameElements.Resources;

namespace UnitTests.GameElements


{
    [TestClass]
    public class DeckTests
    {
        [TestMethod]
        public void DeckShrinks_OnCardDraw()
        {
            DeckOfPlayingCards deck = new();

            int initSize = deck.Count;
            deck.DrawCard();

            deck.Count.Should().Be(initSize - 1, "a card was drawn");
            //Assert.IsTrue((deck.Size() == initSize - 1), "Deck did not shrink on card draw");
        }

        [TestMethod]
        public void CardDrawReturnsNull_WhenDeckIsEmpty()
        {
            DeckOfPlayingCards deck = new();

            deck.DrawMultipleCards(52);

            PlayingCard? c = deck.DrawCard();
            c.Should().BeNull("there are no more cards left in the deck");
        }

        [TestMethod]
        [DataRow(5)]
        public void CardsWereDrawn_FromDeckXTimes(int count)
        {
            DeckOfPlayingCards deck = new();
            List<PlayingCard>? cards = new();

            cards = deck.DrawMultipleCards(count);

            cards!.Count().Should().Be(count);
        }

        [TestMethod]
        [DataRow(5)]
        public void CardsWereDrawn_FromNearEmptyDeckXTimes(int count)
        {
            DeckOfPlayingCards deck = new();
            List<PlayingCard>? cards = new();

            deck.DrawMultipleCards(49);
            int initDeckSize = deck.Count;

            cards = deck.DrawMultipleCards(count);

            cards!.Count.Should().Be(initDeckSize, $"only {initDeckSize} cards were left in the deck");
        }

        [TestMethod]
        public void AddCardToTopOfDeck()
        {
            DeckOfPlayingCards deck = new();
            PlayingCard c = new(99, "W");

            deck.AddCardToDeck(c, pos: "top");
            PlayingCard drawn = deck.DrawCard()!;

            drawn.Val.Should().Be(99, "the special card was added to the top of the deck");
        }

        [TestMethod]
        public void AddCardToBottomOfDeck()
        {
            DeckOfPlayingCards deck = new();
            PlayingCard c = new(99, "W");

            deck.AddCardToDeck(c, pos: "bottom");
            for (var i = 0; i < 52; i++)
            {
                deck.DrawCard();
            }
            PlayingCard drawn = deck.DrawCard()!;

            drawn.Val.Should().Be(99, "the special card was added to the bottom of the deck");
        }

        [TestMethod]
        public void AddCardToMiddleOfDeck()
        {
            DeckOfPlayingCards deck = new();
            PlayingCard c = new(99, "W");

            deck.AddCardToDeck(c, pos: "middle");
            for (var i = 0; i < 26; i++)
            {
                deck.DrawCard();
            }
            PlayingCard drawn = deck.DrawCard()!;

            drawn.Val.Should().Be(99, "the special card was added to the middle of the deck");
        }

        [TestMethod]
        public void AddCardAtSpecificPosition()
        {
            DeckOfPlayingCards deck = new();
            PlayingCard c = new(99, "W");

            deck.AddCardToDeck(c, pos: "50");
            for (var i = 0; i < 2; i++)
            {
                deck.DrawCard();
            }
            PlayingCard drawn = deck.DrawCard()!;
            drawn.Val.Should().Be(99, "the special card was put 2 cards from the top");
        }

        [TestMethod]
        public void AddMultipleCardsToTopOfDeck()
        {
            DeckOfPlayingCards deck = new();
            List<PlayingCard> cards = new List<PlayingCard> { new PlayingCard(100, "W"), new PlayingCard(101, "W"), new PlayingCard(102, "W") };
            bool match = true;

            deck.AddMultipleCardsToDeck(cards, pos: "top");
            List<PlayingCard> drawnCards = new();
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
        public void AddResouceToDeck()
        {
            PlayingCard c = new(21, "J");
            DeckOfPlayingCards d = new DeckOfPlayingCards(defaultAddCardPos: "top");

            d.AddResource(c);
            var cardDrawn = d.DrawCard();

            c.Val.Should().Be(21, "the card '21J' was added to the top of the deck and then drawn");
        }

        [TestMethod]
        public void RemoveResourceFromDeck()
        {
            PlayingCard c = new(21, "J");
            DeckOfPlayingCards d = new DeckOfPlayingCards(defaultAddCardPos: "top");

            d.AddResource(c);
            d.RemoveResource(c);

            d.Count.Should().Be(52, "a card resource was added and then removed from the deck");
        }

        [TestMethod]
        public void DeckCannotIncrementResourceCollection()
        {
            DeckOfPlayingCards d = new DeckOfPlayingCards();

            Action a = () => { d.IncrementResourceCollection(); };

            a.Should().Throw<NotImplementedException>("it doesn't make sense to increment the deck size without a card");
        }

        [TestMethod]
        public void DeckDecrementsDeckSize()
        {
            DeckOfPlayingCards d = new DeckOfPlayingCards();

            d.DecrementResourceCollection();

            d.Count.Should().Be(51, "a card was removed from the deck");
        }

        [TestMethod]
        public void DeckCanGiveResource()
        {
            DeckOfPlayingCards d = new(defaultAddCardPos: "top");

            d.AddResource(new PlayingCard(21, "W"));
            PlayingCard? c = d.GainResource();

            c!.Val.Should().Be(21, "the card 21W was added to the top of the deck");
        }

        [TestMethod]
        public void DeckCanBeCleared()
        {
            DeckOfPlayingCards d = new();

            d.ClearCollection();

            d.Count.Should().Be(0, "the deck was emptied");
        }

        [TestMethod]
        public void DeckCanResourceCount()
        {
            DeckOfPlayingCards d = new();

            d.DrawCard();
            d.DrawCard();
            d.DrawCard();

            d.Count.Should().Be(49, "3 cards were drawn from the deck (52 - 3 = 49)");
        }
    }
}
