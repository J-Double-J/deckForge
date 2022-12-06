using DeckForge.GameElements.Resources.Cards.CardEvents;
using DeckForge.GameElements.Resources.Cards.CardTraits;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// A card that is functionally null and does nothing.
    /// </summary>
    public class NullCard : ICard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullCard"/> class.
        /// </summary>
        public NullCard()
        {
            Facedown = true;
            OwnedBy = null;
            CardActive = false;
        }

        /// <inheritdoc/>
        public event EventHandler<CardIsRemovedFromTableEventArgs>? CardIsRemovedFromTable;

        /// <inheritdoc/>
        public bool Facedown { get; set; }

        /// <inheritdoc/>
        public IPlayer? OwnedBy { get; set; }

        /// <inheritdoc/>
        public bool CardActive { get; set; }

        /// <inheritdoc/>
        public CardPlacedOnTableDetails? TablePlacementDetails { get; }

        /// <inheritdoc/>
        public IReadOnlyList<ICardTrait> CardTraits
        {
            get { return new List<BaseCardTrait>(); }
        }

        /// <inheritdoc/>
        public void Flip()
        {
        }

        /// <inheritdoc/>
        public void Flip(bool facedown)
        {
        }

        /// <inheritdoc/>
        public void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
        }

        /// <inheritdoc/>
        public void OnPlace(CardPlacedOnTableDetails placementDetails)
        {
        }

        /// <inheritdoc/>
        public void OnRemoval()
        {
        }

        /// <inheritdoc/>
        public string PrintCard()
        {
            return string.Empty;
        }
    }
}
