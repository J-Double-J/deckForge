using DeckForge.GameElements.Resources.Cards;
using FluentAssertions;

namespace UnitTests.HelperObjectTests
{
    [TestClass]
    public class CostVerifierTests
    {
        [TestMethod]
        public void CostIsPaidForAndVerified()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };

            CostVerifier.VerifyPaymentExactly(cost, payment).Should().Be(true, "cost and payment were the same");
        }

        [TestMethod]
        public void PaymentExactlyFailsEvenIfMorePaidThanCost()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 2 }, { typeof(Coin), 4 } };

            CostVerifier.VerifyPaymentExactly(cost, payment).Should().Be(false, "payment is overpayed for the cost");
        }

        [TestMethod]
        public void PaymentFailsIfMinimumNotCovered()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 1 }, { typeof(Coin), 3 } };

            CostVerifier.VerifyPaymentExactly(cost, payment).Should().Be(false, "payment does not equal cost");
        }

        [TestMethod]
        public void PaymentSucceedsIfMinimumPaymentIsGiven()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };

            CostVerifier.VerifyMinimumPayment(cost, payment, out _).Should().Be(true, "payment is the same as cost");
        }

        [TestMethod]
        public void PaymentSucceedsIfOverpaymentIsGiven()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 3 }, { typeof(Coin), 4 } };

            CostVerifier.VerifyMinimumPayment(cost, payment, out _).Should().Be(true, "payment is the same as cost");
        }

        [TestMethod]
        public void VerifyMinimumPaymentMethod_FailsOnLackOfMinimumPayment()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 3 }, { typeof(Coin), 2 } };

            CostVerifier.VerifyMinimumPayment(cost, payment, out _).Should().Be(false, "payment is the same as cost");
        }

        [TestMethod]
        public void ExactPaymentDoesNotAcceptAdditionalResources_EvenIfUnneeded()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 }, { typeof(Sheep), 4 } };

            CostVerifier.VerifyPaymentExactly(cost, payment).Should().Be(false, "sheep are not needed for the cost");
        }

        [TestMethod]
        public void MinimumPaymentAcceptsAdditionalResources_EvenIfUnneeded()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 }, { typeof(Sheep), 4 } };

            CostVerifier.VerifyMinimumPayment(cost, payment, out _).Should().Be(true, "payment is covered even if sheep are not needed for the cost");
        }

        [TestMethod]
        public void MinimumPayment_ReturnsCorrectDifferenceForExact()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> targetOverpayment = new() { { typeof(Wood), 0 }, { typeof(Coin), 0 } };

            CostVerifier.VerifyMinimumPayment(cost, payment, out var overpayment);
            overpayment.Should().BeEquivalentTo(targetOverpayment, "all resources were paid for exactly");
        }

        [TestMethod]
        public void MinimumPayment_ReturnsCorrectDifference()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 3 }, { typeof(Coin), 5 } };
            Dictionary<Type, int> targetOverpayment = new() { { typeof(Wood), 1 }, { typeof(Coin), 2 } };

            CostVerifier.VerifyMinimumPayment(cost, payment, out var overpayment);
            overpayment.Should().BeEquivalentTo(targetOverpayment, "all resources were paid for exactly");
        }

        [TestMethod]
        public void MinimumPayment_ReturnsCorrectDifference_WithExtraResources()
        {
            Dictionary<Type, int> cost = new() { { typeof(Wood), 2 }, { typeof(Coin), 3 } };
            Dictionary<Type, int> payment = new() { { typeof(Wood), 3 }, { typeof(Coin), 5 }, { typeof(Sheep), 3 } };
            Dictionary<Type, int> targetOverpayment = new() { { typeof(Wood), 1 }, { typeof(Coin), 2 }, { typeof(Sheep), 3 } };

            CostVerifier.VerifyMinimumPayment(cost, payment, out var overpayment);
            overpayment.Should().BeEquivalentTo(targetOverpayment, "all resources were paid for exactly");
        }

        // Dummy resources
        private class Wood
        {
            public Wood()
            {
            }
        }

        private class Coin
        {
            public Coin()
            {
            }
        }

        private class Sheep
        {
            public Sheep()
            {
            }
        }
    }
}
