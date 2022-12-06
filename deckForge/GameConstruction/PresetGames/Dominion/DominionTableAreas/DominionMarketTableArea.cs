using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;

namespace DeckForge.GameConstruction.PresetGames.Dominion.DominionTableAreas
{
    /// <summary>
    /// Standard settings for the Market in Dominion.
    /// </summary>
    public class DominionMarketTableArea : TableArea
    {
        private int numberOfEmptyDecks = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="DominionMarketTableArea"/> class.
        /// </summary>
        /// <param name="decks">The list of <see cref="IDeck"/>s that can be bought from in this <see cref="TableArea"/>.</param>
        public DominionMarketTableArea(List<IDeck> decks)
            : base(id: 0, TablePlacementZoneType.NeutralZone, decks, areaCardLimit: 0)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the game is over according to the Market.
        /// </summary>
        public bool IsGameOver { get; private set; } = false;

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

        /// <summary>
        /// Gets the cost of the <see cref="DominionCard"/>s in the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="deckNum">Specifies which <see cref="IDeck"/> to get the cost from.</param>
        /// <returns>The cost of the <see cref="ICard"/>.</returns>
        /// <exception cref="ArgumentException">Throws if there are no <see cref="ICard"/>s in the <see cref="IDeck"/>.</exception>
        public Dictionary<Type, int> GetCostOfCardsInDeck(int deckNum)
        {
            if (Decks[deckNum].Count == 0)
            {
                throw new ArgumentException("That deck is empty, cost is unknown", nameof(deckNum));
            }
            else
            {
                return ((DominionCard)Decks[deckNum].Deck[0]).Cost;
            }
        }

        /// <inheritdoc/>
        public override List<ICard?> DrawCardsFromDeck(int deckNum, int cardCount)
        {
            List<ICard?> cards = base.DrawCardsFromDeck(deckNum, cardCount);
            if (cards[0] is not null)
            {
                UpdateProgressTowardsGameEndState(cards[0]!, deckNum);
            }

            return cards;
        }

        private void UpdateProgressTowardsGameEndState(ICard card, int deckNum)
        {
            if (Decks[deckNum].Count == 0)
            {
                if (card.GetType() == typeof(ProvinceCard))
                {
                    IsGameOver = true;
                }
                else
                {
                    numberOfEmptyDecks++;
                    if (numberOfEmptyDecks == 3)
                    {
                        IsGameOver = true;
                    }
                }
            }
        }

        private string GetStringOfTopCardIfNotEmptyDeck(IDeck deck)
        {
            string cardRepresentation = string.Empty;

            if (deck.Deck.Count is not 0)
            {
                cardRepresentation = ((DominionCard)deck.Deck[0]).Name + $" ({((DominionCard)deck.Deck[0]).Cost[typeof(Coin)]} Coins)";
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
