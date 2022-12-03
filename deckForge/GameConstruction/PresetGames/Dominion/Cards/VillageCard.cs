using DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTraits;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
    /// <summary>
    /// An action card that costs 3 coins. Gives +1 card and +2 actions.
    /// </summary>
    public class VillageCard : DominionCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VillageCard"/> class.
        /// </summary>
        public VillageCard()
            : base(new() { { typeof(Coin), 3 } }, "Village", "+1 Card, +2 Actions")
        {
            Traits.Add(new ActionTrait(this));
        }

        /// <inheritdoc/>
        public override void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
            base.OnPlay(placementDetails);
            OwnedBy?.DrawCard();
            ((DominionPlayer?)OwnedBy)?.GainAction(new PlayCardAction(), 2);
        }
    }
}
