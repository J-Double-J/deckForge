using deckForge.GameConstruction;
using CardNamespace;

namespace deckForge.GameElements
{
    public class Table
    {
        GameMediator _gm;
        List<List<Card>> playedCards;
        Table(GameMediator mediator)
        {
            _gm = mediator;

            playedCards = new();
            for (var i = 0; i < _gm.PlayerCount; i++)
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
                    c.PrintCard();
                }
            }
        }
    }
}
