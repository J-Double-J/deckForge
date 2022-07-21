using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.PlayerConstruction;
using deckForge.PhaseActions;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarPhase : PlayerPhase
    {
        int iteration = 1;
        List<Card?> FlippedCards = new();
        public WarPhase(IGameMediator gm, string name)
        : base(gm, name)
        {
            Actions.Add(new PlayCardsAction(playCount: 2));
            Actions.Add(new FlipOneCard_OneWay_Action(2 * iteration));
        }

        protected override void PhaseActionLogic(int playerID, int actionNum, out bool repeatAction)
        {
            repeatAction = false;
            if (actionNum == 1)
            {
                FlippedCards.Add((Card?)GM.TellPlayerToDoAction(playerID, Actions[actionNum]));
            }
        }

        public override void EndPhase()
        {
            OnSkipToPhase(new SkipToPhaseEventArgs(1));
        }

        public override void StartPhase()
        {
            FlippedCards = new();
            base.StartPhase();
        }

        public List<Card> GetFlippedCards()
        {
            foreach (Card? c in FlippedCards)
            {
                if (c is null)
                {
                    throw new NullReferenceException("Flipped card was actually null!");
                }
            }
            return FlippedCards!;
        }

        public void increaseIteration()
        {
            iteration++;
        }
        public void resetIteration()
        {
            iteration = 1;
        }
    }
}