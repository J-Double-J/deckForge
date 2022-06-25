using DeckNameSpace;
using CardNamespace;
using PlayerNamespace;

namespace GameNamespace
{

    internal interface IAbstractGameMediator
    {
        public void EndPlayerTurn();
        public void EndGame();
        public void StartPlayerTurn(int turn);
    }

    public class GameMediator : IAbstractGameMediator
    {
        Game? game;
        List<Player>? players;
        Score? score;

        public Game Game
        {
            set { game = value; }
        }

        public List<Player> Players
        {
            set { players = value; }
        }

        public Score Score
        {
            set { score = value; }
        }

        public void StartPlayerTurn(int turn)
        {
            if (players != null)
            {
                players[turn].StartTurn();
            }
            else
            {
                Console.WriteLine("Start Player Turn - Improper Instantiation of GameMediator: No Players List Object Set");
                System.Environment.Exit(1);
            }
        }

        public void PlayerPlayedCard(Card c)
        {
            if (game != null && score != null)
                score.IncreasePlayerScore(game.GetCurrentPlayer(), c.val);
            else
            {
                Console.WriteLine("Player Played Card - Improper Instantiation of GameMediator: No Game Object Set");
                System.Environment.Exit(1);
            }
        }

        public void EndPlayerTurn()
        {
            if (game != null)
            {
                StartPlayerTurn(game.NextPlayerTurn());
            }
            else
            {
                Console.WriteLine("End Player Turn - Improper Instantiation of GameMediator: No Game Object Set");
                System.Environment.Exit(1);
            }
        }

        public void EndGame()
        {
            if (game != null)
            {
                game.EndGame();
            }
            else
            {
                Console.WriteLine("End Game - Improper Instantiation of GameMediator: No Game Object Set");
                System.Environment.Exit(1);
            }
        }

        public Card? DrawCardFromDeck()
        {
            if (game != null) {                                
                Card? c = game.DrawCard();
                if (c != null) {
                    return c;
                } else {
                    return null;
                }
            }
            else
            {
                Console.WriteLine("DrawCardFromDeck - Improper Instantiation of GameMediator: No Game Object Set");
                System.Environment.Exit(1);
                return null;
            }
        }

    }

    public class CardGameInitializer
    {
        GameMediator gm = new GameMediator();
        Game game;
        List<Player> players;

        public CardGameInitializer(int playerCount)
        {
            game = new Game(gm, playerCount);
            gm.Game = game;

            players = new List<Player>();
            for (var i = 0; i < playerCount; i++)
            {
                players.Add(new Player(gm));
            }
            gm.Players = players;

            gm.StartPlayerTurn(game.GetCurrentPlayer());
        }
    }

    public class Game
    {
        Deck deck;
        Score score;
        GameMediator gm;
        int playerCount;
        TurnHandler turnOrder;

        public Game(GameMediator gm, int playerCount, bool turnRandomizer = false)
        {
            this.gm = gm;
            this.playerCount = playerCount;
            deck = new Deck();
            deck.Shuffle();
            score = new Score(playerCount);
            turnOrder = new TurnHandler(playerCount, turnRandomizer);

            gm.Score = score;
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
