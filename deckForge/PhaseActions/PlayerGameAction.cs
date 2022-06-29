using deckForge.PlayerConstruction;
using deckForge.GameRules.RoundConstruction.Phases;

namespace deckForge.PhaseActions
{
    public abstract class PlayerGameAction : IAction<Player>
    {

        public PlayerGameAction(string name = "Player Action", string description = "This is a Player Action")
        {
            Name = name;
            Description = description;
        }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public abstract void execute(Player player);
    }
}


