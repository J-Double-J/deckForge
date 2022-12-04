using DeckForge.GameConstruction.PresetGames.Dominion.Actions;
using DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTraits;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
    /// <summary>
    /// A Dominion card that costs 3 coins. Gives +1 buy and +2 coins.
    /// </summary>
    public class WoodcutterCard : DominionCard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WoodcutterCard"/> class.
        /// </summary>
        public WoodcutterCard()
            : base(new() { { typeof(Coin), 3 } }, "Woodcutter", "+1 Buy, +2 Coins")
        {
            Traits.Add(new ActionTrait(this));
        }

        /// <inheritdoc/>
        public override void OnPlay(CardPlacedOnTableDetails placementDetails)
        {
            base.OnPlay(placementDetails);
            DominionPlayer? player = OwnedBy as DominionPlayer;
            player?.GainAction(new BuyAction(), 1);
            player?.IncreaseCoins(2);
        }
    }
}
