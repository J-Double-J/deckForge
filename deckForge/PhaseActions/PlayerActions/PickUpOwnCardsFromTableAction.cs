using DeckForge.GameElements.Resources;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Picks up all the <see cref="Card"/>s from the <see cref="GameElements.ITable"/> that the <see cref="IPlayer"/> owns.
    /// </summary>
    public class PickUpOwnCardsFromTableAction : PlayerGameAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PickUpOwnCardsFromTableAction"/> class.
        /// </summary>
        /// <param name="name">The name of <see cref="PickUpOwnCardsFromTableAction"/>.</param>
        /// <param name="description">The description of <see cref="PickUpOwnCardsFromTableAction"/>.</param>
        public PickUpOwnCardsFromTableAction(
            string name = "Pick up all cards owned by Player",
            string description = "Pick up all Cards on the Players Table spot.")
            : base(name: name, description: description)
        {
        }

        /// <inheritdoc/>
        public override List<Card> Execute(IPlayer player)
        {
            try
            {
                int resourceCollectionID = player.FindCorrectResourceCollectionID(typeof(Card));
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
