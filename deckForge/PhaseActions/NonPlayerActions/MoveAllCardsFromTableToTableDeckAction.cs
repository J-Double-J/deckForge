using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions.NonPlayerActions;

namespace DeckForge.PhaseActions.NonPlayerActions
{
    public class MoveAllCardsFromTableToTableDeckAction : BaseGameAction
    {
        private IGameMediator gm;
        private int targetDeck;
        private bool shuffleAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveAllCardsFromTableToTableDeck"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with the <see cref="ITable"/>.</param>
        /// <param name="deckPos">Position of the <see cref="IDeck"/> on the table.</param>
        /// <param name="shuffleAfter">Shuffles the <see cref="IDeck"/> after adding the cards.</param>
        /// <param name="name">Name of the action.</param>
        /// <param name="description">Description of the action.</param>
        public MoveAllCardsFromTableToTableDeckAction(
            IGameMediator gm,
            int deckPos,
            bool shuffleAfter = true,
            string name = "Move All Cards From Table to Table Deck",
            string description = "Moves cards from table and readds them to deck.")
            : base(name, description)
        {
            this.gm = gm;
            targetDeck = deckPos;
            this.shuffleAfter = shuffleAfter;
        }

        /// <inheritdoc/>
        public override object? Execute()
        {
            if (gm.Table is not null)
            {
                gm.Table.TableDecks[targetDeck].AddMultipleCardsToDeck(
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
