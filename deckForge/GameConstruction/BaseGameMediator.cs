using deckForge.PlayerConstruction;
using deckForge.GameElements;
using deckForge.PhaseActions;
using deckForge.GameElements.Resources;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.GameRules.RoundConstruction.Rounds;

namespace deckForge.GameConstruction
{

    /// <summary>
    /// Mediates gameplay and iteractions between various objects such as <see cref="GameElements.Table"/>,
    /// <see cref="IPlayer"/>, <see cref="IRoundRules"/>, etc.
    /// </summary>
    public class BaseGameMediator : IGameMediator
    {
        protected bool gameOver = false;
        protected int CurRound = 0;
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
            player.PlayerMessageEvent += OnSimplePlayerMessage;
        }

        public void RegisterTable(Table table)
        {
            GameTable = table;
        }

        public void RegisterGameController(IGameController gameController)
        {
            GameController = gameController;
        }

        public void RegisterRoundRules(IRoundRules roundRules)
        {
            if (RoundRules is null)
                RoundRules = new();
            RoundRules.Add(roundRules);
        }

        public int PlayerCount
        {
            get { return Players!.Count; }
        }

        public List<int> TurnOrder
        {
            get { return GameController!.TurnOrder; }
        }

        public void ShiftTurnOrderClockwise()
        {
            GameController!.ShiftTurnOrderClockwise();
        }

        public void ShiftTurnOrderCounterClockwise()
        {
            GameController!.ShiftTurnOrderCounterClockwise();
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
            if (RoundRules is not null)
            {
                RoundRules[CurRound].EndRound();
                gameOver = true;
            }
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

        public IPlayer? GetPlayerByID(int playerID)
        {
            try
            {
                if (Players is null)
                {
                    throw new NullReferenceException("No players have been registered with GameMediator");
                }
                else
                {
                    int index = IndexOfPlayerByPlayerID(playerID);
                    return index != -1 ? Players[IndexOfPlayerByPlayerID(playerID)] : null;
                }
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

        public virtual void RoundEnded() { }

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
            int index = IndexOfPlayerByPlayerID(playerID);
            return index != -1 ? Players![index].ExecuteGameAction(action) : null;
        }

        public object? TellPlayerToDoActionAgainstAnotherPlayer(int playerID, int playerTargetID, IAction<IPlayer> action)
        {
            int playerIndex = IndexOfPlayerByPlayerID(playerID);
            int targetIndex = IndexOfPlayerByPlayerID(playerTargetID);

            if (playerIndex is not -1 && targetIndex is not -1)
                return Players![playerIndex].ExecuteGameActionAgainstPlayer(action, Players[targetIndex]);
            else
                return null;
        }

        public object? TellPlayerToDoActionAgainstMultiplePlayers(int playerID, IAction<IPlayer> action, bool includeSelf = false)
        {
            int playerIndex = IndexOfPlayerByPlayerID(playerID);

            return playerIndex is not -1 ?
            Players![playerIndex].ExecuteGameActionAgainstMultiplePlayers(action, Players, includeSelf)
            : null;
        }

        public object? TellPlayerToDoActionAgainstSpecificMultiplePlayers(int playerID, List<int> targets, IAction<IPlayer> action)
        {
            List<IPlayer> targettedPlayers = new();

            foreach (int targetID in targets)
            {
                int targetIndex = IndexOfPlayerByPlayerID(targetID);
                if (targetIndex is not -1)
                    targettedPlayers.Add(Players![targetIndex]);
            }

            int playerIndex = IndexOfPlayerByPlayerID(playerID);

            return playerIndex is not -1 ?
            Players![playerIndex].ExecuteGameActionAgainstMultiplePlayers(action, targettedPlayers)
            : null;
        }

        public void OnSimplePlayerMessage(object? sender, SimplePlayerMessageEventArgs e)
        {
            if (e.message == "LOSE_GAME")
            {
                IPlayer playerSender = (IPlayer)sender!;
                PlayerLost(playerSender.PlayerID);
            }
        }

        virtual public void PlayerLost(int playerID)
        {

            Players!.Remove(GetPlayerByID(playerID)!);

            //Could use LINQ most likely here
            List<int> remaingPlayerIDs = new();
            foreach (IPlayer player in Players)
            {
                remaingPlayerIDs.Add(player.PlayerID);
            }

            GameController!.UpdatePlayerList(remaingPlayerIDs);
            GameTable!.PickUpAllCards_FromPlayer(playerID);

            if (RoundRules is not null)
            {
                foreach (IRoundRules rr in RoundRules!)
                {
                    if (rr is PlayerRoundRules)
                    {
                        PlayerRoundRules playerRound = (PlayerRoundRules)rr;
                        playerRound.UpdatePlayerList(remaingPlayerIDs);
                    }
                }
            }

            if (Players.Count == 1)
            {
                EndGameWithWinner(GetPlayerByID(Players[0].PlayerID)!);
            }
        }

        /// <summary>
        /// Gets the index of an <see cref="IPlayer"/> by their <paramref name="playerID"/> in the Players array. 
        /// </summary>
        /// <remarks>
        /// This is the safest way to see if an <see cref="IPlayer"/> is still in the Players
        /// list, as an <see cref="IPlayer"/> may be knocked out of the game at any point.
        /// </remarks>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> to search for.</param>
        /// <returns>
        /// Index of the <see cref="IPlayer"/> in the Players list or
        /// -1 if the <see cref="IPlayer"/> is not found.
        /// </returns>
        protected int IndexOfPlayerByPlayerID(int playerID)
        {
            IPlayer? foundPlayer = Players!.Find(player => player.PlayerID == playerID);
            return foundPlayer is not null ? Players!.IndexOf(foundPlayer) : -1;
        }
    }
}
