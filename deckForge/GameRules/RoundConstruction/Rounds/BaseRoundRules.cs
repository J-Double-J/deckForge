using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Interfaces;


namespace deckForge.GameRules.RoundConstruction.Rounds
{
    public class BaseRoundRules : IRoundRules
    {

        protected List<IPhase> Phases;
        protected int CurPhase = 0;
        public BaseRoundRules(List<IPhase> phases, bool subscribeToAllPhaseEvents = true)
        {
            Phases = phases;
            if (subscribeToAllPhaseEvents)
            {
                SubscribeToAllPhases_SkipToPhaseEvents();
            }
        }

        virtual public void StartRound()
        {
            NextPhase(0);
        }

        virtual protected void NextPhase(int phaseNum)
        {
            if (phaseNum < Phases.Count)
            {
                Phases[CurPhase].StartPhase();
                phaseNum++;
            }
            if (phaseNum > Phases.Count - 1)
            {
                EndRound();
            }
            else
            {
                NextPhase(phaseNum);
            }
        }

        virtual public void EndRound() { }

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

        virtual protected void Phase_SkipToPhaseEvent(Object? sender, SkipToPhaseEventArgs e)
        {
            SkipToPhase(e.phaseNum);
        }

        virtual protected void SubscribeToAllPhases_SkipToPhaseEvents()
        {
            foreach (IPhase phase in Phases)
            {
                phase.SkipToPhase += Phase_SkipToPhaseEvent;
            }
        }
    }
}