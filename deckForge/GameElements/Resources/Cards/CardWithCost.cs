using DeckForge.HelperObjects;

namespace DeckForge.GameElements.Resources.Cards
{
    // TODO: Is this class useful?

    /// <summary>
    /// A <see cref="Card"/> that has some cost associated with it to gain.
    /// </summary>
    public abstract class CardWithCost : Card, ICardWithCost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardWithCost"/> class.
        /// </summary>
        /// <param name="cost">Sets initial cost of the <see cref="Card"/>.</param>
        public CardWithCost(Dictionary<Type, int> cost)
            : base()
        {
            Cost = cost;
        }

        /// <inheritdoc/>
        public Dictionary<Type, int> Cost { get; protected set; }

        /// <inheritdoc/>
        public string GetCostAsString()
        {
            string retString = string.Empty;

            foreach (var pair in Cost)
            {
                retString += $"{pair.Key}: {pair.Value}\n";
            }

            return retString;
        }

        /// <inheritdoc/>
        public ICard PayCostAndGetRemainingResources(Dictionary<Type, int> payment, out Dictionary<Type, int>? remainder)
        {
            if (CostVerifier.VerifyMinimumPayment(Cost, payment, out remainder))
            {
                return this;
            }
            else
            {
                return new NullCard();
            }
        }

        /// <inheritdoc/>
        public ICard PayCostExactly(Dictionary<Type, int> payment, bool verifiedPayment = false)
        {
            if (CostVerifier.VerifyPaymentExactly(Cost, payment))
            {
                return this;
            }
            else
            {
                return new NullCard();
            }
        }
    }
}
