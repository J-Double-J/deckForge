using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Deals a certain number of <see cref="ICard"/>s to the <see cref="Table"/>.
    /// </summary>
    public class DealCardsFromTableDeckToTable : BaseGameAction
    {
        private readonly IGameMediator gm;

        /// <summary>
        /// Initializes a new instance of the <see cref="DealCardsFromTableDeckToTable"/> class.
        /// </summary>
        /// <param name="gm">The <see cref="IGameMediator"/> that the action will 
        /// communicate with.</param>
        /// <param name="numberOfCardsToDealToTable">Number of <see cref="ICard"/>s to draw.</param>
        /// <param name="zoneOfDeck"><see cref="TablePlacementZoneType"/> that the <see cref="IDeck"/> resides in.</param>
        /// <param name="zoneToDealCardsTo"><see cref="TablePlacementZoneType"/> to deal the <see cref="ICard"/>s
        /// drawn from the <see cref="IDeck"/> to.</param>
        /// <param name="areaOfDeck">Area of the <see cref="TableZone"/> the <see cref="IDeck"/> resides in.</param>
        /// <param name="areaToDealToInZone">Area of the <see cref="TableZone"/> to deal the <see cref="ICard"/>s to.</param>
        /// <param name="faceup">Whether to play the <see cref="ICard"/>s face up.</param>
        /// <param name="name">Name of the action.</param>
        /// <param name="description">Description of the action.</param>
        public DealCardsFromTableDeckToTable(
            IGameMediator gm,
            int numberOfCardsToDealToTable,
            TablePlacementZoneType zoneOfDeck,
            TablePlacementZoneType zoneToDealCardsTo,
            int areaOfDeck = 0,
            int areaToDealToInZone = 0,
            bool faceup = true,
            string name = "Deal Cards to Table",
            string description = "Deal a number of cards to the table")
            : base(name: name, description: description)
        {
            this.gm = gm;
            ZoneOfDeck = zoneOfDeck;
            AreaOfDeck = areaOfDeck;
            ZoneToDealCardsTo = zoneToDealCardsTo;
            AreaToDealToInZone = areaToDealToInZone;
            NumberOfCardsToDealToTable = numberOfCardsToDealToTable;
            PlayFaceUp = faceup;
        }

        /// <summary>
        /// Gets or sets the <see cref="TablePlacementZoneType"/> of where the target <see cref="IDeck"/> resides.
        /// </summary>
        public TablePlacementZoneType ZoneOfDeck { get; set; }

        /// <summary>
        /// Gets or sets the area of where the <see cref="IDeck"/> resides in <see cref="TablePlacementZoneType"/>.
        /// </summary>
        public int AreaOfDeck { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TablePlacementZoneType"/> of which zone to play the <see cref="ICard"/>s to. 
        /// </summary>
        public TablePlacementZoneType ZoneToDealCardsTo { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="TablePlacementZoneType"/> of which area to play to in <see cref="ZoneToDealCardsTo"/>.
        /// </summary>
        public int AreaToDealToInZone { get; set; }

        /// <summary>
        /// Gets or sets the number of <see cref="GameElements.Resources.PlayingCard"/>s to deal
        /// to each <see cref="PlayerConstruction.IPlayer"/>.
        /// </summary>
        public int NumberOfCardsToDealToTable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to play the <see cref="ICard"/>s faceup or not.
        /// </summary>
        public bool PlayFaceUp { get; set; }

        public override object? Execute()
        {
            List<ICard> cards = gm.Table!.DrawMultipleCardsFromDeck(NumberOfCardsToDealToTable, ZoneOfDeck, AreaOfDeck);

            foreach (ICard? card in cards)
            {
                if (card != null)
                {
                    card.Flip(!PlayFaceUp);
                }
                else
                {
                    break;
                }
            }

            gm.Table!.PlayCardsToZone(cards, ZoneToDealCardsTo, AreaToDealToInZone);
            return cards;
        }
    }
}
