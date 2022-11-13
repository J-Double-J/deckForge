using DeckForge.GameConstruction;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;

namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// A <see cref="ICard"/> that causes the <see cref="IPlayer"/> to draw some cards.
    /// </summary>
    public class DrawCardEffectCard : InstantEffectCard
    {
        private DrawCardsAction cardEffect;
        private int drawCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawCardEffectCard"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="zoneType">Type of <see cref="TableZone"/> that owns the <see cref="IDeck"/> to draw from.</param>
        /// <param name="area">An optional parameter specifying which area in the <see cref="TableZone"/> the <see cref="IDeck"/> resides.</param>
        /// <param name="drawCount">Number of <see cref="ICard"/>s to draw.</param>
        /// <param name="facedown">Default orientation of the card.</param>
        public DrawCardEffectCard(IGameMediator gm, TablePlacementZoneType zoneType, int area = 0, int drawCount = 1, bool facedown = true)
            : base(gm, facedown, description: "On play, draws one card.")
        {
            cardEffect = new DrawCardsAction(zoneType, area);
            DrawCount = drawCount;
            ZoneType = zoneType;
            Area = area;

        }

        /// <summary>
        /// Gets or sets the number of <see cref="ICard"/>s to draw when played.
        /// </summary>
        public int DrawCount
        {
            get
            {
                return drawCount;
            }

            set
            {
                drawCount = value;
                cardEffect.DrawCount = drawCount;
            }
        }

        /// <summary>
        /// Gets or sets the type of <see cref="TableZone"/> that owns the <see cref="IDeck"/> to draw from.
        /// </summary>
        public TablePlacementZoneType ZoneType { get; set; }

        /// <summary>
        /// Gets or sets the optional parameter specifying which area in the <see cref="TableZone"/> the <see cref="IDeck"/> resides.
        /// </summary>
        public int Area { get; set; }

        /// <inheritdoc/>
        public override void ExecuteEffect()
        {
            if (OwnedBy is not null)
            {
                cardEffect.Execute(OwnedBy);
            }
        }
    }
}
