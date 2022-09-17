using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PhaseActions;

namespace DeckForge.GameConstruction.PresetGames.Poker.PokerPhases
{
    /// <summary>
    /// Sets up the <see cref="GameElements.Table"/> for the next round of Poker.
    /// </summary>
    public class SetUpTablePokerPhase : BasePhase
    {
        public SetUpTablePokerPhase(PokerGameMediator gm)
            : base(gm, "Set up table for next round of Poker")
        {
            Actions.Add(new ShuffleDeckOnTable(gm, 0));
            Actions.Add(new DealCardsFromTableDeckToPlayers(gm, 0, 2));
            Actions.Add(new PokerPlayersBet(gm));
        }
    }
}
