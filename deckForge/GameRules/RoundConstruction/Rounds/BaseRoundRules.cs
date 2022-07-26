using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameConstruction;
using deckForge.PhaseActions;

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
            gm.RegisterRoundRules(this);
        }
        virtual public void StartRound() { }
        protected abstract void NextPhase(int phaseNum);


        /// <summary>
        /// Ends the round and sets the <c>CurPhase</c> to -1 and ends any <see cref="IAction{T}"/> ongoing 
        /// in any <see cref="IPhase"/> in case of an early round termination. 
        /// </summary>
        virtual public void EndRound()
        {
            //Prevents multiple clean ups if CurPhase is already -1
            if (CurPhase >= 0)
            {
                if (CurPhase >= 0 && CurPhase < Phases.Count)
                    Phases[CurPhase].EndPhaseEarly();

                CurPhase = -1;
            }
        }

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

        virtual public void NextPhaseHook(int phaseNum, out bool handledRound)
        {
            handledRound = false;
        }

        virtual protected void Phase_SkipToPhaseEvent(object? sender, SkipToPhaseEventArgs e)
        {
            SkipToPhase(e.phaseNum);
        }

        virtual protected void Phase_EndRoundEarlyEvent(object? sender, EndRoundEarlyArgs e)
        {
            EndRound();
        }

        virtual protected void PhaseEndedEvent(object? sender, PhaseEndedArgs e) { }

        virtual protected void SubscribeToAllPhases_SkipToPhaseEvents()
        {
            foreach (IPhase phase in Phases)
                phase.SkipToPhase += Phase_SkipToPhaseEvent;
        }

        virtual protected void SubscribeToAllPhases_EndRoundEarlyEvents()
        {
            foreach (IPhase phase in Phases)
                phase.EndRoundEarly += Phase_EndRoundEarlyEvent;
        }

        virtual protected void SubscribeToAllPhases_PhasesEndedEvents()
        {
            foreach (IPhase phase in Phases)
                phase.PhaseEnded += PhaseEndedEvent;
        }
    }
}
