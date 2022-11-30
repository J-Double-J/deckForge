using DeckForge.PhaseActions;

namespace DeckForge.PlayerConstruction
{
    /// <summary>
    /// A player that has a dictionary of <see cref="IGameAction{T}"/>s that they may execute in whatever order they may like.
    /// </summary>
    public interface IPlayerWithActionChoices
    {
        // TODO: Decide the proper openness of this Dictionary's set.

        /// <summary>
        /// Gets or sets a dictionary of the <see cref="IGameAction{T}"/>s that the <see cref="IPlayer"/> can choose from and execute.
        /// Keys are action's names.
        /// </summary>
        Dictionary<string, (IGameAction<IPlayer> Action, int ActionCount)> Actions { get; set; }
    }
}