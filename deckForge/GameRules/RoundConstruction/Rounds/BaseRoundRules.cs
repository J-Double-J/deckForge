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
        public BaseRoundRules(IGameMediator gm, bool subscribeToAllPhaseEvents = true)
        {
            if (subscribeToAllPhaseEvents)
            {
                SubscribeToAllPhases_SkipToPhaseEvents();
            }

            GM = gm;
        }

        virtual public void StartRound()
        {
            NextPhase(0);
        }

        virtual protected void NextPhase(int phaseNum)
        {
            NextPhaseHook(phaseNum, out bool repeatPhase);
            if (repeatPhase) {
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
            } else
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

        virtual public void NextPhaseHook(int phaseNum, out bool repeatPhase) {
            repeatPhase = true;
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