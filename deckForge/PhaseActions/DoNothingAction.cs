namespace DeckForge.PhaseActions
{
    /// <summary>
    /// Do Nothing action that works with any object.
    /// </summary>
    /// <typeparam name="T">Takes any object.</typeparam>
    public class DoNothingAction<T> : IGameAction<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoNothingAction{T}"/> class.
        /// </summary>
        public DoNothingAction()
        {
        }

        /// <inheritdoc/>
        public string Name
        {
            get
            {
                return "Do Nothing";
            }
        }

        /// <inheritdoc/>
        public string Description
        {
            get
            {
                return "Do Nothing";
            }
        }

        /// <inheritdoc/>
        public virtual object? Execute(T t)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual object? Execute(T t, T t_two)
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual List<object?> Execute(T t, List<T> t_group)
        {
            return new List<object?> { null };
        }
    }
}
