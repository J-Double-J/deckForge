using deckForge.PhaseActions;
using deckForge.PlayerConstruction;

namespace deckForge.GameRules.RoundConstruction.Phases
{
    public class PlayerPhase : BasePhase<Player>
    {

        Player _player;
        public PlayerPhase(Player player, List<IAction<Player>> actions, string phaseName = "") : base(actions: actions, phaseName: phaseName)
        {
            _player = player;
        }

        new virtual public void StartPhase()
        {
            base.StartPhase();
            NextAction(actionNum: _curAction);
        }
        new virtual public void EndPhase()
        {
            base.EndPhase();
        }

        private void NextAction(int actionNum)
        {
            Actions[actionNum].execute(_player);
        }
    }
}