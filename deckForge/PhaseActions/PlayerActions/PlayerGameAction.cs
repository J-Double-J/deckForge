using DeckForge.GameElements.Resources;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions
{
    /// <summary>
    /// <see cref="IGameAction{T}"/> that target and interact with <see cref="IPlayer"/>s.
    /// </summary>
    public class PlayerGameAction : IGameAction<IPlayer>
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerGameAction"/> class.
        /// </summary>
        /// <param name="name">Name of the <see cref="PlayerGameAction"/>.</param>
        /// <param name="description">Description of the <see cref="PlayerGameAction"/>.</param>
        public PlayerGameAction(string name = "Player Action", string description = "This is a Player Action")
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="PlayerGameAction"/>.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="PLayerGameAction"/>.
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// Executes the <see cref="IGameAction{IPlayer}"/> on <paramref name="player"/>.
        /// </summary>
        /// <remarks>
        /// Can throw <see cref="NotSupportedException"/> if this <see cref="PlayerGameAction"/> function is not overriden
        /// in a derived class.
        /// </remarks>
        /// <param name="player">Object that will be executing the action.</param>
        /// <returns>A nullable object that references what the <see cref="IGameAction{IPlayer}"/>
        /// may have interacted with.</returns>
        public virtual object? Execute(IPlayer player)
        {
            throw new NotSupportedException("This implentation '(IPlayer player)' is not defined for this action.");
        }

        /// <summary>
        /// Executes the <see cref="IGameAction{IPlayer}"/> where
        /// <paramref name="playerExecutor"/> targets the <see cref="IGameAction{IPlayer}"/>
        /// against <paramref name="playerTarget"/>.
        /// </summary>
        /// <remarks>
        /// Can throw <see cref="NotSupportedException"/> if this <see cref="PlayerGameAction"/> function is not overriden
        /// in a derived class.
        /// </remarks>
        /// <param name="playerExecutor"><see cref="IPlayer"/> executing the <see cref="IGameAction{IPlayer}"/>.</param>
        /// <param name="playerTarget"><see cref="IPlayer"/> being targetted by the <see cref="IGameAction{IPlayer}"/>.</param>
        /// <returns>A nullable object that references what the <see cref="IGameAction{IPlayer}"/>
        /// may have interacted with.</returns>
        public virtual object? Execute(IPlayer playerExecutor, IPlayer playerTarget)
        {
            throw new NotSupportedException("This implentation '(IPlayer playerExecutor, IPlayer playerTarget)' is not defined for this action.");
        }

        /// <summary>
        /// Executes the <see cref="IGameAction{IPlayer}"/> where
        /// <paramref name="playerExecutor"/> targets the <see cref="IGameAction{IPlayer}"/>
        /// against all objects in <paramref name="playerGroup"/>.
        /// </summary>
        /// <param name="playerExecutor"><see cref="IPlayer"/> executing the <see cref="IGameAction{IPlayer}"/>.</param>
        /// <param name="playerGroup">List of <see cref="IPlayer"/>s being targetted by the <see cref="IGameAction{IPlayer}"/>.</param>
        /// <returns>A list of nullable objects that references what the <see cref="IGameAction{IPlayer}"/>
        /// may have interacted with. </returns>
        public virtual List<object?> Execute(IPlayer playerExecutor, List<IPlayer> playerGroup)
        {
            List<object?> objects = new ();
            foreach (IPlayer player in playerGroup)
            {
                objects.Add(Execute(playerExecutor, player));
            }

            return objects;
        }
    }
}
