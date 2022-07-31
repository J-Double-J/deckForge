using DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs;
using DeckForge.PhaseActions;

namespace DeckForge.GameRules.RoundConstruction.Interfaces
{
    /// <summary>
    /// A collection of related <see cref="IAction{T}"/>s and the logic that occurs between them.
    /// <see cref="IPhase"/>s are similar to <see cref="IRoundRules"/> but concern
    /// themselves with a smaller chunk of rules.
    /// </summary>
    public interface IPhase
    {
        /// <summary>
        /// Informs the subscribers (typically Round owner) to skip to another <see cref="IPhase"/> based on their ID in a Round.
        /// </summary>
        event EventHandler<SkipToPhaseEventArgs> SkipToPhase;

        /// <summary>
        /// Informs the subscribers (typically Round owner) to end the Round prematurely.
        /// </summary>
        event EventHandler<EndRoundEarlyArgs> EndRoundEarly;

        /// <summary>
        /// Informs subscribers that the <see cref="IPhase"/> has ended.
        /// </summary>
        event EventHandler<PhaseEndedArgs> PhaseEnded;

        /// <summary>
        /// Gets the name of the <see cref="IPhase"/>.
        /// </summary>
        public string PhaseName { get; }

        /// <summary>
        /// Starts the <see cref="IPhase"/> and begins iterating through its <see cref="IAction{T}"/>s.
        /// </summary>
        public void StartPhase();

        /// <summary>
        /// Ends the <see cref="IPhase"/> and executes any cleanup.
        /// </summary>
        public void EndPhase();

        /// <summary>
        /// Ends the <see cref="IPhase"/> prematurely.
        /// </summary>
        public void EndPhaseEarly();
    }
}
