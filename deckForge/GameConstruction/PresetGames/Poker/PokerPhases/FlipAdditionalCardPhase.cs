using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PhaseActions;

namespace DeckForge.GameConstruction.PresetGames.Poker.PokerPhases
{
    /// <summary>
    /// Flips a <see cref="PlayingCard"/> and then starts another round of betting.
    /// </summary>
    public class FlipAdditionalCardPhase : BasePhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlipAdditionalCardPhase"/> class.
        /// </summary>
        /// <param name="pGM">GameMediator to communicate with rest of game.</param>
        public FlipAdditionalCardPhase(PokerGameMediator pGM)
        : base(pGM, "Flip Another Card Phase")
        {
            Actions.Add(new DealCardsFromTableDeckToTable(pGM, 0, 1, 0, true));
            Actions.Add(new PokerPlayersBet(pGM));
        }
    }
}
