using deckForge.PhaseActions;
using deckForge.PlayerConstruction;
using deckForge.GameConstruction;

namespace deckForge.GameRules.RoundConstruction.Phases
{
    public class PlayerPhase : BasePhase<IPlayer>
    {

        protected int CurrentPlayer = 0;
        public PlayerPhase(IGameMediator gm, string phaseName = "") : base(gm, phaseName: phaseName)
        {}


        public override void StartPhase()
        {
            throw new NotImplementedException("PlayerPhases cannot be started without parameters, try `StartPhase(List<int> playerIDs)`, or `StartPhase(playerID)`");
        }

        //Players take an action, then wait for other players to finish action, then all go to next action etc
        virtual public void StartPhase(List<int> playerIDs)
        {
            DoPhaseActionsWithMultiplePlayers(playerIDs, actionNum: CurrentAction);
        }


        //Player does all actions in phase in order
        virtual public void StartPhase(int playerID) {
            DoPhaseActions(playerID);
        }

        //Each action must be done by a player before going to the next actions
        virtual protected void DoPhaseActionsWithMultiplePlayers(List<int> playerIDs, int actionNum)
        {
            
            foreach (int player in playerIDs)
            {
                PhaseActionLogic(player, actionNum, out bool handledAction);
                
                //Assumes that all actions are not targetted against another player
                if (!handledAction)
                    GM.TellPlayerToDoAction(player, Actions[actionNum]);
            }
            CurrentAction++;
            if (CurrentAction < ActionCount - 1)
                DoPhaseActionsWithMultiplePlayers(playerIDs, CurrentAction);
            else
                EndPhase();
        }


        //Do all actions in one go
        virtual protected void DoPhaseActions(int playerID) {
            for (var actionNum = 0; actionNum < Actions.Count; actionNum++) {
                PhaseActionLogic(playerID, actionNum, out bool handledAction);

                //Assumes that all actions are not targetted against another player
                if (!handledAction)
                    GM.TellPlayerToDoAction(playerID, Actions[actionNum]);
            }

            EndPhase();
        }



        new virtual public void EndPhase()
        {
            base.EndPhase();
        }

        //Phases implement any logic for individual actions here. Should an action need to be executed in this function
        //(as is often the case if an action needs to be targetted) handledAction should be set to true
        virtual protected void PhaseActionLogic(int playerID, int actionNum, out bool handledAction) { handledAction = false; }
    }
}
