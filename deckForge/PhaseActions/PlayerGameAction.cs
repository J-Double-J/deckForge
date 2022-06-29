using deckForge.PlayerConstruction;
using deckForge.GameRules.RoundConstruction.Phases;

namespace deckForge.PhaseActions
{
    public abstract class PlayerGameAction : IAction<Player>
    {

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract void execute(Player p);
    }
}


