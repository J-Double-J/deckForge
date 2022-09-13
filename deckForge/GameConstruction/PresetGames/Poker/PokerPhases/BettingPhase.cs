using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PhaseActions;

namespace DeckForge.GameConstruction.PresetGames.Poker.PokerPhases
{
    public class BettingPhase : PlayerPhase
    {
        public BettingPhase(PokerGameMediator pGM, List<int> playerIDs)
        : base(pGM, playerIDs, "Betting Phase")
        {
            Actions.Add(new DoPreFlopAction());
        }
    }
}