using DeckForge.GameElements.Resources;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.War
{
    /// <summary>
    /// Second phase of War where <see cref="Card"/>s are compared against one another.
    /// <see cref="WarComparePhase"/> then decides if it should end the Round or go to <see cref="WarPhase"/>.
    /// </summary>
    public class WarComparePhase : PlayerPhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WarComparePhase"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that <see cref="WarComparePhase"/> will use to communicate
        /// with other game elemetns.</param>
        /// <param name="playerIDs">IDs of the <see cref="IPlayer"/>s playing.</param>
        /// <param name="phaseName">Name of the <see cref="WarComparePhase"/>.</param>
        public WarComparePhase(IGameMediator gm, List<int> playerIDs, string phaseName)
        : base(gm, playerIDs, phaseName)
        {
            FlippedCards = new List<Card>();
            Actions.Add(new TakeAllCards_FromTargetPlayerTable_ToPlayerDeckAction());
            Actions.Add(new PickUpOwnCardsFromTableAction());
        }

        /// <summary>
        /// Gets or sets the flipped <see cref="Card"/>s that the <see cref="WarComparePhase"/> will compare.
        /// </summary>
        public List<Card> FlippedCards { get; set; }

        /// <inheritdoc/>
        public override void StartPhase()
        {
            bool isWar;
            if (FlippedCards.Count > 0)
            {
                if (FlippedCards[0].val != FlippedCards[1].val)
                {
                    isWar = false;
                }
                else
                {
                    isWar = true;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(paramName: "FlippedCards", "Cards to compare were not set");
            }

            DecideIfGoToWarPhase(isWar);
        }

        /// <inheritdoc/>
        public override void EndPhase()
        {
            FlippedCards = new ();
        }

        /// <summary>
        /// Decides if the game needs to go to <see cref="WarPhase"/> or not. Gives <see cref="Card"/>s to
        /// winning <see cref="IPlayer"/>.
        /// </summary>
        /// <param name="isWar">If <c>true</c> game will go to <see cref="WarPhase"/>
        /// else it will give <see cref="Card"/>s to winning <see cref="IPlayer"/>.</param>
        private void DecideIfGoToWarPhase(bool isWar)
        {
            if (!isWar)
            {
                if (FlippedCards![0].val > FlippedCards[1].val)
                {
                    GM.TellPlayerToDoActionAgainstAnotherPlayer(0, 1, Actions[0]);
                    GM.TellPlayerToDoAction(0, Actions[1]);
                    Console.WriteLine("Player 0 won this round");
                }
                else
                {
                    GM.TellPlayerToDoActionAgainstAnotherPlayer(1, 0, Actions[0]);
                    GM.TellPlayerToDoAction(1, Actions[1]);
                    Console.WriteLine("Player 1 won this round");
                }

                OnEndRoundEarly(new EndRoundEarlyArgs("War! has ended."));
            }
            else
            {
                EndPhase();
            }
        }
    }
}
