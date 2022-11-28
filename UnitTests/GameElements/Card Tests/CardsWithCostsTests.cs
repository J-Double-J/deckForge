using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;
using DeckForge.GameElements.Resources.Cards.Example_Cards;
using FluentAssertions;

namespace UnitTests.GameElements.Card_Tests
{
    [TestClass]
    public class CardsWithCostsTests
    {
        [TestMethod]
        public void CardIsGainedWithProperPayment()
        {
            ICardWithCost card = new BarBrawlerCharacterCardWithCost(new BaseGameMediator(0));
            Dictionary<Type, int> payment = new() { { typeof(Mana), 2 } };

            card.PayCostExactly(payment).Should().BeEquivalentTo(card, "the correct payment was given");
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(1)]
        [DataRow(0)]
        public void NullCardIsGainedWithImproperPayment(int manaCount)
        {
            ICardWithCost card = new BarBrawlerCharacterCardWithCost(new BaseGameMediator(0)); // Needs 2 mana.
            Dictionary<Type, int> payment = new() { { typeof(Mana), manaCount } };

            card.PayCostExactly(payment).Should().BeOfType<NullCard>("the incorrect payment count is given");
        }
    }
}
