using DeckForge.PlayerConstruction;
using DeckForge.GameElements.Resources;

namespace DeckForge.PhaseActions
{
    public class TakeAllCards_FromTargetPlayerTable_ToPlayerDeck : PlayerGameAction
    {
        public TakeAllCards_FromTargetPlayerTable_ToPlayerDeck(
            string name = "Take Cards from Player on Table",
            string description = "Take all Cards on the Player Target's Table spot."
            ) : base(name: name, description: description)
        { }

        public override List<Card> Execute(IPlayer playerExecutor, IPlayer playerTarget)
        {
            try {
                int resourceCollectionID = playerExecutor.FindCorrectResourceCollectionID(typeof(Card));
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
