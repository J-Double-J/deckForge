using DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTraits;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
    /// <summary>
    /// A Dominion card that cost 5 coins. Gives +2 Cards and +1 Action.
    /// </summary>
    public class LaboratoryCard : DominionCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LaboratoryCard"/> class.
        /// </summary>
        public LaboratoryCard()
            : base(new() { { typeof(Coin), 5 } }, "Laboratory", "+2 Cards, +1 Action")
        {
            Traits.Add(new ActionTrait(this));
        }

        /// <inheritdoc/>
        public override void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
            base.OnPlay(placementDetails);
            OwnedBy?.DrawMultipleCards(2);
            ((DominionPlayer?)OwnedBy)?.GainAction(new PlayCardAction(), 1);
        }
    }
}
