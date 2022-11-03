using DeckForge.GameConstruction;

namespace DeckForge.GameElements.Resources.Cards.CardTraits
{
    /// <summary>
    /// Trait for any Character Card.
    /// </summary>
    public class CharacterTrait : CharacterCardTrait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTrait"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to interact with other game elements.</param>
        /// <param name="attachedToCard"><see cref="BaseCharacterCard"/> this trait is attached to.</param>
        public CharacterTrait(IGameMediator gm, BaseCharacterCard attachedToCard)
            : base(gm, attachedToCard)
        {
        }

        /// <inheritdoc/>
        public override void OnPlay()
        {
            if (AttachedToCard.TablePlacemenetDetails?.TablePlacementZone == TablePlacementZones.PlayerZone)
            {
                GM.ChangeCardModifierValueBy(HelperObjects.CardModifiers.CharacterCardsInPlayerZones, 1);
            }
        }

        /// <inheritdoc/>
        public override void OnPlace()
        {
            // Currently they do the same thing.
            OnPlay();
        }

        /// <inheritdoc/>
        public override void OnCardRemoval()
        {
            if (AttachedToCard.TablePlacemenetDetails?.TablePlacementZone == TablePlacementZones.PlayerZone)
            {
                GM.ChangeCardModifierValueBy(HelperObjects.CardModifiers.CharacterCardsInPlayerZones, -1);
            }
        }

        /// <inheritdoc/>
        public override void OnTraitRemoved()
        {
            // Currently they do the same thing.
            OnCardRemoval();
        }
    }
}
