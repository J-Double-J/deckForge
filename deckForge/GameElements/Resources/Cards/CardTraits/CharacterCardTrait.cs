using DeckForge.GameConstruction;
using DeckForge.GameElements.Table;

namespace DeckForge.GameElements.Resources.Cards.CardTraits
{
    /// <summary>
    /// A <see cref="BaseCardTrait"/> trait for any <see cref="BaseCharacterCard"/>.
    /// </summary>
    public class CharacterCardTrait : BaseCardTrait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterCardTrait"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to interact with other game elements.</param>
        /// <param name="attachedCard"><see cref="BaseCharacterCard"/> that this trait is attached to.</param>
        public CharacterCardTrait(IGameMediator gm, BaseCharacterCard attachedCard)
            : base(gm, attachedCard)
        {
        }

        /// <inheritdoc/>
        public override void OnPlay()
        {
            if (AttachedToCard.TablePlacementDetails?.TablePlacementZone == TablePlacementZoneType.PlayerZone)
            {
                GM.ChangeCardModifierValueBy(HelperObjects.CardModifiers.CharacterCardsInPlayerZones, 1);
            }
        }

        /// <inheritdoc/>
        public override void OnPlace()
        {
            if (AttachedToCard.TablePlacementDetails?.TablePlacementZone == TablePlacementZoneType.PlayerZone)
            {
                GM.ChangeCardModifierValueBy(HelperObjects.CardModifiers.CharacterCardsInPlayerZones, 1);
            }
        }

        /// <inheritdoc/>
        public override void OnCardRemoval()
        {
            if (AttachedToCard.TablePlacementDetails?.TablePlacementZone == TablePlacementZoneType.PlayerZone)
            {
                GM.ChangeCardModifierValueBy(HelperObjects.CardModifiers.CharacterCardsInPlayerZones, -1);
            }
        }

        /// <inheritdoc/>
        public override void OnTraitRemoved()
        {
            if (AttachedToCard.TablePlacementDetails?.TablePlacementZone == TablePlacementZoneType.PlayerZone)
            {
                GM.ChangeCardModifierValueBy(HelperObjects.CardModifiers.CharacterCardsInPlayerZones, -1);
            }
        }
    }
}
