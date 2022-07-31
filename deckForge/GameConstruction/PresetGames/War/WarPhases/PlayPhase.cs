using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PlayerConstruction;
using DeckForge.PhaseActions;
using DeckForge.GameElements.Resources;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs;

namespace DeckForge.GameConstruction.PresetGames.War
{ 
    public class WarPlayCardsPhase : PlayerPhase
    {

        List<Card?> FlippedCards = new();
        public WarPlayCardsPhase(IGameMediator gm, List<int> playerIDs, string name)
        : base(gm, playerIDs, name)
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

        /// <inheritdoc/>
        protected override bool PhaseActionLogic(int playerID, int actionNum)
        {
            bool handledAction = false;

            try
            {
                if (actionNum == 0)
                {
                    handledAction = true;
                    Card? drawnCard = (Card?)GM.TellPlayerToDoAction(playerID, Actions[actionNum]);

                    if (drawnCard is not null)
                    {
                        FlippedCards.Add(drawnCard);
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

        public override void StartPhase()
        {
            FlippedCards = new();
            base.StartPhase();
        }

        public override void StartPhase(List<int> playerIDs)
        {
            FlippedCards = new();
            base.StartPhase(playerIDs);
        }
    }
}
