using PlayerNamespace;
using CardNamespace;

namespace GameNamespace
{
    public class GameMediator
    {
        Game game;
        List<Player> players;
        Score score;

        public GameMediator(int playerCount) {
            game = new Game(playerCount);
            score = new Score(playerCount);

            players = new List<Player>();
            for (var i = 0; i < playerCount; i++)
                players.Add(new Player(this));
        }

        public void StartGame() {
            StartPlayerTurn(game.GetCurrentPlayer());
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
            if (game != null)
            {
                Card? c = game.DrawCard();
                if (c != null)
                {
                    return c;
                }
                else
                {
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
}
