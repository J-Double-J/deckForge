using DeckForge.GameElements.Resources.Cards;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
    /// <summary>
    /// A card with a treasure value.
    /// </summary>
    public interface ITreasureCard : ICardWithCost
    {
        /// <summary>
        /// Gets the treasure value of the <see cref="TreasureCard"/>.
        /// </summary>
        public int TreasureValue { get; }
    }
}
