using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;
using DeckForge.GameElements.Resources.Cards.Example_Cards;
using DeckForge.PlayerConstruction;
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

        [TestMethod]
        public void CardGainedEvenWithOverpayment_IfSpecified()
        {
            ICardWithCost card = new BarBrawlerCharacterCardWithCost(new BaseGameMediator(0)); // Needs 2 mana.
            Dictionary<Type, int> payment = new() { { typeof(Mana), 2 }, { typeof(BasePlayer), 1 } };

            card.PayCostAndGetRemainingResources(payment, out var _).Should().BeEquivalentTo(card, "card is gained because minimum payment is provided");
        }

        [TestMethod]
        public void RemainderGivenWithOverpayment_IfSpecified()
        {
            ICardWithCost card = new BarBrawlerCharacterCardWithCost(new BaseGameMediator(0));
            Dictionary<Type, int> payment = new() { { typeof(Mana), 2 }, { typeof(BasePlayer), 1 } };
            Dictionary<Type, int> remainderValidation = new() { { typeof(Mana), 0 }, { typeof(BasePlayer), 1 } };

            card.PayCostAndGetRemainingResources(payment, out var remainder);
            remainder.Should().BeEquivalentTo(remainderValidation, "these are the remaining resources left");
        }

        [TestMethod]
        public void NullCardGainedWithInsufficientResources_EvenWithOverpaymentMethod()
        {
            ICardWithCost card = new BarBrawlerCharacterCardWithCost(new BaseGameMediator(0)); // Needs 2 mana.
            Dictionary<Type, int> payment = new() { { typeof(Mana), 1 }, { typeof(BasePlayer), 1 } };

            card.PayCostAndGetRemainingResources(payment, out var _).Should().BeOfType<NullCard>("the incorrect minimum payment count is given");
        }
    }
}
