using deckForge.PhaseActions;
using deckForge.PlayerConstruction;

namespace deckForge.GameRules.RoundConstruction.Phases
{
    public class PlayerPhase : BasePhase<Player>
    {

        protected int CurrentPlayer = 0;
        protected List<Player> Players;
        public PlayerPhase(List<Player> players, List<IAction<Player>> actions, string phaseName = "") : base(actions: actions, phaseName: phaseName)
        {
            Players = players;
        }

        new virtual public void StartPhase()
        {
            base.StartPhase();
            NextAction(actionNum: CurrentAction);

        }
        new virtual public void EndPhase()
        {
            base.EndPhase();
        }

        protected void NextAction(int actionNum)
        {
            foreach (Player p in Players)
            {
                Actions[actionNum].execute(p);
            }
            CurrentAction++;
            NextAction(CurrentAction);
        }
    }
}