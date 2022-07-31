namespace DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs
{
    /// <summary>
    /// Arguments for the EndRoundEarly event. Can contain an optional
    /// message that the Round can interpret.
    /// </summary>
    public class EndRoundEarlyArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndRoundEarlyArgs"/> class.
        /// </summary>
        public EndRoundEarlyArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndRoundEarlyArgs"/> class.
        /// </summary>
        /// <param name="message">Message to pass along to the Round or any other subscriber.</param>
        public EndRoundEarlyArgs(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the Message stored in the <see cref="EndRoundEarlyArgs"/>.
        /// </summary>
        public string? Message { get; }
    }
}
