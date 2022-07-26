using deckForge.PhaseActions;
using deckForge.PlayerConstruction;
using deckForge.GameConstruction;
using deckForge.GameRules.RoundConstruction.Interfaces;


namespace deckForge.GameRules.RoundConstruction.Phases
{
    abstract public class PlayerPhase : BasePhase<IPlayer>, IPlayerPhase, IPhase
    {
        protected int CurrentPlayerTurn;
        protected List<int> playerIDs;
        protected List<int>? ToBeUpdatedTurnOrder;

        public List<int> PlayerTurnOrder
        {
            get { return playerIDs; }
            private set { playerIDs = value; }
        }


        public PlayerPhase(IGameMediator gm, List<int> playerIDs, string phaseName = "") : base(gm, phaseName: phaseName)
        {
            this.playerIDs = playerIDs;
        }

        public PlayerPhase(IGameMediator gm, int playerID, string phaseName = "") : base(gm, phaseName: phaseName)
        {
            playerIDs = new()
            {
                playerID
            };
        }

        //Based on how many playerIDs this phase has, it decides what style to do
        virtual public void StartPhase()
        {
            CurrentAction = 0;
            if (playerIDs.Count == 1)
            {
                CurrentPlayerTurn = playerIDs[0];
                DoPhaseActions(playerIDs[0]);
            }
            else
            {
                DoPhaseActionsWithMultiplePlayers(playerIDs, actionNum: CurrentAction);
            }
        }

        //Players take an action, then wait for other players to finish action, then all go to next action etc
        virtual public void StartPhase(List<int> playerIDs)
        {
            DoPhaseActionsWithMultiplePlayers(playerIDs, actionNum: CurrentAction);
        }


        //Player does all actions in phase in order
        virtual public void StartPhase(int playerID)
        {
            CurrentPlayerTurn = playerID;
            DoPhaseActions(playerID);
        }

        //Each action must be done by a player before going to the next actions
        virtual protected void DoPhaseActionsWithMultiplePlayers(List<int> playerIDs, int actionNum)
        {

            foreach (int player in playerIDs)
            {
                CurrentPlayerTurn = player;
                PhaseActionLogic(player, actionNum, out bool handledAction);

                //Assumes that all actions are not targetted against another player
                if (!handledAction)
                    GM.TellPlayerToDoAction(player, Actions[actionNum]);
            }
            CurrentAction++;
            if (!(CurrentAction > ActionCount - 1) && !(CurrentAction < 0))
                DoPhaseActionsWithMultiplePlayers(playerIDs, CurrentAction);
            else
            {
                EndPhase();
            }
        }


        //Do all actions in one go
        virtual protected void DoPhaseActions(int playerID)
        {
            for (var actionNum = 0; actionNum < Actions.Count; actionNum++)
            {
                if (CurrentAction == -1 ||
                GM.GetPlayerByID(playerID) is null ||
                GM.GetPlayerByID(playerID)?.IsActive == false)
                {
                    break;
                }
                PhaseActionLogic(playerID, actionNum, out bool handledAction);

                //Assumes that all actions are not targetted against another player
                if (!handledAction)
                    GM.TellPlayerToDoAction(playerID, Actions[actionNum]);
            }

            EndPhase();
        }



        virtual public void EndPhase()
        {
            CurrentPlayerTurn = -1;
            CurrentAction = -1;

            if (ToBeUpdatedTurnOrder is not null)
            {
                PlayerTurnOrder = ToBeUpdatedTurnOrder;
                ToBeUpdatedTurnOrder = null;
            }
        }

        //Phases implement any logic for individual actions here. Should an action need to be executed in this function
        //(as is often the case if an action needs to be targetted) handledAction should be set to true
        virtual protected void PhaseActionLogic(int playerID, int actionNum, out bool handledAction) { handledAction = false; }

        virtual public void UpdateTurnOrder(List<int> newPlayerList)
        {
            ToBeUpdatedTurnOrder = newPlayerList;
        }

        /// <summary>
        /// Ends the current <see cref="IPlayer"/>'s turn in the phase.
        /// </summary>
        virtual public void EndPlayerTurn()
        {
            CurrentAction = -1;
        }
    }
}
