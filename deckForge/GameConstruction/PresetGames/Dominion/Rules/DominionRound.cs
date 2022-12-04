using DeckForge.GameRules.RoundConstruction.Rounds;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Rules
{
    /// <summary>
    /// A round of Dominion that contains the <see cref="DominionPlayPhase"/>. Only ever one "round" will play.
    /// </summary>
    public class DominionRound : PlayerRoundRules
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DominionRound"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="players">List of <see cref="IPlayer"/> IDs.</param>
        public DominionRound(DominionGameMediator gm, List<int> players)
            : base(gm, players, -1, -1)
        {
            Phases.Add(new DominionPlayPhase(gm, players));
        }
    }
}
