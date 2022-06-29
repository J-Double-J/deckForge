using deckForge.PlayerConstruction;
using deckForge.PhaseActions;

namespace deckForge.PhaseActions
{
    public class DoNothingAction<T> : IAction<T>
    {
        public DoNothingAction() { }
        public string Name { get { return "Do Nothing"; } }

        public string Description
        {
            get { return "Do Nothing"; }
        }

        public void execute(T t) { }
    }
}
