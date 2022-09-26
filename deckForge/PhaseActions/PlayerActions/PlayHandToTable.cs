using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions.PlayerActions
{
    /// <summary>
    /// Plays a <see cref="IPlayer"/>'s hand to the <see cref="Table"/>.
    /// </summary>
    public class PlayHandToTable : PlayerGameAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayHandToTable"/> class.
        /// </summary>
        public PlayHandToTable()
            : base("Play Hand to Table", "Plays Entire Hand to Table")
        {
        }

        /// <inheritdoc/>
        public override List<PlayingCard>? Execute(IPlayer player)
        {
            List<PlayingCard> cardsPlayed = new();
            for (int i = 0; i < player.HandSize; i++)
            {
                cardsPlayed.Add(player.PlayCard()!);
            }

            return cardsPlayed;
        }
    }
}
