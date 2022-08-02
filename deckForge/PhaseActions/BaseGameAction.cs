namespace DeckForge.PhaseActions 
{ 
    public abstract class BaseGameAction : IGameAction
    {
        public BaseGameAction(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        public abstract object? Execute();
    }
}