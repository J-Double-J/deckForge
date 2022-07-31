using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Takes all the cards from a target <see cref="IPlayer"/>'s spot on the <see cref="GameElements.Table"/> and
    /// puts its in the <see cref="Deck"/> managed by the <see cref="IPlayer"/>.
    /// </summary>
    public class TakeAllCards_FromTargetPlayerTable_ToPlayerDeckAction : PlayerGameAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TakeAllCards_FromTargetPlayerTable_ToPlayerDeckAction"/> class.
        /// </summary>
        /// <param name="name">Name of the <see cref="TakeAllCards_FromTargetPlayerTable_ToPlayerDeckAction"/>.</param>
        /// <param name="description">Description of the <see cref="TakeAllCards_FromTargetPlayerTable_ToPlayerDeckAction"/>.</param>
        public TakeAllCards_FromTargetPlayerTable_ToPlayerDeckAction(
            string name = "Take Cards from Player on Table",
            string description = "Take all Cards on the Player Target's Table spot.")
            : base(name: name, description: description)
        {
        }

        /// <inheritdoc/>
        public override List<Card> Execute(IPlayer playerExecutor, IPlayer playerTarget)
        {
            try
            {
                int resourceCollectionID = playerExecutor.FindCorrectResourceCollectionID(typeof(Card));
                List<Card> cards = playerTarget.TakeAllCardsFromTable();
                List<object> objectCards = cards.Cast<object>().ToList();

                playerExecutor.AddMultipleResourcesToCollection(resourceCollectionID, objectCards);

                return cards;
            }
            catch
            {
                throw;
            }
        }
    }
}
