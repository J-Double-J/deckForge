﻿using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameRules.RoundConstruction.Rounds;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace DeckForge.GameConstruction
{
    /// <summary>
    /// Mediates gameplay and iteractions between various objects such as <see cref="Table"/>,
    /// <see cref="IPlayer"/>, <see cref="IRoundRules"/>, etc.
    /// </summary>
    public class BaseGameMediator : IGameMediator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGameMediator"/> class.
        /// </summary>
        /// <param name="playerCount">Number of <see cref="IPlayer"/>s in the game.</param>
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

            GameOver = false;
            CurRound = 0;
        }

        /// <inheritdoc/>
        public int PlayerCount
        {
            get { return Players!.Count; }
        }

        /// <inheritdoc/>
        public List<int> TurnOrder
        {
            get { return GameController!.TurnOrder; }
        }

        /// <inheritdoc/>
        public List<List<Card>> CurrentTableState
        {
            get { return GameTable!.TableState; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IGameController"/>.
        /// </summary>
        protected IGameController? GameController { get; set; }

        /// <summary>
        /// Gets or sets the list of Rounds that <see cref="BaseGameMediator"/> manages.
        /// </summary>
        protected List<IRoundRules>? RoundRules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game is over or not.
        /// </summary>
        protected bool GameOver { get; set; }

        /// <summary>
        /// Gets or sets the current round that the GameMediator is on.
        /// </summary>
        protected int CurRound { get; set; }

        /// <summary>
        /// Gets or sets the current list of <see cref="IPlayer"/>s that are playing.
        /// </summary>
        protected List<IPlayer>? Players { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Table"/> that the game is played on.
        /// </summary>
        protected Table? GameTable { get; set; }

        /// <inheritdoc/>
        public void RegisterPlayer(IPlayer player)
        {
            if (Players is null)
            {
                Players = new ();
            }

            Players.Add(player);
            player.PlayerMessageEvent += OnSimplePlayerMessage;
        }

        /// <inheritdoc/>
        public void RegisterTable(Table table)
        {
            GameTable = table;
        }

        /// <inheritdoc/>
        public void RegisterGameController(IGameController gameController)
        {
            GameController = gameController;
        }

        /// <inheritdoc/>
        public void RegisterRoundRules(IRoundRules roundRules)
        {
            if (RoundRules is null)
            {
                RoundRules = new ();
            }

            RoundRules.Add(roundRules);
        }

        /// <inheritdoc/>
        public void ShiftTurnOrderClockwise()
        {
            GameController!.ShiftTurnOrderClockwise();
        }

        /// <inheritdoc/>
        public void ShiftTurnOrderCounterClockwise()
        {
            GameController!.ShiftTurnOrderCounterClockwise();
        }

        /// <inheritdoc/>
        public virtual void StartGame()
        {
            try
            {
                CheckGameMediatorSetUp();
                while (GameOver != true)
                {
                    CurRound = 0;
                    foreach (IRoundRules rr in RoundRules!)
                    {
                        if (GameOver != true)
                        {
                            rr.StartRound();
                            CurRound++;
                        }

                        // Since a game can end midround, check again.
                        if (GameOver != true)
                        {
                            RoundEndedHook();
                        }
                    }

                    if (GameOver is not true)
                    {
                        AfterAllRoundsEndedHook();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Starts an <see cref="IPlayer"/>'s turn based on <paramref name="turn"/>.
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public virtual void EndGameWithWinner(IPlayer winner)
        {
            Console.WriteLine($"Player {winner.PlayerID} wins!");
            if (RoundRules is not null)
            {
                RoundRules[CurRound].EndRound();
                GameOver = true;
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public object? TellPlayerToDoAction(int playerID, IAction<IPlayer> action)
        {
            int index = IndexOfPlayerByPlayerID(playerID);
            return index != -1 ? Players![index].ExecuteGameAction(action) : null;
        }

        /// <inheritdoc/>
        public object? TellPlayerToDoActionAgainstAnotherPlayer(int playerID, int playerTargetID, IAction<IPlayer> action)
        {
            int playerIndex = IndexOfPlayerByPlayerID(playerID);
            int targetIndex = IndexOfPlayerByPlayerID(playerTargetID);

            if (playerIndex is not -1 && targetIndex is not -1)
            {
                return Players![playerIndex].ExecuteGameActionAgainstPlayer(action, Players[targetIndex]);
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public object? TellPlayerToDoActionAgainstMultiplePlayers(int playerID, IAction<IPlayer> action, bool includeSelf = false)
        {
            int playerIndex = IndexOfPlayerByPlayerID(playerID);

            return playerIndex is not -1 ?
            Players![playerIndex].ExecuteGameActionAgainstMultiplePlayers(action, Players, includeSelf)
            : null;
        }

        /// <inheritdoc/>
        public object? TellPlayerToDoActionAgainstSpecificMultiplePlayers(int playerID, List<int> targets, IAction<IPlayer> action)
        {
            List<IPlayer> targettedPlayers = new ();

            foreach (int targetID in targets)
            {
                int targetIndex = IndexOfPlayerByPlayerID(targetID);
                if (targetIndex is not -1)
                {
                    targettedPlayers.Add(Players![targetIndex]);
                }
            }

            int playerIndex = IndexOfPlayerByPlayerID(playerID);

            return playerIndex is not -1 ?
            Players![playerIndex].ExecuteGameActionAgainstMultiplePlayers(action, targettedPlayers)
            : null;
        }

        /// <inheritdoc/>
        public void OnSimplePlayerMessage(object? sender, SimplePlayerMessageEventArgs e)
        {
            if (e.message == "LOSE_GAME")
            {
                IPlayer playerSender = (IPlayer)sender!;
                PlayerLost(playerSender.PlayerID);
            }
        }

        /// <inheritdoc/>
        public virtual void PlayerLost(int playerID)
        {
            Players!.Remove(GetPlayerByID(playerID)!);

            // Could use LINQ most likely here7
            List<int> remaingPlayerIDs = new ();
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
                    if (rr is PlayerRoundRules playerRound)
                    {
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

        /// <summary>
        /// Verifies that all important objects have registered themselves with the GameMediator before the game starts.
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws exception if any important object is missing.</exception>
        protected virtual void CheckGameMediatorSetUp()
        {
            if (GameController is null || Players is null || GameTable is null || RoundRules is null)
            {
                string errorMessage = "GameMediator is not fully initialized: \n";
                if (GameController is null)
                {
                    errorMessage += "GameController is null \n";
                }

                if (Players is null)
                {
                    errorMessage += "Players is null \n";
                }

                if (GameTable is null)
                {
                    errorMessage += "Table is null \n";
                }

                if (RoundRules is null)
                {
                    errorMessage += "Round Rules is null \n";
                }

                throw new ArgumentNullException(errorMessage);
            }
        }

        /// <summary>
        /// Any logic put in this hook will execute after all <see cref = "IRoundRules"/> in a loop have
        /// executed and before it starts with <see cref="IGameMediator"/>'s first <see cref = "IRoundRules"/> again.
        /// </summary>
        /// <remarks>
        /// If one wanted to shift the turn order and do certain checks before the top of a round, override this function.
        /// Functionally similar to RoundEndedHook(). This function is not called if the game ends.
        /// before loop is finished.
        /// </remarks>
        protected virtual void AfterAllRoundsEndedHook()
        {
        }

        /// <summary>
        /// Called everytime any <see cref = "IRoundRules"/> has ended. Override to execute any logic between rounds.
        /// </summary>
        /// <remarks>
        /// Functionally similar to AfterAllRoundsHook(). This function is not called if the game
        /// ends in the middle of a round.
        /// </remarks>
        protected virtual void RoundEndedHook()
        {
        }
    }
}
