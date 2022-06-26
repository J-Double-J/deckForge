using deckForge.PhaseActions;

namespace deckForge.GameRules.PlayerRoundRules
{
    public class Phase
    {
        public string PhaseName
        {
            get;
        }

        public List<GameAction> Actions {
            get;
            private set;
        }

        public Phase(List<GameAction>? actions = null, string phaseName = "") {
            if (actions == null) {
                actions = new List<GameAction>();
                actions.Add(new DoNothingAction());
            }
            Actions = actions;
            PhaseName = phaseName;
        }
    }
}
