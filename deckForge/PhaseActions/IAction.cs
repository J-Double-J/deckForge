namespace deckForge.PhaseActions
{
    public interface IAction<T>
    {
        public string Name { get; }
        public string Description { get; }
        public void execute(T t);
        public void execute(T t, T t_two);
        public void execute(T t, List<T> t_group);
    }
}