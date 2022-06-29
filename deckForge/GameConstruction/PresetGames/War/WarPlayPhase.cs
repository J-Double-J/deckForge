using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.PlayerConstruction;
using deckForge.PhaseActions;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarPlayPhase : PlayerPhase
    {

        List<PlayerGameAction> steps = new List<PlayerGameAction>{
            new DrawCardsAction()
        };
        public WarPlayPhase(Player p, List<IAction<Player>> actions, string name) : base(p, actions, name) { }
        public override void StartPhase()
        {

        }

    }
}