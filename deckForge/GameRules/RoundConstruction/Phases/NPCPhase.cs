using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameConstruction;

namespace DeckForge.GameRules.RoundConstruction.Phases
{
    public class NPCPhase<T> : BasePhase<T>, INPCPhase<T>
    {
        public NPCPhase(IGameMediator gm, string phaseName = "") : base(gm, phaseName: phaseName) { }
        virtual public void StartPhase(T t)
        {
            DoPhaseActions(t);
        }

        //Do all actions in one go, passing in the object type that IAction<T> is
        virtual protected void DoPhaseActions(T t)
        {
            for (var actionNum = 0; actionNum < Actions.Count; actionNum++)
            {
                PhaseActionLogic(out bool handledAction);
                if (!handledAction)
                    Actions[actionNum].Execute(t);
            }

            EndPhase();
        }

        //Phases implement any logic for individual actions here. Should an action need to be executed in this function
        //(as is often the case if an action needs to be targetted) handledAction should be set to true
        virtual protected void PhaseActionLogic(out bool handledAction) { handledAction = false; }
    }
}
