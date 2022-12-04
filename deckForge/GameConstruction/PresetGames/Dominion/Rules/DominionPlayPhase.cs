using DeckForge.GameRules.RoundConstruction.Phases;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Rules
{
    /// <summary>
    /// Phase where players continually take their turns until the game end trigger condition.
    /// </summary>
    public class DominionPlayPhase : TurnBasedPhase
    {
        private bool phaseOver = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="DominionPlayPhase"/> class.
        /// </summary>
        /// <param name="gm"><see cref="DominionGameMediator"/> used to communicate with other game elements in Dominion.</param>
        /// <param name="playerIDs">List of <see cref="IPlayer"/>'s IDs.</param>
        public DominionPlayPhase(DominionGameMediator gm, List<int> playerIDs)
            : base(gm, playerIDs, "Player Turn")
        {
        }

        /// <inheritdoc/>
        public override void StartPhase()
        {
            while (!phaseOver)
            {
                base.StartPhase();
            }
        }

        /// <inheritdoc/>
        protected override void InterPlayerTurnExtraLogic()
        {
            if (((DominionGameMediator)GM).Market.IsGameOver)
            {
                phaseOver = true;
                EndPhase();
            }
        }
    }
}
