using DeckForge.HelperObjects;

namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// Cost manager for <see cref="ICard"/>s that validate costs and payments.
    /// </summary>
    public class CardCostManager
    {
        public CardCostManager(ICardWithCost card)
        {
            ManagedCard = card;
        }

        /// <summary>
        /// Gets the <see cref="ICardWithCost"/> this <see cref="CardCostManager"/> is attached to.
        /// </summary>
        public ICardWithCost ManagedCard { get; }

        /// <summary>
        /// Gets the listed cost of the <see cref="ManagedCard"/>.
        /// </summary>
        public Dictionary<Type, int> Cost
        {
            get { return ManagedCard.Cost; }
        }

        public bool PayForCostExact(Dictionary<Type, int> payment, bool verifiedAlready = false)
        {
            if (verifiedAlready)
            {
                ZeroOutResources(payment);
                return true;
            }
            else
            {
                if (CostVerifier.VerifyPaymentExactly(Cost, payment))
                {
                    ZeroOutResources(payment);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool PayForCostWithChange(Dictionary<Type, int> payment, out Dictionary<Type, int>? remainder)
        {
            if (CostVerifier.VerifyMinimumPayment(Cost, payment, out remainder))
            {
                payment = remainder!;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a <see cref="string"/> that represents the cost of the <see cref="ICard"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the cost of the <see cref="ICard"/>.</returns>
        public string GetCostAsString()
        {
            string retString = string.Empty;

            foreach (var pair in Cost)
            {
                retString += $"{pair.Key}: {pair.Value}\n";
            }

            return retString;
        }

        private void ZeroOutResources(Dictionary<Type, int> payment)
        {
            foreach (var pair in payment)
            {
                payment[pair.Key] = 0;
            }
        }
    }
}
