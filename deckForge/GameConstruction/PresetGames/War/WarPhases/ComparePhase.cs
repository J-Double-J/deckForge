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
        public WarComparePhase(IGameMediator gm, List<int> playerIDs, string name)
        : base(gm, playerIDs, name)
        {
            Actions.Add(new TakeAllCards_FromTargetPlayerTable_ToPlayerDeck());
        }

        public override void StartPhase(List<int> playerIDs)
        {
            bool isWar;
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
                if (FlippedCards![0].val > FlippedCards[1].val)
                {
                    GM.TellPlayerToDoActionAgainstAnotherPlayer(0, 1, Actions[0]);
                    Console.WriteLine("Player 0 won this round");
                }
                else
                {
                    GM.TellPlayerToDoActionAgainstAnotherPlayer(1, 0, Actions[0]);
                    Console.WriteLine("Player 1 won this round");
                }
                
                OnSkipToPhase(new SkipToPhaseEventArgs(0));
            }
            else
            {
                //No extra steps, time to go to war!
                EndPhase();
            }
        }

        public override void EndPhase()
        {
            FlippedCards = null;
        }
    }
}
