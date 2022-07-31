using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PlayerConstruction;
using DeckForge.PhaseActions;
using DeckForge.GameElements.Resources;

namespace DeckForge.GameConstruction.PresetGames.War
{
    public class WarPhase : PlayerPhase
    {
        int iteration = 1;
        List<Card?> FlippedCards = new();
        public WarPhase(IGameMediator gm, List<int> playerIDs, string name)
        : base(gm, playerIDs, name)
        {
            Actions.Add(new PlayMultipleCardsAction(playCount: 2));
            Actions.Add(new FlipOneCard_OneWay_Action(2 * iteration));
        }

        protected override void PhaseActionLogic(int playerID, int actionNum, out bool handledAction)
        {
            handledAction = false;

            if (actionNum == 1)
            {
                handledAction = true;
                FlippedCards.Add((Card?)GM.TellPlayerToDoAction(playerID, Actions[actionNum]));
                foreach (Card? card in FlippedCards) {
                    if (card is null) {
                        OnEndRoundEarly(new EndRoundEarlyArgs());
                        break;
                    }
                }
            }
        }

        public override void EndPhase()
        {
            if (CurrentAction  >= 0)
                OnSkipToPhase(new SkipToPhaseEventArgs(1));
        }

        public override void StartPhase(List<int> playerIDs)
        {
            FlippedCards = new();
            base.StartPhase(playerIDs);
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