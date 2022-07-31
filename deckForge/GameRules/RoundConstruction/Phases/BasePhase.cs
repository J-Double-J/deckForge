using DeckForge.PhaseActions;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameConstruction;
using DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs;

namespace DeckForge.GameRules.RoundConstruction.Phases
{
    abstract public class BasePhase<T> : IPhase
    {
        protected readonly IGameMediator GM;
        protected int CurrentAction = 0;

        public event EventHandler<SkipToPhaseEventArgs>? SkipToPhase;
        public event EventHandler<EndRoundEarlyArgs>? EndRoundEarly;
        public event EventHandler<PhaseEndedArgs>? PhaseEnded;

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

        public int ActionCount
        {
            get
            {
                if (Actions is null)
                {
                    return 0;
                }
                else
                {
                    return Actions.Count;
                }
            }
        }

        virtual public void StartPhase() { }

        virtual public void EndPhase()
        {
            CurrentAction = -1;
        }

        public void EndPhaseEarly()
        {
            CurrentAction = -1;
        }

        protected virtual void OnSkipToPhase(SkipToPhaseEventArgs e)
        {
            CurrentAction = -1;
            SkipToPhase?.Invoke(this, e);
        }

        protected virtual void OnEndRoundEarly(EndRoundEarlyArgs e)
        {
            EndRoundEarly?.Invoke(this, e);
        }

        protected virtual void OnPhaseEnded(PhaseEndedArgs e)
        {
            PhaseEnded?.Invoke(this, e);
        }
    }
}
