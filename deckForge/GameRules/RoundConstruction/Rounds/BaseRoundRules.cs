using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameConstruction;

namespace deckForge.GameRules.RoundConstruction.Rounds
{
    abstract public class BaseRoundRules : IRoundRules
    {

        abstract public List<IPhase> Phases { get; }
        protected int CurPhase = 0;
        protected readonly IGameMediator GM;
        public BaseRoundRules(IGameMediator gm)
        {
            GM = gm;
        }
        virtual public void StartRound() { }
        protected abstract void NextPhase(int phaseNum);

        virtual public void EndRound() { CurPhase = -1; }

        virtual public void SkipToPhase(int phaseNum)
        {
            try
            {
                CurPhase = phaseNum;
                NextPhase(CurPhase);
            }
            catch
            {
                throw;
            }
        }

        virtual public void NextPhaseHook(int phaseNum, out bool handledRound) {
            handledRound = false;
        }

        virtual protected void Phase_SkipToPhaseEvent(object? sender, SkipToPhaseEventArgs e)
        {
            SkipToPhase(e.phaseNum);
        }

        virtual protected void Phase_EndRoundEarlyEvent(object? sender, EndRoundEarlyArgs e) {
            EndRound();
        }

        virtual protected void SubscribeToAllPhases_SkipToPhaseEvents()
        {
            foreach (IPhase phase in Phases)
            {
                phase.SkipToPhase += Phase_SkipToPhaseEvent;
            }
        }

        virtual protected void SubscribeToAllPhases_EndRoundEarlyEvents() {
            foreach (IPhase phase in Phases)
                phase.EndRoundEarly += Phase_EndRoundEarlyEvent;
        }
    }
}
