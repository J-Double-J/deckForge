using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Flips a card on the table for a <see cref="IPlayer"/>.
    /// </summary>
    public class FlipOneCard_OneWayAction : PlayerGameAction
    {
        private readonly bool? facedown;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlipOneCard_OneWayAction"/> class.
        /// </summary>
        /// <param name="specificCardTablePos">The specific position of the card on the <see cref="GameElements.ITable"/>
        /// to flip.</param>
        /// <param name="facedown">Indicates whether the card should be flipped up, down, or regardless of current
        /// orientation.</param>
        /// <param name="name">Name of the <see cref="FlipOneCard_OneWayAction"/>.</param>
        /// <param name="description">Description of the <see cref="FlipOneCard_OneWayAction"/>.</param>
        public FlipOneCard_OneWayAction(
            int specificCardTablePos,
            bool? facedown = null,
            string name = "Flip Action",
            string description = "Flips Cards")
            : base(name: name, description: description)
        {
            SpecificCardTablePos = specificCardTablePos;
            this.facedown = facedown;
        }

        /// <summary>
        /// Gets or sets the specific <see cref="Card"/> position on the table to specify
        /// which <see cref="Card"/> to flip.
        /// </summary>
        public int SpecificCardTablePos { get; set; }

        // Returns what card was targetted for the flip

        /// <inheritdoc/>
        public override Card Execute(IPlayer player)
        {
            return player.FlipSingleCard(SpecificCardTablePos, facedown);
        }
    }
}