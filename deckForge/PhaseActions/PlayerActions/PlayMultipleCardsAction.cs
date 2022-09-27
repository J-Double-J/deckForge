using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Lets the <see cref="IPlayer"/> to play a number of <see cref="ICard"/>s.
    /// </summary>
    public class PlayMultipleCardsAction : PlayerGameAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayMultipleCardsAction"/> class.
        /// </summary>
        /// <param name="playCount">Number of <see cref="ICard"/>s that the action lets
        /// <see cref="IPlayer"/> play.</param>
        /// <param name="name">Name of the <see cref="PlayMultipleCardsAction"/>.</param>
        public PlayMultipleCardsAction(int playCount, string name = "Play")
        : base()
        {
            if (playCount < 0)
            {
                throw new ArgumentException("Play count must be 0 or greater");
            }

            Name = name;
            PlayCount = playCount;
            Description = $"Play {playCount} Cards";
        }

        /// <summary>
        /// Gets or sets the number of <see cref="ICard"/>s the action lets
        /// the <see cref="IPlayer"/> play.
        /// </summary>
        public int PlayCount { get; set; }

        /// <inheritdoc/>
        public override List<ICard?> Execute(IPlayer player)
        {
            List<ICard?> playedCards = new();

            for (var i = 0; i < PlayCount; i++)
            {
                playedCards.Add(player.PlayCard());
            }

            return playedCards;
        }
    }
}
