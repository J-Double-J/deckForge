using deckForge.PlayerConstruction;
using deckForge.GameElements;
using deckForge.PhaseActions;
using deckForge.GameElements.Resources;
using deckForge.GameRules.RoundConstruction.Interfaces;

namespace deckForge.GameConstruction
{

    /// <summary>
    /// Mediates gameplay and iteractions between various objects such as <see cref="GameElements.Table"/>,
    /// <see cref="IPlayer"/>, <see cref="IRoundRules"/>, etc.
    /// </summary>
    public class BaseGameMediator : IGameMediator
    {
        protected IGameController? GameController;
        protected List<IRoundRules>? RoundRules;
        protected List<IPlayer>? Players;
        protected Table? GameTable;

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

                Players = new List<IPlayer>();
            }
            catch
            {
                throw;
            }
        }

        public void RegisterPlayer(IPlayer player)
        {
            if (Players is null)
                Players = new();
            Players.Add(player);
        }

        public void RegisterTable(Table table)
        {
            GameTable = table;
        }

        public void RegisterGameController(IGameController gameController)
        {
            GameController = gameController;
        }

        public void RegisterRoundRules(IRoundRules roundRules) {
            if (RoundRules is null)
                RoundRules = new();
            RoundRules.Add(roundRules);
        }

        public int PlayerCount
        {
            get { return Players!.Count; }
        }

        public List<int> TurnOrder {
            get { return GameController!.TurnOrder; }
        }

        //TODO: Only used for testing at this moment. Does not fix bad ID's
        public void AddPlayer(IPlayer p)
        {
            Players!.Add(p);
        }

        /// <summary>
        /// Starts the game with the first <see cref="IPlayer"/>.
        /// </summary>
        public virtual void StartGame()
        {
            try
            {
                if (GameController is null || Players is null || GameTable is null || RoundRules is null)
                {
                    string errorMessage = "GameMediator is not fully initialized: \n";
                    if (GameController is null)
                        errorMessage += "GameController is null \n";
                    if (Players is null)
                        errorMessage += "Players is null \n";
                    if (GameTable is null)
                        errorMessage += "Table is null \n";
                    if (RoundRules is null)
                        errorMessage += "Round Rules is null \n";
                    throw new ArgumentNullException(errorMessage);
                }
                else
                {
                    StartPlayerTurn(GameController!.GetCurrentPlayer());
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Starts an <see cref="IPlayer"/>'s turn based on <paramref name="turn"/>
        /// </summary>
        /// <param name="turn">Turn number in the round used to tell that player in the List to
        /// start their turn.</param>
        public virtual void StartPlayerTurn(int turn)
        {
            Players![turn].StartTurn();
        }

        /// <summary>
        /// Puts a <see cref="Card"/> on the <see cref="Table"/> everytime <see cref="IPlayer"/> plays a <see cref="Card"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> who played a <see cref="Card"/>.</param>
        /// <param name="card"><see cref="Card"/> that was played.</param>
        public virtual void PlayerPlayedCard(int playerID, Card card)
        {
            try
            {
                GameTable!.PlaceCardOnTable(playerID, card);
            }
            catch
            {
                throw;
            }
        }

        public virtual void EndPlayerTurn()
        {
            try
            {
                StartPlayerTurn(GameController!.NextPlayerTurn());
            }
            catch
            {
                throw;
            }
        }

        public virtual void EndGame()
        {
            try
            {
                GameController!.EndGame();
            }
            catch
            {
                throw;
            }
        }

        public virtual void EndGameWithWinner(IPlayer winner)
        {
            Console.WriteLine($"Player {winner.PlayerID} wins!");
        }

        public virtual Card? DrawCardFromDeck()
        {
            try
            {
                Card? c = GameTable!.DrawCardFromDeck();
                if (c != null)
                {
                    return c;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }

        }

        public IPlayer GetPlayerByID(int id)
        {
            try
            {
                if (Players is null)
                {
                    throw new NullReferenceException("No players have been registered with GameMediator");
                }
                else
                    return Players[id];
            }
            catch
            {
                throw;
            }
        }

        public virtual List<Card> GetPlayedCardsOfPlayer(int playerID)
        {
            try
            {
                return GameTable!.GetCardsForSpecificPlayer(playerID);
            }
            catch
            {
                throw;
            }

        }

        public virtual void RoundEnded() {}

        public virtual Card FlipSingleCard(int playerID, int cardPos, bool? facedown)
        {
            try
            {
                if (facedown is null)
                {
                    return GameTable!.Flip_SpecificCard_SpecificPlayer(playerID, cardPos);
                }
                else
                {
                    return GameTable!.Flip_SpecificCard_SpecificPlayer_SpecificWay(playerID, cardPos, (bool)facedown);
                }
            }
            catch
            {
                throw;
            }

        }

        public virtual List<Card> PickUpAllCards_FromTable_FromPlayer(int playerID)
        {
            try
            {
                return GameTable!.PickUpAllCards_FromPlayer(playerID);
            }
            catch
            {
                throw;
            }
        }

        public object? TellPlayerToDoAction(int playerID, IAction<IPlayer> action)
        {
            return Players![playerID].ExecuteGameAction(action);
        }

        public object? TellPlayerToDoActionAgainstAnotherPlayer(int playerID, int playerTargetID, IAction<IPlayer> action)
        {
            return Players![playerID].ExecuteGameActionAgainstPlayer(action, Players[playerTargetID]);
        }

        public object? TellPlayerToDoActionAgainstMultiplePlayers(int playerID, IAction<IPlayer> action, bool includeSelf = false)
        {
            return Players![playerID].ExecuteGameActionAgainstMultiplePlayers(action, Players, includeSelf);
        }

        public object? TellPlayerToDoActionAgainstSpecificMultiplePlayers(int playerID, List<int> targets, IAction<IPlayer> action)
        {
            List<IPlayer> targettedPlayers = new();

            foreach (int targetID in targets)
            {
                targettedPlayers.Add(Players![targetID]);
            }

            return Players![playerID].ExecuteGameActionAgainstMultiplePlayers(action, targettedPlayers);
        }
    }
}
