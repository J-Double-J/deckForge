using DeckForge.PlayerConstruction;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.GameElements.Resources;
namespace DeckForge.PhaseActions
{
    public class PlayerGameAction : IGameAction<IPlayer>
    {

        public PlayerGameAction(string name = "Player Action", string description = "This is a Player Action")
        {
            Name = name;
            Description = description;
        }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual Object? Execute(IPlayer player) { throw new NotSupportedException("This implentation '(IPlayer player)' is not defined for this action."); }
        public virtual Object? Execute(IPlayer playerExecutor, IPlayer playerTarget) { throw new NotSupportedException("This implentation '(IPlayer playerExecutor, IPlayer playerTarget)' is not defined for this action."); }
        public virtual List<Object?> Execute(IPlayer player, List<IPlayer> playerGroup)
        {
            List<Object?> objects = new();
            foreach (IPlayer p in playerGroup)
            {
                objects.Add(Execute(player, p));
            }

            return objects;
        }
    }
}
