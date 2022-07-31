namespace DeckForge.PhaseActions
{
    public interface IAction<T>
    {
        public string Name { get; }
        public string Description { get; }
        public Object? execute(T t);
        public Object? execute(T t, T t_target);
        public List<Object?> execute(T t, List<T> t_group_target);
    }
}