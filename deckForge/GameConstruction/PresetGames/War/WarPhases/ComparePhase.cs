using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.PlayerConstruction;
using deckForge.PhaseActions;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarComparePhase : PlayerPhase
    {
        public List<Card>? FlippedCards { get; set; }
        public WarComparePhase(List<IPlayer> players, string name)
        : base(players, name)
        {
            Actions.Add(new TakeAllCards_FromTargetPlayerTable_ToPlayerDeck());
        }

        public override void StartPhase()
        {
            bool isWar = false;

            if (FlippedCards != null)
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
                throw new ArgumentNullException("Cards to compare were not set");
            }

            DecideIfGoToWarPhase(isWar);
        }

        private void DecideIfGoToWarPhase(bool isWar)
        {
            if (!isWar)
            {
                OnSkipToPhase(new SkipToPhaseEventArgs(0));
            }
            else
            {
                //Action here is TakeCardsFromPlayerOnTable
                if (FlippedCards![0].val > FlippedCards[1].val)
                {
                    Actions[0].execute(Players[0], Players[1]);
                }
                else
                {
                    Actions[0].execute(Players[1], Players[0]);
                }
                EndPhase();
            }
        }

        public override void EndPhase()
        {
            FlippedCards = null;
        }
    }
}