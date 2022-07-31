using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameRules.RoundConstruction.Rounds;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace DeckForge.GameConstruction.PresetGames.War
{
    /// <summary>
    /// All the rules involving playing the game of <see cref="War"/>.
    /// </summary>
    public class WarRoundRules : PlayerRoundRules
    {
        private bool atWar = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="WarRoundRules"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that the <see cref="WarRoundRules"/> will use to
        /// communicate with other game elements.</param>
        /// <param name="players">IDs of all the playing <see cref="PlayerConstruction.IPlayer"/>s.</param>
        public WarRoundRules(IGameMediator gm, List<int> players)
            : base(gm, players: players)
        {
            Phases = new List<IPhase>
            {
                new WarPlayCardsPhase(gm, players, "Play Cards"),
                new WarComparePhase(gm, players, "Compare Cards"),
                new WarPhase(gm, players, "War!")
            };

            SubscribeToAllPhases_SkipToPhaseEvents();
            SubscribeToAllPhases_EndRoundEarlyEvents();
        }

        /// <summary>
        /// Function that is called when a <see cref="PlayerConstruction.IPlayer"/> raises a simple
        /// message. Checks if the <see cref="PlayerConstruction.IPlayer"/> is informing the rules that
        /// it lost the game.
        /// </summary>
        /// <param name="sender">Object (usually a <see cref="PlayerConstruction.IPlayer"/>) that sent the message.</param>
        /// <param name="args">The <see cref="SimplePlayerMessageEventArgs"/> that the <see cref="PlayerConstruction.IPlayer"/>
        /// passed.</param>
        public void PlayerRaisedEvent(object? sender, SimplePlayerMessageEventArgs args)
        {
            if (args.message == "LOSE_GAME")
            {
                EndRound();
            }
        }

        /// <inheritdoc/>
        protected override bool NextPhaseHook(int phaseNum)
        {
            bool handledPhase = false;

            if (phaseNum == 0)
            {
                atWar = false;

                // Reset WarPhase Counter
                var phase = (WarPhase)Phases[2];
                phase.ResetIteration();
            }
            else if (phaseNum == 1)
            {
                if (atWar)
                {
                    // Give ComparePhase the cards flipped from War
                    var warPhase = (WarPhase)Phases[2];
                    var comparePhase = (WarComparePhase)Phases[1];
                    comparePhase.FlippedCards = warPhase.GetFlippedCards();
                }
                else
                {
                    // Give ComparePhase the cards to compare
                    var phase = (WarPlayCardsPhase)Phases[0];
                    var compare = (WarComparePhase)Phases[1];
                    compare.FlippedCards = phase.GetFlippedCards();
                }

            }
            else if (phaseNum == 2)
            {
                // Update War Status, and track num of War iterations
                if (atWar == false)
                {
                    atWar = true;
                }
                else
                {
                    var phase = (WarPhase)Phases[2];
                    phase.IncreaseIteration();
                }
            }

            return handledPhase;
        }
    }
}
