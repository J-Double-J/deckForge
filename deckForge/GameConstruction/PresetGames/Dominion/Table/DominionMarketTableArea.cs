using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Table
{
    /// <summary>
    /// Standard settings for the Market in Dominion.
    /// </summary>
    public class DominionMarketTableArea : TableArea
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionMarketTableArea"/> class.
        /// </summary>
        /// <param name="decks">The list of <see cref="IDeck"/>s that can be bought from in this <see cref="TableArea"/>.</param>
        public DominionMarketTableArea(List<IDeck> decks)
            : base(id: 0, TablePlacementZoneType.NeutralZone, decks, areaCardLimit: 0)
        {
        }

        public List<string> GetMarketAreaAsStringList()
        {
            List<string> market = new();

            for (int i = 0; i < Decks.Count; i++)
            {
                string topCard = GetStringOfTopCardIfNotEmptyDeck(Decks[i]);
                string deckString = WriteStringForDeck(Decks[i], topCard);
                AddToMarketListIfDeckStringNotEmpty(market, deckString);
            }

            return market;
        }

        private string GetStringOfTopCardIfNotEmptyDeck(IDeck deck)
        {
            string cardRepresentation = string.Empty;

            if (deck.Deck.Count is not 0)
            {
                cardRepresentation = ((DominionCard)deck.Deck[0]).Name;
            }

            return cardRepresentation;
        }

        private string WriteStringForDeck(IDeck deck, string cardString)
        {
            string output = string.Empty;
            if (cardString is not "")
            {
                output = cardString + $" - [{deck.Count} Remaining]";
            }

            return output;
        }

        private void AddToMarketListIfDeckStringNotEmpty(List<string> market, string deckString)
        {
            if (deckString is not "")
            {
                market.Add(deckString);
            }
        }
    }
}
