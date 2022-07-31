using DeckForge.PlayerConstruction;
using DeckForge.PhaseActions;

namespace DeckForge.PhaseActions
{
    public class DoNothingAction<T> : IAction<T>
    {
        public DoNothingAction() { }
        public string Name { get { return "Nothing"; } }

        public string Description
        {
            get { return "Do Nothing"; }
        }

        //Return non-sensical values
        virtual public Object execute(T t) { return -1; }

        virtual public Object execute (T t, T t_two) { return -1; }

        virtual public List<Object?> execute (T t, List<T> t_group) { return new List<Object?> {-1, -1}; }
    }
}
