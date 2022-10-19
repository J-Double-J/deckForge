using DeckForge.GameElements.Resources.Cards.CardEvents;
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

        /// <summary>
        /// Invokes the CardIsRemovedFromTableEventHandler. Default informs Table to remove <see cref="ICard"/>.
        /// </summary>
        /// <param name="e">Args for <see cref="CardIsRemovedFromTableEventArgs"/>.</param>
        protected virtual void OnCardIsRemovedFromTable(CardIsRemovedFromTableEventArgs e)
        {
            CardIsRemovedFromTable?.Invoke(this, e);
        }
    }
}
