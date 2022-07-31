using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Lets the <see cref="IPlayer"/> play a <see cref="Card"/>.
    /// </summary>
    public class PlayCardAction : PlayerGameAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayCardAction"/> class.
        /// </summary>
        /// <param name="name">Name of the <see cref="PlayCardAction"/>.</param>
        public PlayCardAction(string name = "Play Card")
        : base()
        {
            Name = name;
            Description = $"Play a Card";
        }

        /// <inheritdoc/>
        public override Card? Execute(IPlayer player)
        {
            return player.PlayCard();
        }
    }
}
