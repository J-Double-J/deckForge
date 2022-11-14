using DeckForge.GameConstruction;
using DeckForge.GameElements.Table;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    public class ShuffleDeckOnTable : BaseGameAction
    {
        private IGameMediator gm;

        public ShuffleDeckOnTable(
            IGameMediator gm,
            TablePlacementZoneType zoneThatOwnsDeck,
            int areaThatOwnsDeck = 0,
            string name = "Shuffle deck on Table.",
            string description = "Takes a deck on the table and shuffles it.")
        : base(name: name, description: description)
        {
            this.gm = gm;
            AreaThatOwnsDeck = areaThatOwnsDeck;
            ZoneThatOwnsDeck = zoneThatOwnsDeck;
        }

        /// <summary>
        /// Gets or sets the area that owns the <see cref="IDeck"/> in the <see cref="TableZone"/>.
        /// </summary>
        public int AreaThatOwnsDeck { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TablePlacementZoneType"/> of the <see cref="TableZone"/> that owns the target <see cref="IDeck"/>.
        /// </summary>
        public TablePlacementZoneType ZoneThatOwnsDeck { get; set; }

        public override object? Execute()
        {
            gm.Table?.GetDeckFromAreaInZone(ZoneThatOwnsDeck, AreaThatOwnsDeck);
            return null;
        }
    }
}