namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Actions that can be executed to take effect on the game.
    /// </summary>
    /// <remarks>
    /// Not to be confused with <see cref="IGameAction{T}"/> which
    /// executes commands on specific objects.
    /// </remarks>
    public interface IGameAction
    {
        /// <summary>
        /// Gets the name of the <see cref="IGameAction"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a description of the <see cref="IGameAction"/>.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Executes the <see cref="IGameAction"/>.
        /// </summary>
        /// <returns>A nullable object that references what the <see cref="IGameAction"/>
        /// may have interacted with. If object could reveal private
        /// object information about another object (such as cards in a player's hand) return null.</returns>
        public object? Execute();
    }
}