using deckForge.PhaseActions;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameConstruction;

namespace deckForge.GameRules.RoundConstruction.Phases
{
    public class BasePhase<T>
    {
        protected readonly IGameMediator GM;
        public event EventHandler<SkipToPhaseEventArgs>? SkipToPhase;

        public BasePhase(IGameMediator gm, string phaseName = "")
        {
            Actions = new();
            PhaseName = phaseName;
            GM = gm;
        }

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

        protected virtual void OnSkipToPhase(SkipToPhaseEventArgs e)
        {
            SkipToPhase?.Invoke(this, e);
        }
    }
}
