using DeckForge.GameElements.Resources;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.War
{
    /// <summary>
    /// First <see cref="PlayerPhase"/> of <see cref="War"/>. <see cref="IPlayer"/>s will
    /// draw a <see cref="PlayingCard"/> and then flip it.
    /// </summary>
    public class WarPlayCardsPhase : PlayerPhase
    {
        private List<PlayingCard?> flippedCards = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="WarPlayCardsPhase"/> class.
        /// </summary>
        /// <param name="gm">IGameMediator that the <see cref="WarPlayCardsPhase"/> will use to communicate
        /// with other game elements.</param>
        /// <param name="playerIDs">IDs of the <see cref="IPlayer"/>s playing.</param>
        /// <param name="phaseName">Name of the <see cref="WarPlayCardsPhase"/>.</param>
        public WarPlayCardsPhase(IGameMediator gm, List<int> playerIDs, string phaseName)
        : base(gm, playerIDs, phaseName)
        {
            Actions.Add(new PlayCardAction());
            Actions.Add(new FlipOneCard_OneWayAction(gm, 0, facedown: false));
        }

        /// <summary>
        /// Gets the flipped cards during <see cref="WarPlayCardsPhase"/>.
        /// </summary>
        /// <returns>List of <see cref="PlayingCard"/>s that were flipped.</returns>
        /// <exception cref="NullReferenceException">Throws exception if a <see cref="PlayingCard"/> is nonexistant.</exception>
        public List<PlayingCard> GetFlippedCards()
        {
            foreach (PlayingCard? c in flippedCards)
            {
                if (c is null)
                {
                    throw new NullReferenceException("The flipped card was actually null!");
                }
            }

            return flippedCards!;
        }

        /// <inheritdoc/>
        public override void StartPhase()
        {
            flippedCards = new();
            base.StartPhase();
        }

        /// <inheritdoc/>
        public override void StartPhase(List<int> playerIDs)
        {
            flippedCards = new();
            base.StartPhase(playerIDs);
        }

        /// <inheritdoc/>
        protected override bool PhaseActionLogic(int playerID, int actionNum)
        {
            bool handledAction = false;

            try
            {
                if (actionNum == 0)
                {
                    handledAction = true;
                    PlayingCard? drawnCard = (PlayingCard?)GM.TellPlayerToDoAction(playerID, Actions[actionNum]);

                    if (drawnCard is not null)
                    {
                        flippedCards.Add(drawnCard);
                    }
                    else
                    {
                        OnEndRoundEarly(new EndRoundEarlyArgs());
                    }
                }

                return handledAction;
            }
            catch
            {
                throw;
            }
        }
    }
}
