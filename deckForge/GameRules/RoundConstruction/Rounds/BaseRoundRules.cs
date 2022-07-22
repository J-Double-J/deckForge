using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameConstruction;

//HEY Josh, a lot of this has to be abstracted out. Figure out what methods are going to be implemented
//by NPC and PlayerRounds and make those abstract. Remove pretty much everything else.
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
        virtual public void StartRound() { }
        protected abstract void NextPhase(int phaseNum);

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

        virtual public void NextPhaseHook(int phaseNum, out bool handledRound) {
            handledRound = false;
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
