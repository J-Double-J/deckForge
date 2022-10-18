using DeckForge.GameConstruction.PresetGames.Poker.PokerPhases;
using DeckForge.GameRules.RoundConstruction.Rounds;

// TODO: A problem is clearly presented here where some phases are awkwardly
// blocked into new "rounds" when its not really appropriate to segment it as thus.
// There should be a way for a round agnostic ruleset that handles differences in the two
// different constructors and their method requirements.
namespace DeckForge.GameConstruction.PresetGames.Poker
{
    /// <summary>
    /// Round rules for Poker.
    /// </summary>
    public class PokerRoundRules : BaseRoundRules
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PokerRoundRules"/> class.
        /// </summary>
        /// <param name="pGM"><see cref="IGameMediator"/> used to communicate with other elements.</param>
        public PokerRoundRules(PokerGameMediator pGM)
            : base(pGM)
        {
            Phases.Add(new SetUpTablePokerPhase(pGM));
            Phases.Add(new FlopPhase(pGM));
            Phases.Add(new FlipAdditionalCardPhase(pGM));
            Phases.Add(new FlipAdditionalCardPhase(pGM));
        }
    }

    /// <summary>
    /// Round rules for revealing player cards.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too small of class")]
    public class PokerRevealRound : PlayerRoundRules
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PokerRevealRound"/> class.
        /// </summary>
        /// <param name="pGM"><see cref="IGameMediator"/> used to communicate with other elements.</param>
        /// <param name="playerIDs">IDs of all the active <see cref="IPlayer"/>s.</param>
        public PokerRevealRound(PokerGameMediator pGM, List<int> playerIDs)
            : base(pGM, playerIDs)
        {
            Phases.Add(new RevealCardsPhase(pGM, playerIDs));
        }
    }


    /// <summary>
    /// Poker Rules for evaluating cards and then cleaning up afterwards.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Too small of class")]
    public class PokerEvaluateAndCleanupRoundRules : BaseRoundRules
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PokerEvaluateAndCleanupRoundRules"/> class.
        /// </summary>
        /// <param name="pGM"><see cref="IGameMediator"/> used to communicate with other elements.</param>
        public PokerEvaluateAndCleanupRoundRules(PokerGameMediator pGM)
            : base(pGM)
        {
            Phases.Add(new EvaluateCardsPhase(pGM));
            Phases.Add(new CleanUpPhase(pGM));
        }
    }
}
