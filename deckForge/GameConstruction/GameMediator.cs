using CardNamespace;
using deckForge.PlayerConstruction;
using deckForge.GameElements;
using deckForge.PhaseActions;

namespace deckForge.GameConstruction
{
    public class GameMediator
    {
        private Game _game;
        private List<Player> _players;
        private Score _score;
        private Table _table;

        public GameMediator(int playerCount)
        {
            try
            {
                if (playerCount < 0)
                {
                    throw new ArgumentException(message: "Cannot have negative players");
                }
                else if (playerCount > 12)
                {
                    throw new ArgumentException(message: "Game cannot have more than 12 players");
                }

                _game = new Game(playerCount);
                _score = new Score(playerCount);
                _table = new Table(this, playerCount);


                _players = new List<Player>();
                for (var i = 0; i < playerCount; i++)
                    _players.Add(new Player(this, i));

            }
            catch
            {
                throw;
            }

        }

        public int PlayerCount
        {
            get { return _players.Count; }
        }

        //TODO: Only used for testing at this moment. Does not fix bad ID's
        public void AddPlayer(Player p)
        {
            _players.Add(p);
        }

        public void StartGame()
        {
            StartPlayerTurn(_game.GetCurrentPlayer());
        }
        public void StartPlayerTurn(int turn)
        {
            _players[turn].StartTurn();
        }

        public void PlayerPlayedCard(int id, Card c)
        {
            _table.PlaceCardOnTable(id, c);
        }

        public void EndPlayerTurn()
        {
            StartPlayerTurn(_game.NextPlayerTurn());
        }

        public void EndGame()
        {
            _game.EndGame();
        }

        public Card? DrawCardFromDeck()
        {

            Card? c = _game.DrawCard();
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
            try
            {
                return _players[id];
            }
            catch
            {
                throw;
            }
        }

        public List<Card> GetPlayedCardsOfPlayer(int playerID)
        {
            return _table.GetCardsForSpecificPlayer(playerID);
        }

        public void TellPlayerToExecuteCommand(int playerID, PlayerGameAction command)
        {
            try
            {
                _players[playerID].ExecuteGameAction(command);
            }
            catch
            {
                throw;
            }
        }
    }
}
