using deckForge.GameRules.PlayerRoundRules;
using deckForge.PhaseActions;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarPlayPhase : Phase
    {

        List<GameAction> steps = new List<GameAction>{
            new DrawCardsAction()
        };
        public WarPlayPhase() { }
        public override void StartPhase()
        {
            
        }

    }
}