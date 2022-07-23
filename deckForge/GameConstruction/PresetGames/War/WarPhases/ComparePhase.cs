using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.PlayerConstruction;
using deckForge.PhaseActions;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarComparePhase : PlayerPhase
    {
        public List<Card> FlippedCards { get; set; }
        public WarComparePhase(IGameMediator gm, List<int> playerIDs, string name)
        : base(gm, playerIDs, name)
        {
            FlippedCards = new List<Card>();
            Actions.Add(new TakeAllCards_FromTargetPlayerTable_ToPlayerDeck());
            Actions.Add(new PickUpOwnCardsFromTable());
        }

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
            FlippedCards = new();
        }
    }
}
