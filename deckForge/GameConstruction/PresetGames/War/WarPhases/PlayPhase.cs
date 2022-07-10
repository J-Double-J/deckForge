using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.PlayerConstruction;
using deckForge.PhaseActions;
using CardNamespace;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarPlayCardsPhase : PlayerPhase
    {

        List<Card?> FlippedCards = new List<Card?>();
        public WarPlayCardsPhase(List<IPlayer> players, string name)
        : base(players, name)
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

        override protected void NextActionHook(IPlayer p, int actionNum, out bool repeatAction)
        {
            repeatAction = false;
            if (actionNum == 0)
                FlippedCards.Add((Card?)Actions[actionNum].execute(p));
        }

        public override void StartPhase()
        {
            FlippedCards = new();
            base.StartPhase();
        }
    }
}