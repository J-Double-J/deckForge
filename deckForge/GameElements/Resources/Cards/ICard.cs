using DeckForge.GameElements.Resources.Cards.CardEvents;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// Interface shared among all <see cref="ICard"/>s.
    /// </summary>
    public interface ICard
    {
        event EventHandler<CardIsRemovedFromTableEventArgs> CardIsRemovedFromTable;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ICard"/> is facedown or not. If true,
        /// <see cref="ICard"/> is facedown.
        /// </summary>
        public bool Facedown { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IPlayer"/> who owns this <see cref="ICard"/>.
        /// </summary>
        public IPlayer? OwnedBy { get; set; }

        /// <summary>
        /// Gets the string representing the card.
        /// </summary>
        /// <returns>Returns the string representation of the card.</returns>
        public string PrintCard();

        /// <summary>
        /// Swaps the boolean of <see cref="Facedown"/>.
        /// </summary>
        public void Flip();
    }
}