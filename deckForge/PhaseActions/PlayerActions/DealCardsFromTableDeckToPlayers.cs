using DeckForge.GameConstruction;

namespace DeckForge.PhaseActions.PlayerActions
{
    /// <summary>
    /// Deals a certain number of <see cref="PlayingCard"/>s to each <see cref="IPlayer"/>.
    /// </summary>
    public class DealCardsFromTableDeckToPlayers : BaseGameAction
    {
        private readonly IGameMediator gm;

        public DealCardsFromTableDeckToPlayers(
            IGameMediator gm,
            int deckPos,
            int numberOfCardsToDealToEachPlayer,
            string name,
            string description)
            : base(name: name, description: description)
        {
            this.gm = gm;
            DeckPos = deckPos;
            NumberOfCardsToDealToEachPlayer = numberOfCardsToDealToEachPlayer;
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
        public int NumberOfCardsToDealToEachPlayer { get; set; }

        public override object? Execute()
        {
            gm.DealCardsFromDeckToAllPlayers(DeckPos, NumberOfCardsToDealToEachPlayer);
            return null;
        }
    }
}
