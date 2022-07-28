using deckForge.GameConstruction;
using deckForge.PhaseActions;
using deckForge.PlayerConstruction;

namespace deckForge.GameRules.RoundConstruction.Phases
{
    public class DoNothingPlayerPhase : PlayerPhase
    {
        public DoNothingPlayerPhase(IGameMediator gm, List<int> playerIDs) : base(gm, playerIDs)
        {
            Actions.Add(new DoNothingAction<IPlayer>());
        }
    }

    public class DoNothingGenericPhase<T> : BasePhase<T>
    {
        public DoNothingGenericPhase(IGameMediator gm) : base(gm, "Do Nothing")
        {
            Actions.Add(new DoNothingAction<T>());
        }
    }
}
