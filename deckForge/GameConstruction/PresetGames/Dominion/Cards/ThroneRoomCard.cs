using DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTraits;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
    /// <summary>
    /// A Dominion card that costs 4 coins
    /// </summary>
    public class ThroneRoomCard : DominionCard
    {
        private int numberOfCardsToDouble = 1;
        private int startDoublingAtNthTrigger = 1;

        public ThroneRoomCard()
            : base(new Dictionary<Type, int>() { { typeof(Coin), 4 } }, "Throne", "You may play an Action card from your hand twice")
        {
        }

        public override void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
            OwnedBy!.PlayerPlayedCard += PlayerPlayedCard;
            ((DominionPlayer)OwnedBy).GainAction(new PlayCardAction(), 1);
        }

        /// <inheritdoc/>
        public override void OnRemoval()
        {
            base.OnRemoval();
            numberOfCardsToDouble = 1;
            startDoublingAtNthTrigger = 1;
            OwnedBy!.PlayerPlayedCard -= PlayerPlayedCard;
        }

        /// <summary>
        /// This tells the recently played Throne card to wait its turn before doubling as there are other Throne Cards
        /// doubling before it.
        /// </summary>
        /// <param name="numberOfCardsToDouble">Number of cards this <see cref="ThroneRoomCard"/> will double.</param>
        /// <param name="startDoublingAtNthTrigger">How many time <see cref="ICard"/>s with <see cref="ActionTrait"/> must be played
        /// before this <see cref="ThroneRoomCard"/> begins doubling.</param>
        public void ChangeNumberOfCardToDoubleAndWhenTo(int numberOfCardsToDouble, int startDoublingAtNthTrigger)
        {
            this.numberOfCardsToDouble = numberOfCardsToDouble;
            this.startDoublingAtNthTrigger = startDoublingAtNthTrigger;
            ((DominionPlayer?)OwnedBy)!.GainAction(new PlayCardAction(), numberOfCardsToDouble - 1);
        }

        private void PlayerPlayedCard(object? sender, PlayerPlayedCardEventArgs e)
        {
            if (e.CardPlayed.CardTraits.OfType<ActionTrait>().Any() && e.CardPlayed != this)
            {
                startDoublingAtNthTrigger--;
                if (startDoublingAtNthTrigger == 0)
                {
                    DoubleActionCardEffect(e.CardPlayed);
                    numberOfCardsToDouble--;
                    StopListeningIfNoMoreDoubleActions();
                }
            }
        }

        private void DoubleActionCardEffect(ICard cardPlayed)
        {
            ThroneRoomCard? potentailThroneCard = cardPlayed as ThroneRoomCard;
            if (potentailThroneCard is not null)
            {
                potentailThroneCard.ChangeNumberOfCardToDoubleAndWhenTo(2, startDoublingAtNthTrigger + 1);
            }
            else
            {
                cardPlayed.OnPlay(cardPlayed.TablePlacementDetails!);
            }
        }

        private void StopListeningIfNoMoreDoubleActions()
        {
            if (numberOfCardsToDouble == 0)
            {
                OwnedBy!.PlayerPlayedCard -= PlayerPlayedCard;
            }
        }
    }
}
