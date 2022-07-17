using deckForge.PlayerConstruction;
using deckForge.GameElements.Resources;

namespace deckForge.PhaseActions
{
    public class TakeAllCards_FromTargetPlayerTable_ToPlayerDeck : PlayerGameAction
    {
        public TakeAllCards_FromTargetPlayerTable_ToPlayerDeck(
            string name = "Take Cards from Player on Table",
            string description = "Take all Cards on the Player Target's Table spot."
            ) : base(name: name, description: description)
        { }

        public override List<Card> execute(IPlayer playerExecutor, IPlayer playerTarget)
        {
            try {
                int resourceCollectionID = playerExecutor.FindCorrectPoolID(typeof(Card));
                List<Card> cards = playerTarget.TakeAllCardsFromTable();
                List<object> objectCards = cards.Cast<object>().ToList();

                playerExecutor.AddMultipleResourcesToCollection(resourceCollectionID, objectCards);

                return cards;
            } catch {
                throw;
            }
        }
    }
}
