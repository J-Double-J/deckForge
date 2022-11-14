using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Flips a card on the table for a <see cref="IPlayer"/>.
    /// </summary>
    public class FlipOneCard_OneWayAction : PlayerGameAction
    {
        private readonly bool? facedown;
        private readonly IGameMediator gm;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlipOneCard_OneWayAction"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to interact with other game elements.</param>
        /// <param name="specificCardTablePos">The specific position of the card on the <see cref="Table.ITable"/>
        /// to flip.</param>
        /// <param name="facedown">Indicates whether the card should be flipped up, down, or regardless of current
        /// orientation.</param>
        /// <param name="name">Name of the <see cref="FlipOneCard_OneWayAction"/>.</param>
        /// <param name="description">Description of the <see cref="FlipOneCard_OneWayAction"/>.</param>
        public FlipOneCard_OneWayAction(
            IGameMediator gm,
            int specificCardTablePos,
            bool? facedown = null,
            string name = "Flip Action",
            string description = "Flips Cards")
            : base(name: name, description: description)
        {
            this.gm = gm;
            SpecificCardTablePos = specificCardTablePos;
            this.facedown = facedown;
        }

        /// <summary>
        /// Gets or sets the specific <see cref="ICard"/> position on the table to specify
        /// which <see cref="ICard"/> to flip.
        /// </summary>
        public int SpecificCardTablePos { get; set; }

        // Returns what card was targetted for the flip

        /// <inheritdoc/>
        public override ICard Execute(IPlayer player)
        {
            player.FlipSingleCard(SpecificCardTablePos, facedown);
            return gm.Table!.PlayerZones[player.PlayerID][SpecificCardTablePos];
        }
    }
}