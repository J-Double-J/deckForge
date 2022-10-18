namespace DeckForge.GameElements.Resources.Cards.CardEvents
{
    /// <summary>
    /// Event that is raised for whenever a card is played from a <see cref="IPlayer"/>.
    /// </summary>
    public class PlayerPlayedCardEvent : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPlayedCardEvent"/> class.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> that is played.</param>
        public PlayerPlayedCardEvent(ICard card)
        {
            Card = card;
        }

        /// <summary>
        /// Gets the <see cref="ICard"/> played.
        /// </summary>
        public ICard Card { get; }
    }
}
