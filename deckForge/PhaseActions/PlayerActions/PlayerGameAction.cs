using DeckForge.PlayerConstruction;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.GameElements.Resources;
namespace DeckForge.PhaseActions
{
    public class PlayerGameAction : IAction<IPlayer>
    {

        public PlayerGameAction(string name = "Player Action", string description = "This is a Player Action")
        {
            Name = name;
            Description = description;
        }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual Object? execute(IPlayer player) { throw new NotSupportedException("This implentation '(IPlayer player)' is not defined for this action."); }
        public virtual Object? execute(IPlayer playerExecutor, IPlayer playerTarget) { throw new NotSupportedException("This implentation '(IPlayer playerExecutor, IPlayer playerTarget)' is not defined for this action."); }
        public virtual List<Object?> execute(IPlayer player, List<IPlayer> playerGroup)
        {
            List<Object?> objects = new();
            foreach (IPlayer p in playerGroup)
            {
                objects.Add(execute(player, p));
            }

            return objects;
        }
    }
}
