using DeckForge.GameConstruction;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameRules.RoundConstruction.Phases
{
    /// <summary>
    /// A <see cref="PlayerPhase"/> where nothing happens.
    /// </summary>
    public class DoNothingPhase : PlayerPhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoNothingPhase"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that is used for <see cref="DoNothingPhase"/> to
        /// communicate with all other game objects.</param>
        /// <param name="playerIDs">List of the all the <see cref="IPlayer"/>s by their IDs that will do nothing.</param>
        public DoNothingPhase(IGameMediator gm, List<int> playerIDs)
            : base(gm, playerIDs)
        {
            Actions.Add(new DoNothingAction<IPlayer>());
        }
    }
}
