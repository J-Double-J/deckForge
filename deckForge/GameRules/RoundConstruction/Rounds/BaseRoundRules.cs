using DeckForge.GameConstruction;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs;
using DeckForge.PhaseActions;

namespace DeckForge.GameRules.RoundConstruction.Rounds
{
    /// <summary>
    /// Base class for all rounds. Outlines <see cref="IPhase"/>s and the algorithm for <see cref="IPhase"/> looping.
    /// </summary>
    public abstract class BaseRoundRules : IRoundRules
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRoundRules"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that <see cref="BaseRoundRules"/> will use
        /// to communicate with other game elements.</param>
        public BaseRoundRules(IGameMediator gm)
        {
            GM = gm;
            gm.RegisterRoundRules(this);
            Phases = new();
            CurPhase = 0;
        }

        /// <summary>
        /// Gets or sets the list of all the Phases that the Round owns.
        /// </summary>
        public List<IPhase> Phases { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IGameMediator"/> that <see cref="BaseRoundRules"/> will use
        /// to communicate with other game elements.
        /// </summary>
        protected IGameMediator GM { get; }

        /// <summary>
        /// Gets or sets the Current Phase the Round is on.
        /// </summary>
        protected int CurPhase { get; set; }

        /// <summary>
        /// Starts the Round and begins iterating through its <see cref="IPhase"/>s.
        /// </summary>
        public virtual void StartRound()
        {
            foreach (var phase in Phases)
            {
                phase.StartPhase();
            }
        }

        /// <summary>
        /// Ends the round and sets the <c>CurPhase</c> to -1 and ends any <see cref="IGameAction{T}"/> ongoing
        /// in any <see cref="IPhase"/> in case of an early round termination.
        /// </summary>
        public virtual void EndRound()
        {
            // Prevents multiple clean ups if CurPhase is already -1
            if (CurPhase >= 0)
            {
                if (CurPhase >= 0 && CurPhase < Phases.Count)
                {
                    Phases[CurPhase].EndPhaseEarly();
                }

                CurPhase = -1;
            }
        }

        /// <summary>
        /// Skips ahead to an <see cref="IPhase"/> that could be out of
        /// the order of the typical <see cref="IPhase"/> iteration.
        /// </summary>
        /// <param name="phaseNum">Index of the <see cref="IPhase"/> in the <see cref="IPhase"/> list.</param>
        public virtual void SkipToPhase(int phaseNum)
        {
            try
            {
                // -1 to account for For loop in derived StartRounds increasing CurPhase by 1 after a phase completes.
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
        /// <param name="phaseNum">Index of <see cref="IPhase"/> in Phases list.</param>
        /// <returns>
        /// If the <see cref="IPhase"/> was executed in the hook returns <c>true</c>, else returns <c>false</c>.
        /// </returns>
        protected virtual bool NextPhaseHook(int phaseNum)
        {
            return false;
        }

        /// <summary>
        /// Skip to specific <see cref="IPhase"/> based on <see cref="SkipToPhaseEventArgs"/>.
        /// </summary>
        /// <param name="sender">
        /// Object that sent notification to skip to a phase.
        /// Usually another <see cref="IPhase"/>.
        /// </param>
        /// <param name="e">The arguments containing what <see cref="IPhase"/> to skip to.</param>
        protected virtual void Phase_SkipToPhaseEvent(object? sender, SkipToPhaseEventArgs e)
        {
            SkipToPhase(e.PhaseNum);
        }

        /// <summary>
        /// Called whenever a Round ends before all <see cref="IPhase"/>s would normally be
        /// finished.
        /// </summary>
        /// <param name="sender">Object that sent notification to end the Round early.</param>
        /// <param name="e">The args for <see cref="EndRoundEarlyArgs"/>.</param>
        protected virtual void Phase_EndRoundEarlyEvent(object? sender, EndRoundEarlyArgs e)
        {
            EndRound();
        }

        /// <summary>
        /// Executes given logic whenever an <see cref="IPhase"/> ends.
        /// </summary>
        /// <remarks>
        /// This may not be the function to override if the logic just need it to do things between <see cref="IPhase"/>s,
        /// as <see cref="NextPhaseHook"/> is more applicable.
        /// </remarks>
        /// <param name="sender">Object that notified that an <see cref="IPhase"/> ended
        /// (typically the <see cref="IPhase"/> itself).</param>
        /// <param name="e">The <see cref="PhaseEndedArgs"/> that will be used.</param>
        protected virtual void PhaseEndedEvent(object? sender, PhaseEndedArgs e)
        {
        }

        /// <summary>
        /// Subscribes the Round to listen to phases informing Round to skip to another <see cref="IPhase"/>.
        /// </summary>
        protected virtual void SubscribeToAllPhases_SkipToPhaseEvents()
        {
            foreach (IPhase phase in Phases)
            {
                phase.SkipToPhase += Phase_SkipToPhaseEvent;
            }
        }

        /// <summary>
        /// Subscribes the Round to listen to phases informing Round to end earlier than usual.
        /// </summary>
        protected virtual void SubscribeToAllPhases_EndRoundEarlyEvents()
        {
            foreach (IPhase phase in Phases)
            {
                phase.EndRoundEarly += Phase_EndRoundEarlyEvent;
            }
        }

        /// <summary>
        /// Subscribes the Round to listen to phases informing Round that the <see cref="IPhase"/> ended.
        /// </summary>
        protected virtual void SubscribeToAllPhases_PhasesEndedEvents()
        {
            foreach (IPhase phase in Phases)
            {
                phase.PhaseEnded += PhaseEndedEvent;
            }
        }
    }
}
