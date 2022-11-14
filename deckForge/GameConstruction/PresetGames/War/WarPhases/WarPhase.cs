using DeckForge.GameElements.Resources;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.War
{
    /// <summary>
    /// This is the War phase for <see cref="War"/>.
    /// <see cref="IPlayer"/>s will draw cards and flip one of them before going back to <see cref="WarComparePhase"/>.
    /// </summary>
    public class WarPhase : PlayerPhase
    {
        private int iteration = 1;
        private List<PlayingCard?> flippedCards = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="WarPhase"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that <see cref="WarPhase"/> will use to communicate with other
        /// game elements.</param>
        /// <param name="playerIDs">IDs of the <see cref="IPlayer"/>s involved in War.</param>
        /// <param name="phaseName">Name of the <see cref="WarPhase"/>.</param>
        public WarPhase(IGameMediator gm, List<int> playerIDs, string phaseName)
        : base(gm, playerIDs, phaseName)
        {
            Actions.Add(new PlayMultipleCardsAction(playCount: 2));
            Actions.Add(new FlipOneCard_OneWayAction(gm, 2 * iteration));
        }

        /// <inheritdoc/>
        public override void EndPhase()
        {
            if (CurrentAction >= 0)
            {
                OnSkipToPhase(new SkipToPhaseEventArgs(1));
            }
        }

        /// <inheritdoc/>
        public override void StartPhase(List<int> playerIDs)
        {
            flippedCards = new();
            base.StartPhase(playerIDs);
        }

        /// <summary>
        /// Gets the flipped cards up cards in the War phase.
        /// </summary>
        /// <returns>List of <see cref="PlayingCard"/>s that were flipped.</returns>
        /// <exception cref="NullReferenceException">Throws exception if flipped card was nonexistant.</exception>
        public List<PlayingCard> GetFlippedCards()
        {
            foreach (PlayingCard? c in flippedCards)
            {
                if (c is null)
                {
                    throw new NullReferenceException("Flipped card was actually null!");
                }
            }

            return flippedCards!;
        }

        /// <summary>
        /// Increases the iteration of the War phase which lets <see cref="WarPhase"/> know which
        /// card to flip up.
        /// </summary>
        public void IncreaseIteration()
        {
            iteration++;
        }

        /// <summary>
        /// Resets the iteration of the War phase. This is called whenever <see cref="WarPhase"/> is over.
        /// </summary>
        public void ResetIteration()
        {
            iteration = 1;
        }

        /// <inheritdoc/>
        protected override bool PhaseActionLogic(int playerID, int actionNum)
        {
            bool handledAction = false;

            if (actionNum == 1)
            {
                handledAction = true;
                flippedCards.Add((PlayingCard?)GM.TellPlayerToDoAction(playerID, Actions[actionNum]));
                foreach (PlayingCard? card in flippedCards)
                {
                    if (card is null)
                    {
                        OnEndRoundEarly(new EndRoundEarlyArgs());
                        break;
                    }
                }
            }

            return handledAction;
        }
    }
}