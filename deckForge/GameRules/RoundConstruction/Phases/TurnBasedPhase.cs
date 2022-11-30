using DeckForge.GameConstruction;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameRules.RoundConstruction.Phases
{
    // TODO: I don't know if we should inherit from PlayerPhase

    /// <summary>
    /// A <see cref="PlayerPhase"/> where <see cref="IPlayer"/>s go through potentially numerous <see cref="IGameAction{T}"/>
    /// they can execute before the next <see cref="IPlayer"/>
    /// gets a turn.
    /// </summary>
    /// <remarks><see cref="PlayerPhase"/> has a different method of executing a series of actions amongst its <see cref="IPlayer"/>s.</remarks>
    public class TurnBasedPhase : PlayerPhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TurnBasedPhase"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="playerIDs">IDs of the all the <see cref="IPlayer"/>s involved with this <see cref="TurnBasedPhase"/>.</param>
        /// <param name="phaseName">Name of the <see cref="TurnBasedPhase"/>.</param>
        public TurnBasedPhase(IGameMediator gm, List<int> playerIDs, string phaseName = "")
            : base(gm, playerIDs, phaseName)
        {
        }

        /// <inheritdoc/>
        public override void StartPhase()
        {
            foreach (var playerID in PlayerIDs)
            {
                CurrentPlayerTurn = playerID;
                GM.InformPlayerToStartTurn(playerID);
                InterPlayerTurnExtraLogic();
            }
        }

        /// <summary>
        /// Any extra logic or rules that need to be evaluated between each <see cref="IPlayer"/>'s turn should be
        /// implemented in a overriden call of this function.
        /// </summary>
        protected virtual void InterPlayerTurnExtraLogic()
        {
        }
    }
}
