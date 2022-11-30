using DeckForge.PlayerConstruction;

namespace DeckForge.PhaseActions.PlayerActions
{
    /// <summary>
    /// Ends the <see cref="IPlayer"/>'s turn.
    /// </summary>
    public class EndTurnAction : PlayerGameAction
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="EndTurnAction"/> class.
        /// </summary>
        /// <param name="name">Name of the <see cref="PlayerGameAction"/>.</param>
        /// <param name="description">Description of the <see cref="PlayerGameAction"/>.</param>
        public EndTurnAction(string name = "End Turn", string description = "Ends the Turn")
            : base(name, description)
        {
        }

        /// <inheritdoc/>
        public override object? Execute(IPlayer player)
        {
            player.EndTurn();
            return null;
        }
    }
}
