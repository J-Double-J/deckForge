using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions.NonPlayerActions;

namespace DeckForge.PhaseActions.NonPlayerActions
{
    public class MoveAllCardsFromTableToTableDeckAction : BaseGameAction
    {
        private IGameMediator gm;
        private int areaThatOwnsDeck;
        private bool shuffleAfter;
        private TablePlacementZoneType zoneThatOwnsDeck;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveAllCardsFromTableToTableDeck"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with the <see cref="ITable"/>.</param>
        /// <param name="zoneThatOwnsDeck"><see cref="TablePlacementZoneType"/> that owns the target <see cref="IDeck"/>.</param>
        /// <param name="areaThatOwnsDeck">Position of the <see cref="IDeck"/> on the table.</param>
        /// <param name="shuffleAfter">Shuffles the <see cref="IDeck"/> after adding the cards.</param>
        /// <param name="name">Name of the action.</param>
        /// <param name="description">Description of the action.</param>
        public MoveAllCardsFromTableToTableDeckAction(
            IGameMediator gm,
            TablePlacementZoneType zoneThatOwnsDeck,
            int areaThatOwnsDeck = 0,
            bool shuffleAfter = true,
            string name = "Move All Cards From Table to Table Deck",
            string description = "Moves cards from table and readds them to deck.")
            : base(name, description)
        {
            this.gm = gm;
            this.areaThatOwnsDeck = areaThatOwnsDeck;
            this.shuffleAfter = shuffleAfter;
            this.zoneThatOwnsDeck = zoneThatOwnsDeck;
        }

        /// <inheritdoc/>
        public override object? Execute()
        {
            if (gm.Table is not null)
            {
                gm.Table.GetDeckFromAreaInZone(zoneThatOwnsDeck, areaThatOwnsDeck)!.AddMultipleCardsToDeck(
                    gm.Table.Remove_AllCardsFromTable().ToList(),
                    shuffleAfter: shuffleAfter);
            }
            else
            {
                throw new ArgumentNullException("GameMediator does not have a Table registered with it.");
            }

            return null;
        }
    }
}
