using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards.CardTraits;

namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards.CardTraits
{
    /// <summary>
    /// <see cref="BaseCardTrait"/> that is attached to any <see cref="ITreasureCard"/>s.
    /// </summary>
    public class TreasureTrait : BaseCardTrait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreasureTrait"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="attachedToCard"><see cref="ITreasureCard"/> this trait is attached to.</param>
        public TreasureTrait(ITreasureCard attachedToCard)
            : base(attachedToCard)
        {
        }

        /// <inheritdoc/>
        public override void OnPlay()
        {
            var player = (DominionPlayer)AttachedToCard.OwnedBy!;
            var treasureValue = ((ITreasureCard)AttachedToCard).TreasureValue;

            player.IncreaseCoins(treasureValue);
        }
    }
}
