using DeckNameSpace;
using CardNamespace;
using PlayerNamespace;

namespace GameNamespace
{
    public class Game
    {
        Deck deck;
        int playerCount;
        TurnHandler turnOrder;

        public Game(int playerCount, bool turnRandomizer = false)
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

        public int PlayerTurnXTurnsFromNow(int turns = 1) {
            return turnOrder.GetWhoseTurnXTurnsFromNow(turns);
        }

        public Card? DrawCard()
        {
            Card? c = deck.DrawCard();
            if (c != null) {
                return c;
            } else {
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

            public int GetWhoseTurnXTurnsFromNow(int turns) {
                return (turnNum + turns) % order.Count;
            }
        }
    }

    public class Score
    {
        int rows;
        int[,] scores;

        public Score(int playerCount)
        {
            scores = new int[playerCount, 1];
            rows = playerCount;
            for (var i = 0; i < rows; i++)
            {
                scores.SetValue(0, i, 0);
            }
        }
        public int GetPlayerScore(int playerNum)
        {
            //TODO: Error Handling
            return scores[playerNum, 0];
        }
        public void IncreasePlayerScore(int playerNum, int score)
        {
            scores[playerNum, 0] += score;
        }
    }
}
