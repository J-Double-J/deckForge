using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PhaseActions.PlayerActions;

namespace DeckForge.GameConstruction.PresetGames.Poker.PokerPhases
{
    /// <summary>
    /// Reveals the remaining <see cref="PokerPlayer"/>'s hands.
    /// </summary>
    public class RevealCardsPhase : PlayerPhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RevealCardsPhase"/> class.
        /// </summary>
        /// <param name="pGM">GameMediator that is used to communicate with rest of game.</param>
        /// <param name="playerIDs">IDs of all the remaining <see cref="PokerPlayer"/>s in the round.</param>
        public RevealCardsPhase(PokerGameMediator pGM, List<int> playerIDs)
            : base(pGM, playerIDs, "Reveal and Evaluate Phase")
        {
            Actions.Add(new PlayHandToTable());
        }
    }
}
