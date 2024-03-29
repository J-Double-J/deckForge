namespace DeckForge.GameRules.RoundConstruction.Interfaces
{
    /// <summary>
    /// Contains all the rules and information regarding to how a round plays from start to finish.
    /// </summary>
    public interface IRoundRules
    {
        /// <summary>
        /// Starts the Round and begins going through the <see cref="IPhase"/>s.
        /// </summary>
        public void StartRound();

        /// <summary>
        /// Ends the Round and called at the end of every Round.
        /// </summary>
        public void EndRound();

        /// <summary>
        /// Skips to the specified <see cref="IPhase"/> based on the position of the <see cref="IPhase"/> in <c>Phases</c> list.
        /// </summary>
        /// <param name="phaseNum">The position of the <see cref="IPhase"/> in the <c>Phases</c> list.</param>
        public void SkipToPhase(int phaseNum);
    }
}
