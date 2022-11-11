using DeckForge.GameElements.Resources.Cards.CardEvents;
using DeckForge.GameElements.Resources.Cards.CardTraits;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// Base class for all cards.
    /// </summary>
    public abstract class Card : ICard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="facedown">Sets the <see cref="Facedown"/> property of <see cref="Card"/>.</param>
        public Card(bool facedown = true)
        {
            Facedown = facedown;
            CardActive = false;
            Traits = new();
        }

        /// <summary>
        /// Event is raised whenever this card is about to be removed from the <see cref="ITable"/>.
        /// Used to infrom the <see cref="ITable"/> to remove this <see cref="Card"/>.
        /// </summary>
        public event EventHandler<CardIsRemovedFromTableEventArgs>? CardIsRemovedFromTable;

        /// <inheritdoc/>
        public bool Facedown { get; set; }

        /// <inheritdoc/>
        public virtual IPlayer? OwnedBy { get; set; }

        /// <inheritdoc/>
        public bool CardActive { get; set; }

        /// <inheritdoc/>
        public CardPlacedOnTableDetails? TablePlacementDetails { get; protected set; }

        /// <inheritdoc/>
        public IReadOnlyList<BaseCardTrait> CardTraits
        {
            get { return Traits; }
        }

        /// <summary>
        /// Gets a mutable list of <see cref="BaseCardTrait"/>s on this card.
        /// </summary>
        protected List<BaseCardTrait> Traits { get; }

        /// <inheritdoc/>
        public void Flip()
        {
            if (Facedown)
            {
                Facedown = false;
            }
            else
            {
                Facedown = true;
            }
        }

        /// <inheritdoc/>
        public abstract string PrintCard();

        /// <inheritdoc/>
        public virtual void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
            TablePlacementDetails = placementDetails;
            CardActive = true;

            TriggerAllTraitsOnPlay();
        }

        /// <inheritdoc/>
        public virtual void OnPlace(CardPlacedOnTableDetails placementDetails)
        {
            TablePlacementDetails = placementDetails;

            TriggerAllTraitsOnPlace();
        }

        /// <inheritdoc/>
        public virtual void OnRemoval()
        {
            TriggerAllTraitsOnCardRemoved();
        }

        /// <summary>
        /// Invokes the CardIsRemovedFromTableEventHandler. Default informs Table to remove <see cref="ICard"/>.
        /// </summary>
        /// <param name="e">Args for <see cref="CardIsRemovedFromTableEventArgs"/>.</param>
        protected virtual void OnCardIsRemovedFromTable(CardIsRemovedFromTableEventArgs e)
        {
            CardIsRemovedFromTable?.Invoke(this, e);
        }

        /// <summary>
        /// Triggers all card traits OnPlay() methods.
        /// </summary>
        protected virtual void TriggerAllTraitsOnPlay()
        {
            foreach (BaseCardTrait trait in Traits)
            {
                trait.OnPlay();
            }
        }

        /// <summary>
        /// Triggers all card traits OnPlace() methods.
        /// </summary>
        protected virtual void TriggerAllTraitsOnPlace()
        {
            foreach (BaseCardTrait trait in Traits)
            {
                trait.OnPlace();
            }
        }

        /// <summary>
        /// Triggers all card traits OnCardRemoval() methods.
        /// </summary>
        protected virtual void TriggerAllTraitsOnCardRemoved()
        {
            foreach (BaseCardTrait trait in Traits)
            {
                trait.OnCardRemoval();
            }
        }

        /// <summary>
        /// Triggers all card traits OnTraitRemoved() methods.
        /// </summary>
        protected virtual void TriggerAllTraitsOnTraitRemoved()
        {
            foreach (BaseCardTrait trait in Traits)
            {
                trait.OnTraitRemoved();
            }
        }
    }
}
