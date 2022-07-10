using deckForge.GameElements.Resources;
using FluentAssertions;

namespace UnitTests.GameElements
{
    [TestClass]
    public class CardTests
    {
        [TestMethod]
        public void FlipCardNumerousTimes()
        {
            Card c = new(8, "J");

            c.Flip();
            c.Facedown.Should().Be(false, "cards start facedown, and it was flipped");

            c.Flip();
            c.Facedown.Should().Be(true, "the card was flipped again");
        }

        [TestMethod]
        public void CardPrintsValuesWhenFaceUp()
        {
            Card c = new(8, "J", facedown: false);

            c.PrintCard().Should().Be("8J", "the card is faceup right now and is easily read");
        }

        public void CardIsCoveredWhenFaceDown()
        {
            Card c = new(8, "J");

            c.PrintCard().Should().Be("COVERED", "the card is facedown right now, so it should not be clear what the card's value is");
        }

    }
}
