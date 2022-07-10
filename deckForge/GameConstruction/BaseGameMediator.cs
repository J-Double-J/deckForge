using deckForge.PlayerConstruction;
using deckForge.GameElements;
using deckForge.PhaseActions;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction
{
    public class BaseGameMediator : IGameMediator
    {
        private IGameController? _gameController;
        private List<IPlayer> _players = new();
        private Table? _table;

        //TODO: Remove PlayerCount
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

                _players = new List<IPlayer>();
            }
            catch
            {
                throw;
            }
        }

        public void RegisterPlayer(IPlayer player)
        {
            _players.Add(player);
        }

        public void RegisterTable(Table table)
        {
            _table = table;
        }

        public void RegisterGameController(IGameController gameController)
        {
            _gameController = gameController;
        }

        public int PlayerCount
        {
            get { return _players.Count; }
        }

        //TODO: Only used for testing at this moment. Does not fix bad ID's
        public void AddPlayer(IPlayer p)
        {
            _players.Add(p);
        }

        public void StartGame()
        {
            try
            {
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
            }
            catch
            {
                throw;
            }
        }
        public void StartPlayerTurn(int turn)
        {
            _players[turn].StartTurn();
        }

        public void PlayerPlayedCard(int id, Card c)
        {
            try
            {
                _table!.PlaceCardOnTable(id, c);
            }
            catch
            {
                throw;
            }
        }

        public void EndPlayerTurn()
        {
            try
            {
                StartPlayerTurn(_gameController!.NextPlayerTurn());
            }
            catch
            {
                throw;
            }
        }

        public void EndGame()
        {
            try
            {
                _gameController!.EndGame();
            }
            catch
            {
                throw;
            }
        }

        public Card? DrawCardFromDeck()
        {
            try
            {
                Card? c = _table!.DrawCardFromDeck();
                if (c != null)
                {
                    return c;
                }
                else
                {
                    return null;
                }
            }
            catch {
                throw;
            }
            
        }

        public IPlayer GetPlayerByID(int id)
        {
            try
            {
                if (_players is null) {
                    throw new ArgumentNullException("No players have been registered with GameMediator");
                }
                else
                    return _players[id];
            }
            catch
            {
                throw;
            }
        }

        public List<Card> GetPlayedCardsOfPlayer(int playerID)
        {
            try
            {
                return _table!.GetCardsForSpecificPlayer(playerID);
            }
            catch
            {
                throw;
            }

        }
        /*
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
        }*/

        public Card FlipSingleCard(int playerID, int cardPos, bool? facedown)
        {
            try
            {
                if (facedown is null)
                {
                    return _table!.Flip_SpecificCard_SpecificPlayer(playerID, cardPos);
                }
                else
                {
                    return _table!.Flip_SpecificCard_SpecificPlayer_SpecificWay(playerID, cardPos, (bool)facedown);
                }
            }
            catch
            {
                throw;
            }

        }

        public List<Card> PickUpAllCards_FromTable_FromPlayer(int playerID)
        {
            try
            {
                return _table!.PickUpAllCards_FromPlayer(playerID);
            }
            catch
            {
                throw;
            }
        }
    }
}
