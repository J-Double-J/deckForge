using deckForge.PlayerConstruction;
using deckForge.GameElements.Resources;

namespace deckForge.PhaseActions
{
    public class PickUpOwnCardsFromTable : PlayerGameAction
    {
        public PickUpOwnCardsFromTable(
            string name = "Take Cards from Player on Table",
            string description = "Take all Cards on the Player Target's Table spot."
            ) : base(name: name, description: description)
        { }

        public override List<Card> execute(IPlayer player)
        {
            try
            {
                int resourceCollectionID = player.FindCorrectPoolID(typeof(Card));
                List<Card> cards = player.TakeAllCardsFromTable();
                List<object> objectCards = cards.Cast<object>().ToList();

                player.AddMultipleResourcesToCollection(resourceCollectionID, objectCards);

                return cards;
            }
            catch
            {
                throw;
            }
        }
    }
}
