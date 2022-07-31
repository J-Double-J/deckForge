using DeckForge.GameRules.RoundConstruction.Interfaces;

namespace DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs
{
    /// <summary>
    /// Arguments for events that tell Rounds to skip to another <see cref="IPhase"/>.
    /// </summary>
    public class SkipToPhaseEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkipToPhaseEventArgs"/> class.
        /// </summary>
        /// <param name="phaseNum">Index of the <see cref="IPhase"/> to skip to in the Round.</param>
        public SkipToPhaseEventArgs(int phaseNum) { this.PhaseNum = phaseNum; }

        /// <summary>
        /// Gets the Index of the <see cref="IPhase"/> to skip to in the Round.
        /// </summary>
        public int PhaseNum { get; }

    }
}
