using PlayerNamespace;
using CardNamespace;

namespace deckForge.GameConstruction
{
    public class GameMediator
    {
        Game game;
        List<Player> players;
        Score score;

        public GameMediator(int playerCount, string? presetGame = null)
        {
            try
            {
                game = new Game(playerCount);
                score = new Score(playerCount);

                players = new List<Player>();
                for (var i = 0; i < playerCount; i++)
                    players.Add(new Player(this));
            }
            catch
            {
                throw;
            }

        }

        public void StartGame()
        {
            StartPlayerTurn(game.GetCurrentPlayer());
        }
        public void StartPlayerTurn(int turn)
        {
            players[turn].StartTurn();
        }

        public void PlayerPlayedCard(Card c)
        {
            score.IncreasePlayerScore(game.GetCurrentPlayer(), c.val);
        }

        public void EndPlayerTurn()
        {
            StartPlayerTurn(game.NextPlayerTurn());
        }

        public void EndGame()
        {
            game.EndGame();
        }

        public Card? DrawCardFromDeck()
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
    }
}
