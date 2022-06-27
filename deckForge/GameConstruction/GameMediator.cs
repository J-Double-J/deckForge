using CardNamespace;
using deckForge.PlayerConstruction;

namespace deckForge.GameConstruction
{
    public class GameMediator
    {
        Game game;
        List<Player> players;
        Score score;

        public GameMediator(int playerCount)
        {
            try
            {
                game = new Game(playerCount);
                score = new Score(playerCount);

                players = new List<Player>();
                for (var i = 0; i < playerCount; i++)
                    players.Add(new Player(this, i));
            }
            catch
            {
                throw;
            }

        }

        //TODO: Only used for testing at this moment. Does not fix bad ID's
        public void AddPlayer(Player p)
        {
            players.Add(p);
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

        public Player GetPlayerByID(int id)
        {
            return players[id];
        }
    }
}
