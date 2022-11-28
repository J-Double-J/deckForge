namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// A card that has a cost that must be paid to gain.
    /// </summary>
    public interface ICardWithCost : ICard
    {
        /// <summary>
        /// Gets the cost of resources for the <see cref="ICard"/>.
        /// </summary>
        public Dictionary<Type, int> Cost { get; }

        /// <summary>
        /// Pay the required number of resources, and if satisfactory the <see cref="ICard"/>
        /// will be returned from this function.
        /// </summary>
        /// <param name="payment">The resources that are being paid to attempt to buy
        /// the <see cref="ICard"/>.</param>
        /// <param name="verifiedPayment">If <c>true</c> then does not check if payment is incorrect. Default is <c>false</c>.</param>
        /// <returns><see cref="ICard"/> will return itself if the correct resources are supplied, otherwise
        /// will return a <see cref="NullCard"/> on failure.</returns>
        public ICard PayCostExactly(Dictionary<Type, int> payment, bool verifiedPayment = false);

        /// <summary>
        /// Pay the required number of resources, and if satisfactory the <see cref="ICard"/>
        /// will be returned from this function.
        /// </summary>
        /// <param name="payment">The resources that are being paid to attempt to buy
        /// the <see cref="ICard"/>.</param>
        /// <param name="remainder">Remaining resources left after the payment has been processed.</param>
        /// <returns><see cref="ICard"/> will return itself if the minimum number of resources are supplied, otherwise
        /// will return a <see cref="NullCard"/> on failure.</returns>
        public ICard PayCostAndGetRemainingResources(Dictionary<Type, int> payment, out Dictionary<Type, int>? remainder);

        /// <summary>
        /// Gets a <see cref="string"/> that represents the cost of the <see cref="ICard"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the cost of the <see cref="ICard"/>.</returns>
        public string GetCostAsString();
    }
}
