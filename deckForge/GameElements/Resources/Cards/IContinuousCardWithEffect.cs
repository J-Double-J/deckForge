namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// Any card that is meant to be played on the <see cref="Table"/> and have an effect
    /// that can occur will be an <see cref="IContinuousCardWithEffect"/>.
    /// </summary>
    public interface IContinuousCardWithEffect
    {
        /// <summary>
        /// Executes card effect on start of turn.
        /// </summary>
        /// <remarks>Does nothing by default.</remarks>
        public void OnStartTurn();

        /// <summary>
        /// Executes card effect on end of turn.
        /// </summary>
        /// <remarks>Does nothing by default.</remarks>
        public void OnEndTurn();

        /// <summary>
        /// Executes card effect on event trigger.
        /// </summary>
        /// <param name="sender"><c>Object</c> that raised the event.</param>
        /// <param name="e">Arguments for the event.</param>
        /// <remarks>Does nothing by default.</remarks>
        public void OnEventTrigger(object? sender, EventArgs e);
    }
}
