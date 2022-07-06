using DeckNameSpace;
using CardNamespace;
using deckForge.PlayerConstruction;

namespace deckForge.GameConstruction
{
    public class BaseGameController
    {
        Deck deck;
        int playerCount;
        TurnHandler turnOrder;

        public BaseGameController(int playerCount, bool turnRandomizer = false)
        {
            PlayerCount = playerCount;
            deck = new Deck();
            turnOrder = new TurnHandler(playerCount, turnRandomizer);
        }

        public int PlayerCount
        {
            get { return playerCount; }
            private set { playerCount = value; }
        }

        public int NextPlayerTurn()
        {
            //TODO: Inter-Round Rules Abstraction (Game win? Shuffle Pieces? etc)
            turnOrder.incrementTurnOrder();
            return turnOrder.GetWhoseTurn();
        }

        public int PlayerTurnXTurnsFromNow(int turns = 1)
        {
            return turnOrder.GetWhoseTurnXTurnsFromNow(turns);
        }

        public Card? DrawCard()
        {
            Card? c = deck.DrawCard();
            if (c != null)
            {
                return c;
            }
            else
            {
                return null;
            }
        }

        public int GetCurrentPlayer()
        {
            return turnOrder.GetWhoseTurn();
        }

        public void EndGame()
        {
            Console.WriteLine("You have emptied your hand. Congrats! No logical flaws were found.");
            Environment.Exit(0);
        }

        private class TurnHandler
        {
            private Random rng = new Random();
            private List<int> order = new List<int>();
            int turnNum;
            public int TurnNum
            {
                get { return turnNum; }
                private set { turnNum = value; }
            }

            public List<int> TurnOrder
            {
                get { return order; }
                private set { order = value; }
            }
            public TurnHandler(int playerCount, bool turnRandomizer)
            {

                for (var i = 0; i < playerCount; i++)
                {
                    order.Add(i);
                }
                if (turnRandomizer)
                {
                    var n = playerCount;
                    while (n > 1)
                    {
                        n--;
                        int k = rng.Next(n + 1);
                        int value = order[k];
                        order[k] = order[n];
                        order[n] = value;
                    }
                }
            }

            public void incrementTurnOrder()
            {
                turnNum++;
            }

            public int GetWhoseTurn()
            {
                return turnNum % order.Count;
            }

            public int GetWhoseTurnXTurnsFromNow(int turns)
            {
                return (turnNum + turns) % order.Count;
            }
        }
    }
}
