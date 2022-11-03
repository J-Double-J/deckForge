using DeckForge.GameConstruction;

namespace DeckForge.GameElements.Resources.Cards.CardTraits
{
    /// <summary>
    /// A <see cref="ICard"/> trait that requires the attached card to be a <see cref="BaseCharacterCard"/>.
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
    }
}
