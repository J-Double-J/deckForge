using DeckForge.GameElements.Resources.Cards.CardEvents;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// Interface shared among all <see cref="ICard"/>s.
    /// </summary>
    public interface ICard
    {
        /// <summary>
        /// Event for if the card is removed from the <see cref="Table"/>.
        /// </summary>
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
        /// Gets or sets a value indicating whether the <see cref="ICard"/> is currently active.
        /// Starts inactive on creation. Being active usually means the card will execute effects, listen to events, and
        /// follow standard rules for when a card is "in play". An inactive card will have limited
        /// functionality based on the rules (such as being in a <see cref="IPlayer"/>'s hand).
        /// </summary>
        public bool CardActive { get; set; }

        /// <summary>
        /// Gets the string representing the card.
        /// </summary>
        /// <returns>Returns the string representation of the card.</returns>
        public string PrintCard();

        /// <summary>
        /// Swaps the boolean of <see cref="Facedown"/>.
        /// </summary>
        public void Flip();

        /// <summary>
        /// Executes whenever this card is played.
        /// </summary>
        /// <param name="placementDetails">Details of where the <see cref="ICard"/> was played.</param>
        public void OnPlay(CardPlacedOnTableDetails placementDetails);

        /// <summary>
        /// Executes after this card is removed from the <see cref="Table"/>.
        /// </summary>
        public void OnRemoval();
    }
}