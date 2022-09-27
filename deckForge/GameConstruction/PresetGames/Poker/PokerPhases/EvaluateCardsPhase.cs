using DeckForge.GameRules.RoundConstruction.Phases;

namespace DeckForge.GameConstruction.PresetGames.Poker.PokerPhases
{
    /// <summary>
    /// Evaluates the <see cref="PlayingCard"/>s and awards a winner.
    /// </summary>
    public class EvaluateCardsPhase : BasePhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateCardsPhase"/> class.
        /// </summary>
        /// <param name="pGM">Mediator that will be used to communicate with other game elements.</param>
        public EvaluateCardsPhase(PokerGameMediator pGM)
            : base(pGM, "Evaluate Phase")
        {
            Actions.Add(new EvaluateRoundWinnerAction(pGM));
        }
    }
}
