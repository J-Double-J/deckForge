using DeckForge.GameConstruction;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameRules.RoundConstruction.Phases
{
    /// <summary>
    /// An <see cref="IPhase"/> that concerns itself with managing <see cref="IPlayer"/>s playing through its ruleset.
    /// </summary>
    public abstract class PlayerPhase : BasePhase<IPlayer>, IPlayerPhase, IPhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPhase"/> class. Concerns itself with one or more <see cref="IPlayer"/>s.
        /// </summary>
        /// <param name="gm">
        /// <see cref="IGameMediator"/> that the <see cref="PlayerPhase"/> will use to communicate
        /// with other game elements.
        /// </param>
        /// <param name="playerIDs">IDs of the <see cref="IPlayer"/>s managed by the <see cref="PlayerPhase"/>.</param>
        /// <param name="phaseName">Name of the <see cref="PlayerPhase"/>.</param>
        public PlayerPhase(IGameMediator gm, List<int> playerIDs, string phaseName = "")
            : base(gm, phaseName: phaseName)
        {
            PlayerIDs = playerIDs;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPhase"/> class. Concerns itself with only one <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="gm">
        /// <see cref="IGameMediator"/> that the <see cref="PlayerPhase"/> will use to communicate
        /// with other game elements.
        /// </param>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> managed by the <see cref="PlayerPhase"/>.</param>
        /// <param name="phaseName">Name of the <see cref="PlayerPhase"/>.</param>
        public PlayerPhase(IGameMediator gm, int playerID, string phaseName = "")
            : base(gm, phaseName: phaseName)
        {
            PlayerIDs = new()
            {
                playerID
            };
        }

        /// <summary>
        /// Gets or sets the TurnOrder of the <see cref="IPlayer"/>s in the <see cref="PlayerPhase"/>.
        /// </summary>
        public List<int> PlayerTurnOrder
        {
            get { return PlayerIDs; }
            protected set { PlayerIDs = value; }
        }

        /// <summary>
        /// Gets or sets the current ID of the <see cref="IPlayer"/>.
        /// </summary>
        protected int CurrentPlayerTurn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of IDs of the <see cref="IPlayer"/>s in the game.
        /// </summary>
        protected List<int> PlayerIDs
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the new turn order that PlayerTurnOrder is to be set to later.
        /// </summary>
        protected List<int>? ToBeUpdatedTurnOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Starts the <see cref="IPlayerPhase"/> and based on the number of <see cref="IPlayer"/> IDs this
        /// <see cref="IPlayerPhase"/> manages decides what method of <see cref="IGameAction{T}"/> iteration it will use.
        /// </summary>
        public override void StartPhase()
        {
            CurrentAction = 0;
            if (PlayerIDs.Count == 1)
            {
                CurrentPlayerTurn = PlayerIDs[0];
                DoPhaseActions(PlayerIDs[0]);
            }
            else
            {
                DoPhaseActionsWithMultiplePlayers(PlayerIDs, actionNum: CurrentAction);
            }
        }

        /// <inheritdoc/>
        public virtual void StartPhase(List<int> playerIDs)
        {
            DoPhaseActionsWithMultiplePlayers(playerIDs, actionNum: CurrentAction);
        }

        /// <inheritdoc/>
        public virtual void StartPhase(int playerID)
        {
            CurrentPlayerTurn = playerID;
            DoPhaseActions(playerID);
        }

        /// <inheritdoc/>
        public override void EndPhase()
        {
            CurrentPlayerTurn = -1;
            CurrentAction = -1;

            if (ToBeUpdatedTurnOrder is not null)
            {
                PlayerTurnOrder = ToBeUpdatedTurnOrder;
                ToBeUpdatedTurnOrder = null;
            }
        }

        /// <inheritdoc/>
        public virtual void UpdateTurnOrder(List<int> newPlayerList)
        {
            ToBeUpdatedTurnOrder = newPlayerList;
        }

        /// <inheritdoc/>
        public virtual void EndPlayerTurn()
        {
            CurrentAction = -1;
        }

        /// <summary>
        /// Executes the <see cref="PlayerPhase"/>'s <see cref="IGameAction{T}"/>s in order where each
        /// <see cref="IPlayer"/> must execute the <see cref="IGameAction{T}"/> before any <see cref="IPlayer"/>
        /// can execute the next <see cref="IGameAction{T}"/>.
        /// </summary>
        /// <remarks>
        /// All <see cref="IGameAction{T}"/>s by default presume that the action is untargetted. If it should be targetted, consider overriding
        /// this function, or overriding <see cref="PhaseActionLogic(int, int, out bool)"/>.
        /// </remarks>
        /// <param name="playerIDs">IDs of all the <see cref="IPlayer"/>s taking <see cref="IGameAction{T}"/>s.</param>
        /// <param name="actionNum">Index of the <see cref="IGameAction{T}"/> in the list managed by the <see cref="PlayerPhase"/>.</param>
        protected virtual void DoPhaseActionsWithMultiplePlayers(List<int> playerIDs, int actionNum)
        {
            for (int i = actionNum; i < ActionCount; i++)
            {
                CurrentAction = i;
                foreach (int player in playerIDs)
                {
                    CurrentPlayerTurn = player;

                    if (!PhaseActionLogic(player, i))
                    {
                        GM.TellPlayerToDoAction(player, Actions[i]);
                    }
                }

                if (CurrentAction < 0)
                {
                    break;
                }
            }

            EndPhase();
        }

        /// <summary>
        /// <see cref="IPlayer"/> executes all <see cref="IGameAction{T}"/>s in order.
        /// </summary>
        /// <remarks>
        /// All <see cref="IGameAction{T}"/>s by default presume that the action is untargetted. If it should be targetted, consider overriding
        /// this function, or overriding <see cref="PhaseActionLogic(int, int)"/>.
        /// </remarks>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> executing the <see cref="IGameAction{T}"/>.</param>
        protected virtual void DoPhaseActions(int playerID)
        {
            for (var actionNum = 0; actionNum < Actions.Count; actionNum++)
            {
                if (CurrentAction == -1 ||
                GM.GetPlayerByID(playerID) is null ||
                GM.GetPlayerByID(playerID)?.IsActive == false)
                {
                    break;
                }

                if (!PhaseActionLogic(playerID, actionNum))
                {
                    GM.TellPlayerToDoAction(playerID, Actions[actionNum]);
                }
            }

            EndPhase();
        }

        /// <summary>
        /// Any logic or extra function calls should be overriden here and will be called before each <see cref="IPlayer"/>
        /// executes an <see cref="IGameAction{T}"/>.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> executing the <see cref="IGameAction{T}"/>.</param>
        /// <param name="actionNum">Index of the <see cref="IGameAction{T}"/> in the <see cref="PhaseActions"/> list.</param>
        /// <returns>
        /// Returns true if an <see cref="IGameAction{T}"/> is handled in this function call, else false.
        /// </returns>
        protected virtual bool PhaseActionLogic(int playerID, int actionNum)
        {
            return false;
        }
    }
}
