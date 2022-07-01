using deckForge.PlayerConstruction;
using deckForge.GameRules.RoundConstruction.Phases;
using CardNamespace;

namespace deckForge.PhaseActions
{
    public class PlayerGameAction : IAction<Player>
    {

        public PlayerGameAction(string name = "Player Action", string description = "This is a Player Action")
        {
            Name = name;
            Description = description;
        }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual Object? execute(Player player) { throw new NotSupportedException("This implentation '(Player player)' is not defined for this action."); }
        public virtual Object? execute(Player playerExecutor, Player playerTarget) { throw new NotSupportedException("This implentation '(Player playerExecutor, Player playerTarget)' is not defined for this action."); }
        public virtual List<Object?> execute(Player player, List<Player> playerGroup) {
            List<Object?> objects = new(); 
            foreach(Player p in playerGroup) {
                objects.Add(execute(player, p));
            }

            return objects;
        }
    }
}


