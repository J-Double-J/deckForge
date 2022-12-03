using DeckForge.GameElements.Table;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Table
{
    /// <summary>
    /// Standard settings for the Market in Dominion.
    /// </summary>
    public class DominionMarketTableArea : TableArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionMarketTableArea"/> class.
        /// </summary>
        public DominionMarketTableArea()
            : base(id: 0, TablePlacementZoneType.NeutralZone, areaCardLimit: 0)
        {
        }
    }
}
