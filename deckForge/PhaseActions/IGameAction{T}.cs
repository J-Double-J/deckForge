namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Actions that objects can be commanded to take on themselves or against other
    /// similar objects.
    /// </summary>
    /// <remarks>
    /// Not to be confused with <see cref="IGameAction"/> which executes a general
    /// action on the game that doesn't target specific objects.
    /// </remarks>
    /// <typeparam name="T">Type of objects that can be interacted with by this
    /// <see cref="IGameAction{T}"/>.</typeparam>
    public interface IGameAction<T>
    {
        /// <summary>
        /// Gets the name of the <see cref="IGameAction{T}"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a description of the <see cref="IGameAction{T}"/>.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Executes the <see cref="IGameAction{T}"/> on <paramref name="t"/>.
        /// </summary>
        /// <param name="t">Object that will be executing the action.</param>
        /// <returns>A nullable object that references what the <see cref="IGameAction{T}"/>
        /// may have interacted with.</returns>
        public object? Execute(T t);

        /// <summary>
        /// Executes the <see cref="IGameAction{T}"/> where
        /// <paramref name="t"/> targets the <see cref="IGameAction{T}"/>
        /// against <paramref name="t_target"/>.
        /// </summary>
        /// <param name="t">Object executing the <see cref="IGameAction{T}"/>.</param>
        /// <param name="t_target">Object being targetted by the <see cref="IGameAction{T}"/>.</param>
        /// <returns>A nullable object that references what the <see cref="IGameAction{T}"/>
        /// may have interacted with.</returns>
        public object? Execute(T t, T t_target);

        /// <summary>
        /// Executes the <see cref="IGameAction{T}"/> where
        /// <paramref name="t"/> targets the <see cref="IGameAction{T}"/>
        /// against all objects in <paramref name="t_group_target"/>.
        /// </summary>
        /// <param name="t">Object executing the <see cref="IGameAction{T}"/>.</param>
        /// <param name="t_group_target">List of objects being targetted by the <see cref="IGameAction{T}"/>.</param>
        /// <returns>A list of nullable objects that references what the <see cref="IGameAction{T}"/>
        /// may have interacted with. </returns>
        public List<object?> Execute(T t, List<T> t_group_target);
    }
}