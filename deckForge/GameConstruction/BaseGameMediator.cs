using CardNamespace;
using deckForge.PlayerConstruction;
using deckForge.GameElements;
using deckForge.PhaseActions;

namespace deckForge.GameConstruction
{
    public class BaseGameMediator : IGameMediator
    {
        private IGameController? _gameController;
        private List<Player> _players = new();
        private Table? _table;

        public BaseGameMediator(int playerCount)
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


                _players = new List<Player>();
                for (var i = 0; i < playerCount; i++)
                    _players.Add(new Player(this, i));

            }
            catch
            {
                throw;
            }

        }

        public void RegisterPlayer(Player player) {
            _players.Add(player);
        }

        public void RegisterTable(Table table) {
            _table = table;
        }

        public void RegisterGameController(IGameController gameController) {
            _gameController = gameController;
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
            try {
                if (_gameController is null || _players is null || _table is null)
                {
                    string errorMessage = "GameMediator is not fully initialized: \n";
                    if (_gameController is null)
                        errorMessage += "GameController is null \n";
                    if (_players is null)
                        errorMessage += "Players is null \n";
                    if (_table is null)
                        errorMessage += "Table is null \n";
                    throw new ArgumentNullException(errorMessage);
                }
                else
                {
                    StartPlayerTurn(_gameController.GetCurrentPlayer());
                }
            } catch {
                throw;
            }
        }
        public void StartPlayerTurn(int turn)
        {
            _players[turn].StartTurn();
        }

        public void PlayerPlayedCard(int id, Card c)
        {
            try {
                _table!.PlaceCardOnTable(id, c);
            } catch {
                throw;
            }
        }

        public void EndPlayerTurn()
        {
            try {
                StartPlayerTurn(_gameController!.NextPlayerTurn());
            } catch {
                throw;
            }
        }

        public void EndGame()
        {
            try {
                _gameController!.EndGame();
            } catch {
                throw;
            }
        }

        //TODO implement
        public Card? DrawCardFromDeck()
        {
            return null;
            /*
            Card? c = _gameController.DrawCard();
            if (c != null)
            {
                return c;
            }
            else
            {
                return null;
            }*/
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
            try {
                return _table!.GetCardsForSpecificPlayer(playerID);
            } catch {
                throw;
            }
            
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

        public Card FlipSingleCard(int playerID, int cardPos, bool? facedown)
        {
            try {
                if (facedown is null)
                {
                    return _table!.Flip_SpecificCard_SpecificPlayer(playerID, cardPos);
                }
                else
                {
                    return _table!.Flip_SpecificCard_SpecificPlayer_SpecificWay(playerID, cardPos, (bool)facedown);
                }
            } catch {
                throw;
            }
            
        }

        public List<Card> PickUpAllCards_FromTable_FromPlayer(int playerID)
        {
            try {
                return _table!.PickUpAllCards_FromPlayer(playerID);
            } catch {
                throw;
            }
        }
    }
}
