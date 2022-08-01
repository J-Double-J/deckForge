namespace DeckForge.GameConstruction
{
    /// <summary>
    /// Handles the order of <see cref="PlayerConstruction.IPlayer"/> turns and manipulating the order.
    /// </summary>
    public interface ITurnHandler
    {
        /// <summary>
        /// Gets the current turn order.
        /// </summary>
        public List<int> TurnOrder { get; }

        /// <summary>
        /// Updates the list of <see cref="PlayerConstruction.IPlayer"/>s in the game
        /// by their IDs.
        /// </summary>
        /// <param name="newPlayerList">List of the <see cref="PlayerConstruction.IPlayer"/> IDs.</param>
        public void UpdatePlayerList(List<int> newPlayerList);

        /// <summary>
        /// Shifts the turn order clockwise.
        /// </summary>
        public void ShiftTurnOrderClockwise();

        /// <summary>
        /// Shifts the turn order counter-clockwise.
        /// </summary>
        public void ShiftTurnOrderCounterClockwise();
    }
}
