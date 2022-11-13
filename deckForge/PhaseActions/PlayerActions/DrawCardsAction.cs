using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Draws a number of cards.
    /// </summary>
    public class DrawCardsAction : PlayerGameAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DrawCardsAction"/> class.
        /// </summary>
        /// <param name="drawCount">Number of <see cref="ICard"/>s to draw.</param>
        /// <param name="name">Name of the <see cref="DrawCardsAction"/>.</param>
        /// <param name="zoneType">Type of <see cref="TableZone"/> that owns the <see cref="IDeck"/> to draw from.</param>
        /// <param name="area">An optional parameter specifying which area in the <see cref="TableZone"/> the <see cref="IDeck"/> resides.</param>
        public DrawCardsAction(TablePlacementZoneType zoneType, int area = 0, int drawCount = 1, string name = "Draw")
        : base(name: name)
        {
            Name = name;
            DrawCount = drawCount;
            Description = $"Draw {drawCount} Card(s)";
        }

        /// <summary>
        /// Gets or sets the number of cards to be drawn.
        /// </summary>
        public int DrawCount { get; set; }

        /// <summary>
        /// Gets or sets the type of <see cref="TableZone"/> that owns the <see cref="IDeck"/> to draw from.
        /// </summary>
        public TablePlacementZoneType ZoneType { get; set; }

        /// <summary>
        /// Gets or sets the optional parameter specifying which area in the <see cref="TableZone"/> the <see cref="IDeck"/> resides.
        /// </summary>
        public int Area { get; set; }

        /// <inheritdoc/>
        /// <returns>The list of cards that was drawn into the player's hand.</returns>
        public override List<ICard?> Execute(IPlayer player)
        {
            List<ICard?> cards = new();
            for (int i = 0; i < DrawCount; i++)
            {
                cards.Add(player.DrawCard(ZoneType, Area));
            }

            return cards;
        }
    }
}
