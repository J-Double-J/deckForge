using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PhaseActions.NonPlayerActions;

namespace DeckForge.GameConstruction.PresetGames.Poker.PokerPhases
{
    /// <summary>
    /// Clears the <see cref="PlayingCard"/>s from the <see cref="ITable"/>.
    /// </summary>
    public class CleanUpPhase : BasePhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleanUpPhase"/> class.
        /// </summary>
        /// <param name="pGM"><see cref="IGameMediator"/> to communicate with other game elements.</param>
        /// <param name="name">Name of the <see cref="CleanUpPhase"/>.</param>
        public CleanUpPhase(PokerGameMediator pGM, string name = "Clean Up Phase")
            : base(pGM, name)
        {
            Actions.Add(new MoveAllCardsFromTableToTableDeckAction(pGM, 0, false));
        }
    }
}
