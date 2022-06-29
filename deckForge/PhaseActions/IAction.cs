namespace deckForge.PhaseActions
{
    public interface IAction<T>
    {
        public string Name { get; }
        public string Description { get; }
        public void execute(T t);
    }
}