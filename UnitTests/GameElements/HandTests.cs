using FluentAssertions;
using DeckForge.GameElements.Resources;

namespace UnitTests.GameElements
{
    [TestClass]
    public class HandTests
    {
        [TestMethod]
        public void HandCanBeGivenCard() {
            Hand h = new();

            h.AddResource(new Card(21, "W"));

            h.CurrentHandSize.Should().Be(1, "a card was added to the hand");
        }

        [TestMethod]
        public void HandCanGiveCardAt_ValidPos()
        {
            Hand h = new();

            h.AddResource(new Card(21, "W"));
            Card c = h.GetCardAt(0);

            c.val.Should().Be(21, "the card at 0 pos in the hand was the card 21W");
        }

        [TestMethod]
        public void HandCannotGetCardAt_InvalidPos() {
            Hand h = new();

            h.AddResource(new Card(21, "W"));
            Action a = () => h.GetCardAt(1);

            a.Should().Throw<ArgumentOutOfRangeException>("there is no card at pos 1");
        }

        [TestMethod]
        public void RemoveSpecificCardInHand() {
            Hand h = new();
            Card c = new Card(21, "W");

            h.AddResource(c);
            h.RemoveResource(c);

            h.CurrentHandSize.Should().Be(0, "the card 21W was removed from the hand");
        }

        [TestMethod]
        public void HandCannotDecrementOrIncrement() {
            Hand h = new();

            Action a = () => h.IncrementResourceCollection();
            Action b = () => h.DecrementResourceCollection();

            a.Should().Throw<NotImplementedException>("a hand cannot be incremented without a resource specified");
            b.Should().Throw<NotImplementedException>("a hand cannot be decremented wihtout a resource specified");
        }

        [TestMethod]
        public void HandCannotGainResource() {
            Hand h = new();

            Action a = () => h.GainResource();

            a.Should().Throw<NotImplementedException>("a player cannot gain a resource from a hand");
        }

        [TestMethod]
        public void HandCanClearCollection() {
            Hand h = new();

            h.AddResource(new Card(21, "W"));
            h.ClearCollection();

            h.CurrentHandSize.Should().Be(0, "hand had its cards removed cleared from it");
        }
    }
}
