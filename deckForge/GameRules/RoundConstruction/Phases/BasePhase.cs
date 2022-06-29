using deckForge.PhaseActions;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PlayerConstruction;

namespace deckForge.GameRules.RoundConstruction.Phases
{
    public class BasePhase<T> : IPhase
    {
        protected int CurrentAction = 0;
        public string PhaseName
        {
            get;
        }

        public List<IAction<T>> Actions
        {
            get;
            private set;
        }

        public int ActionCount { get { return Actions.Count; } }

        public BasePhase(List<IAction<T>>? actions = null, string phaseName = "")
        {
            if (actions == null)
            {
                actions = new List<IAction<T>>();
                actions.Add(new DoNothingAction<T>());
            }
            Actions = actions;
            PhaseName = phaseName;
        }

        //These do nothing at the moment, but derived classes will call these in case this changes
        virtual public void StartPhase() { }
        virtual public void EndPhase() { }
    }
}
