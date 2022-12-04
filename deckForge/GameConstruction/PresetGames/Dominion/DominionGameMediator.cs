using DeckForge.GameConstruction.PresetGames.Dominion.Table;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;

namespace DeckForge.GameConstruction.PresetGames.Dominion
{
    /// <summary>
    /// GameMediator for Dominion.
    /// </summary>
    public class DominionGameMediator : BaseGameMediator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionGameMediator"/> class.
        /// </summary>
        /// <param name="playerCount">Number of <see cref="DominionPlayer"/>s in game.</param>
        public DominionGameMediator(int playerCount)
            : base(playerCount)
        {
        }

        /// <summary>
        /// Gets the Dominion game's market area.
        /// </summary>
        public DominionMarketTableArea Market
        {
            get
            {
                return (DominionMarketTableArea)Table!.FindZoneBasedOnType(TablePlacementZoneType.NeutralZone, true)!.Areas[0];
            }
        }

        /// <summary>
        /// Grabs a <see cref="ICard"/> from the Market.
        /// </summary>
        /// <param name="deckNum">Deck from Market to target.</param>
        /// <param name="purchased">Whether this action is due to a purchase or not.</param>
        /// <returns>Drawn <see cref="ICard"/> from <see cref="IDeck"/>.</returns>
        /// <exception cref="ArgumentException">Throws if <see cref="IDeck"/> is empty.</exception>
        public ICard GrabCardFromMarketPlace(int deckNum, bool purchased = true)
        {
            ICard? card = Market.Decks[deckNum].DrawCard();
            if (card is null)
            {
                throw new ArgumentException("Cannot grab card from empty deck", nameof(deckNum));
            }

            if (purchased)
            {
                // TODO: Event?
            }

            return card!;
        }
    }
}
