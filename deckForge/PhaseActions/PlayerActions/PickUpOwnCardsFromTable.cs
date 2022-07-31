using DeckForge.PlayerConstruction;
using DeckForge.GameElements.Resources;

namespace DeckForge.PhaseActions
{
    public class PickUpOwnCardsFromTable : PlayerGameAction
    {
        public PickUpOwnCardsFromTable(
            string name = "Pick up all cards owned by Player",
            string description = "Pick up all Cards on the Players Table spot."
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
