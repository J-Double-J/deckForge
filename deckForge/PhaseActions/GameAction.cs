using deckForge.PlayerConstruction;

namespace deckForge.PhaseActions
{
    public abstract class GameAction
    {

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract void execute(Player p);
    }
}


