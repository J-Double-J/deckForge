using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameConstruction;
using deckForge.PhaseActions;

namespace deckForge.GameRules.RoundConstruction.Rounds
{
    abstract public class BaseRoundRules : IRoundRules
    {

        /// <summary>
        /// List of all the Phases that the Round owns.
        /// </summary>
        public List<IPhase> Phases { get; protected set; }
        protected int CurPhase = 0;
        protected readonly IGameMediator GM;

        public BaseRoundRules(IGameMediator gm)
        {
            GM = gm;
            gm.RegisterRoundRules(this);
            Phases = new();
        }

        /// <summary>
        /// Begins the Round by iterating through all its <see cref="IPhase">s and handling any rules
        /// between them.
        /// </summary>
        abstract public void StartRound();

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

        /// <summary>
        /// Skips ahead to an <see cref="IPhase"/> that could be out of 
        /// the order of the typical <see cref="IPhase"/> iteration.
        /// </summary>
        /// <param name="phaseNum">Index of the <see cref="IPhase"/> in the <see cref="IPhase"/> list.</param>
        virtual public void SkipToPhase(int phaseNum)
        {
            try
            {
                //-1 to account for For loop in derived StartRounds increasing CurPhase by 1 after a phase completes.
                CurPhase = phaseNum - 1;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///  Called before every <see cref="IPhase"/> to execute any logic in the function.
        /// </summary>
        /// <remarks>
        /// Override this function in derived class. Returns false on default.
        /// </remarks>
        /// <param name="phaseNum">Index of <see cref="IPhase"/> in Phases list</param>
        /// <param name="handledRound"></param>
        /// <returns>
        /// If the <see cref="IPhase"/> was executed in the hook returns <c>true</c>, else returns <c>false</c>.
        /// </returns>
        virtual protected bool NextPhaseHook(int phaseNum)
        {
            return false;
        }

        /// <summary>
        /// Skip to specific <see cref="IPhase"/> based on <see cref="SkipToPhaseEventArgs"/>.
        /// </summary>
        /// <param name="sender">
        /// Object that sent notification to skip to a phase.
        /// Usually another phase.
        /// </param>
        /// <param name="e">The arguments containing what <see cref="IPhase"/> to skip to.</param>
        virtual protected void Phase_SkipToPhaseEvent(object? sender, SkipToPhaseEventArgs e)
        {
            SkipToPhase(e.phaseNum);
        }

        virtual protected void Phase_EndRoundEarlyEvent(object? sender, EndRoundEarlyArgs e)
        {
            EndRound();
        }

        virtual protected void PhaseEndedEvent(object? sender, PhaseEndedArgs e) { }


        /// <summary>
        /// Subscribes the Round to listen to phases informing Round to skip to another <see cref="IPhase"/>.
        /// </summary>
        virtual protected void SubscribeToAllPhases_SkipToPhaseEvents()
        {
            foreach (IPhase phase in Phases)
                phase.SkipToPhase += Phase_SkipToPhaseEvent;
        }

        /// <summary>
        /// Subscribes the Round to listen to phases informing Round to end earlier than usual.
        /// </summary>
        virtual protected void SubscribeToAllPhases_EndRoundEarlyEvents()
        {
            foreach (IPhase phase in Phases)
                phase.EndRoundEarly += Phase_EndRoundEarlyEvent;
        }

        /// <summary>
        /// Subscribes the Round to listen to phases informing Round that the <see cref="IPhase"/> ended.
        /// </summary>
        virtual protected void SubscribeToAllPhases_PhasesEndedEvents()
        {
            foreach (IPhase phase in Phases)
                phase.PhaseEnded += PhaseEndedEvent;
        }
    }
}
