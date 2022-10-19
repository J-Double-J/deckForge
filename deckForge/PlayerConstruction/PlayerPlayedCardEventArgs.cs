using DeckForge.GameElements.Resources;

namespace DeckForge.PlayerConstruction.PlayerEvents
{
    /// <summary>
    /// <see cref="EventArgs"/> for when a <see cref="IPlayer"/> plays a <see cref="ICard"/>.
    /// </summary>
    public class PlayerPlayedCardEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPlayedCardEventArgs"/> class.
        /// </summary>
        /// <param name="card">The <see cref="ICard"/> that is played when this event is raised.</param>
        public PlayerPlayedCardEventArgs(ICard card)
        {
            CardPlayed = card;
        }

        /// <summary>
        /// Gets the <see cref="ICard"/> played by a <see cref="IPlayer"/>.
        /// </summary>
        public ICard CardPlayed { get; }
    }
}
