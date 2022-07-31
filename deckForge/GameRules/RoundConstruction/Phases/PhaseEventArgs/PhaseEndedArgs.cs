using DeckForge.GameRules.RoundConstruction.Interfaces;

namespace DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs
{
    /// <summary>
    /// Arguments for when an <see cref="IPhase"/> ends. Has the name of the <see cref="IPhase"/>
    /// that ended and can have an optional message string.
    /// </summary>
    public class PhaseEndedArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhaseEndedArgs"/> class.
        /// </summary>
        /// <param name="phaseName">The name of the <see cref="IPhase"/> that ended.</param>
        public PhaseEndedArgs(string phaseName)
        {
            PhaseName = phaseName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhaseEndedArgs"/> class.
        /// </summary>
        /// <param name="phaseName">The name of the <see cref="IPhase"/> that ended.</param>
        /// <param name="message">A message that any subscribers can interpret.</param>
        public PhaseEndedArgs(string phaseName, string message)
        {
            PhaseName = phaseName;
            Message = message;
        }

        /// <summary>
        /// Gets the optional message of the <see cref="PhaseEndedArgs"/>.
        /// </summary>
        public string? Message { get; }

        /// <summary>
        /// Gets the name of the <see cref="IPhase"/> that ended.
        /// </summary>
        public string PhaseName { get; }
    }
}
