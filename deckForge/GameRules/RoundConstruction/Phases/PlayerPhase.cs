using deckForge.PhaseActions;
using deckForge.PlayerConstruction;

namespace deckForge.GameRules.RoundConstruction.Phases
{
    public class PlayerPhase : BasePhase<IPlayer>
    {

        protected int CurrentPlayer = 0;
        protected List<IPlayer> Players;
        public PlayerPhase(List<IPlayer> players, string phaseName = "") : base(phaseName: phaseName)
        {
            Players = players;
        }

        new virtual public void StartPhase()
        {
            base.StartPhase();
            NextAction(actionNum: CurrentAction);
        }

        virtual protected void NextAction(int actionNum)
        {
            foreach (IPlayer player in Players)
            {
                NextActionHook(player, actionNum, out bool repeatAction);
                if (repeatAction)
                    Actions?[actionNum].execute(player);
            }
            CurrentAction++;
            if (CurrentAction < ActionCount - 1)
                NextAction(CurrentAction);
            else
                EndPhase();
        }

        new virtual public void EndPhase()
        {
            base.EndPhase();
        }

        virtual protected void NextActionHook(IPlayer p, int actionNum, out bool repeatAction) { repeatAction = false; }
    }
}