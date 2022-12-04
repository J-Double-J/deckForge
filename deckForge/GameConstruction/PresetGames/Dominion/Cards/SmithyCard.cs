using DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTraits;
using DeckForge.GameElements.Resources;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
    /// <summary>
    /// A Dominion card that costs 4 coins. Gives +3 cards.
    /// </summary>
    public class SmithyCard : DominionCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmithyCard"/> class.
        /// </summary>
        public SmithyCard()
            : base(new() { { typeof(Coin), 4 } }, "Smithy", "+3 Cards")
        {
            Traits.Add(new ActionTrait(this));
        }

        /// <inheritdoc/>
        public override void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
            base.OnPlay(placementDetails);
            OwnedBy?.DrawMultipleCards(3);
        }
    }
}
