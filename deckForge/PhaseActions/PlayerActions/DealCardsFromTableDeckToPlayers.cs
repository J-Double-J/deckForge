using DeckForge.GameConstruction;
using DeckForge.GameElements.Table;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Deals a certain number of <see cref="PlayingCard"/>s to each <see cref="IPlayer"/>.
    /// </summary>
    public class DealCardsFromTableDeckToPlayers : BaseGameAction
    {
        private readonly IGameMediator gm;

        public DealCardsFromTableDeckToPlayers(
            IGameMediator gm,
            int numberOfCardsToDealToEachPlayer,
            TablePlacementZoneType zoneType,
            int area = 0,
            string name = "Deal Cards",
            string description = "Deal a number of cards to each player")
            : base(name: name, description: description)
        {
            this.gm = gm;
            NumberOfCardsToDealToEachPlayer = numberOfCardsToDealToEachPlayer;
            ZoneType = zoneType;
            Area = area;
        }

        /// <summary>
        /// Gets or sets the number of <see cref="GameElements.Resources.PlayingCard"/>s to deal
        /// to each <see cref="PlayerConstruction.IPlayer"/>.
        /// </summary>
        public int NumberOfCardsToDealToEachPlayer { get; set; }

        /// <summary>
        /// Gets or sets the type of zone to search for <see cref="IDeck"/> on the <see cref="Table"/>.
        /// </summary>
        public TablePlacementZoneType ZoneType { get; set; }

        /// <summary>
        /// Gets or sets the optional parameter of which area to search for a <see cref="IDeck"/> in the zone.
        /// </summary>
        public int Area { get; set; }

        public override object? Execute()
        {
            gm.DealCardsFromDeckToAllPlayers(NumberOfCardsToDealToEachPlayer, ZoneType, Area);
            return null;
        }
    }
}
