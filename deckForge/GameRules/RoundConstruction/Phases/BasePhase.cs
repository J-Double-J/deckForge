using deckForge.PhaseActions;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PlayerConstruction;

namespace deckForge.GameRules.RoundConstruction.Phases
{
    public class BasePhase<T> : IPhase
    {
        protected int CurrentAction = 0;
        public event EventHandler<SkipToPhaseEventArgs>? SkipToPhase;
        public string PhaseName
        {
            get;
        }

        protected List<IAction<T>> Actions
        {
            get;
            set;
        }

        public int ActionCount { 
            get { 
                if (Actions is null) {
                    return 0;
                } else {
                    return Actions.Count;
                }
            } 
        }

        public BasePhase(string phaseName = "")
        {
            Actions = new();
            PhaseName = phaseName;
        }

        //These do nothing at the moment, but derived classes will call these in case this changes
        virtual public void StartPhase() { }
        virtual public void EndPhase() { }

        protected virtual void OnSkipToPhase(SkipToPhaseEventArgs e)
        {
            SkipToPhase?.Invoke(this, e);
        }
    }
}
