using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameRules.RoundConstruction.Interfaces
{
    /// <summary>
    /// A Phase that controls <see cref="IPlayer"/>s.
    /// </summary>
    public interface IPlayerPhase
    {
        /// <summary>
        /// Starts the turn of one <see cref="IPlayer"/> and loops through the <see cref="IPlayerPhase"/>'s <see cref="IAction{T}"/>s.
        /// </summary>
        /// <param name="playerID">ID of the <see cref="IPlayer"/> of interest.</param>
        public void StartPhase(int playerID);

        /// <summary>
        /// Starts the turns of all the <see cref="IPlayer"/>s that it manages and lets each <see cref="IPlayer"/>
        /// execute an <see cref="IAction{T}"/> before going onto the next <see cref="IAction{T}"/>.
        /// </summary>
        /// <param name="playerIDs">IDs of the <see cref="IPlayer"/>s that are in the <see cref="IPlayerPhase"/>.</param>
        public void StartPhase(List<int> playerIDs);

        /// <summary>
        /// Ends the <see cref="IPlayerPhase"/> and executes any necessary cleanup.
        /// </summary>
        public void EndPhase();

        /// <summary>
        /// Ends an <see cref="IPlayer"/>'s turn prematurely.
        /// </summary>
        public void EndPlayerTurn();

        /// <summary>
        /// Updates the turn order of the <see cref="IPlayer"/>s in the <see cref="IPlayerPhase"/>.
        /// </summary>
        /// <param name="newTurnOrder">List of the <see cref="IPlayer"/> IDs that order the <see cref="IPlayer"/>'s turns.</param>
        public void UpdateTurnOrder(List<int> newTurnOrder);
    }
}
