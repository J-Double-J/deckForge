using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.PlayerConstruction;
using deckForge.PhaseActions;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarPlayCardsPhase : PlayerPhase
    {

        List<Card?> FlippedCards = new();
        public WarPlayCardsPhase(IGameMediator gm, string name)
        : base(gm, name)
        {
            Actions.Add(new PlayCardsAction(facedown: true));
            Actions.Add(new FlipOneCard_OneWay_Action(0, facedown: false));
        }

        public List<Card> GetFlippedCards()
        {
            foreach (Card? c in FlippedCards)
            {
                if (c is null)
                    throw new NullReferenceException("The flipped card was actually null!");
            }
            return FlippedCards!;
        }

        override protected void PhaseActionLogic(int playerID, int actionNum, out bool repeatAction)
        {
            repeatAction = false;

            try
            {
                if (actionNum == 0)
                    FlippedCards.Add((Card?)GM.TellPlayerToDoAction(playerID, Actions[actionNum]));
            }
            catch {
                throw;
            }
            
        }

        public override void StartPhase()
        {
            FlippedCards = new();
            base.StartPhase();
        }
    }
}
