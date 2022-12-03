using DeckForge.GameElements.Table;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Table
{
    /// <summary>
    /// Settings for any <see cref="TableArea"/> for players in <see cref="Dominion"/>.
    /// </summary>
    public class DominionPlayerTableArea : TableArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionPlayerTableArea"/> class.
        /// </summary>
        /// <param name="id">ID of the <see cref="TableArea"/>.</param>
        public DominionPlayerTableArea(int id)
            : base(id, TablePlacementZoneType.PlayerZone, discardPile: true)
        {
        }
    }
}
