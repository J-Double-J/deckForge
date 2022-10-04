using DeckForge.GameConstruction;

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
        /// <param name="deckPos">Position of the <see cref="IDeck"/>
        /// on the <see cref="Table"/> to draw <see cref="ICard"/>s from.</param>
        /// <param name="numberOfCardsToDealToTable">Number of <see cref="ICard"/>s to draw.</param>
        /// <param name="tableNeutralZone">Neutral zone on the <see cref="Table"/> to
        /// play the <see cref="ICard"/>s to.</param>
        /// <param name="faceup">Whether to play the <see cref="ICard"/>s face up.</param>
        /// <param name="name">Name of the action.</param>
        /// <param name="description">Description of the action.</param>
        public DealCardsFromTableDeckToTable(
            IGameMediator gm,
            int deckPos,
            int numberOfCardsToDealToTable,
            int tableNeutralZone = 0,
            bool faceup = true,
            string name = "Deal Cards to Table",
            string description = "Deal a number of cards to the table")
            : base(name: name, description: description)
        {
            this.gm = gm;
            DeckPos = deckPos;
            NumberOfCardsToDealToTable = numberOfCardsToDealToTable;
            TableNeutralZone = tableNeutralZone;
            PlayFaceUp = faceup;
        }

        /// <summary>
        /// Gets or sets the position or index of the <see cref="GameElements.Resources.IDeck"/> on the <see cref="GameElements.Table"/>
        /// that the <see cref="IGameAction"/> will interact with.
        /// </summary>
        public int DeckPos { get; set; }

        /// <summary>
        /// Gets or sets the number of <see cref="GameElements.Resources.PlayingCard"/>s to deal
        /// to each <see cref="PlayerConstruction.IPlayer"/>.
        /// </summary>
        public int NumberOfCardsToDealToTable { get; set; }

        public int TableNeutralZone { get; set; }

        public bool PlayFaceUp { get; set; }

        public override object? Execute()
        {
            return gm.Table!.PlayCards_FromTableDeck_ToNeutralZone(
                NumberOfCardsToDealToTable, DeckPos, TableNeutralZone, PlayFaceUp);
        }
    }
}
