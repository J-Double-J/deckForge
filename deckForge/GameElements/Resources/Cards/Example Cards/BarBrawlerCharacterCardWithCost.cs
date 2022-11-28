using DeckForge.GameConstruction;

namespace DeckForge.GameElements.Resources.Cards.Example_Cards
{
    /// <summary>
    /// A Bar Brawler card that has some resource cost to gain.
    /// </summary>
    public class BarBrawlerCharacterCardWithCost : BarBrawlerCharacterCard, ICardWithCost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BarBrawlerCharacterCardWithCost"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        public BarBrawlerCharacterCardWithCost(IGameMediator gm)
            : base(gm)
        {
            Cost = new() { { typeof(Mana), 2 } };
            CostManager = new(this);
        }

        /// <inheritdoc/>
        public Dictionary<Type, int> Cost { get; private set; }

        private CardCostManager CostManager { get; }

        /// <inheritdoc/>
        public string GetCostAsString()
        {
            return CostManager.GetCostAsString();
        }

        /// <inheritdoc/>
        public ICard PayCostAndGetRemainingResources(Dictionary<Type, int> payment, out Dictionary<Type, int>? remainder)
        {
            return CostManager.PayForCostWithChange(payment, out remainder) ? this : new NullCard();
        }

        /// <inheritdoc/>
        public ICard PayCostExactly(Dictionary<Type, int> payment, bool verifiedPayment = false)
        {
            return CostManager.PayForCostExact(payment, false) ? this : new NullCard();
        }
    }
}
