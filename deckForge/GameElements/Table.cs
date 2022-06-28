using deckForge.GameConstruction;
using CardNamespace;

namespace deckForge.GameElements
{
    public class Table
    {
        GameMediator _gm;
        List<List<Card>> playedCards;
        public Table(GameMediator mediator, int playerCount)
        {
            _gm = mediator;

            playedCards = new();
            for (var i = 0; i < playerCount; i++)
            {
                List<Card> cards = new();
                playedCards.Add(cards);
            }
        }

        public void PrintTableState()
        {
            foreach (List<Card> player in playedCards)
            {
                foreach (Card c in player)
                {
                    Console.WriteLine(c.PrintCard());
                }
            }
        }

        public void PlaceCardOnTable(int playerID, Card c) {
            if (playerID >= 0 && playerID <= playedCards.Count - 1)
                playedCards[playerID].Add(c);
            else
                throw new ArgumentException("Cannot place a card to a non-existant player");
        }
    }
}
