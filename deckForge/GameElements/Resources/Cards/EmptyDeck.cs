namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// <see cref="IDeck"/> that starts empty on creation.
    /// </summary>
    public class EmptyDeck : BaseDeck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyDeck"/> class.
        /// </summary>
        /// <param name="defaultAddCardPosition">Specifies where to place a <see cref="ICard"/> by default. Options
        /// are "top", "middle", or "bottom".</param>
        /// <param name="defaultShuffleOnAddCard">If<c>true</c>, shuffles the <see cref="IDeck"/> after adding
        /// any <see cref="ICard"/> by default.</param>
        public EmptyDeck(string defaultAddCardPosition = "bottom", bool defaultShuffleOnAddCard = false)
            : base(defaultAddCardPosition, defaultShuffleOnAddCard)
        {
        }

        /// <inheritdoc/>
        protected override void CreateDeck()
        {
            Deck = new();
        }

    }
}
