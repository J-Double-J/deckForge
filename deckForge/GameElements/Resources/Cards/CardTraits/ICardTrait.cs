using DeckForge.GameConstruction;

namespace DeckForge.GameElements.Resources.Cards.CardTraits
{
    /// <summary>
    /// Trait that defines behavior across various <see cref="ICard"/>s.
    /// </summary>
    public interface ICardTrait
    {
        /// <summary>
        /// Gets the name of the Trait.
        /// </summary>
        public string TraitName { get; }

        /// <summary>
        /// Gets the description of the Trait.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the <see cref="ICard"/> that this trait is attached to.
        /// </summary>
        public ICard AttachedToCard { get; }

        /// <summary>
        /// Executes when the <see cref="ICard"/> this trait is attached to is played.
        /// </summary>
        public virtual void OnPlay()
        {
        }

        /// <summary>
        /// Executes when the <see cref="ICard"/> this trait is attached to is placed.
        /// </summary>
        public virtual void OnPlace()
        {
        }

        /// <summary>
        /// Executes when the <see cref="ICard"/> this trait is attached to is removed.
        /// </summary>
        public virtual void OnCardRemoval()
        {
        }

        /// <summary>
        /// Executes when the trait is removed from the <see cref="ICard"/> it is attached to.
        /// </summary>
        public virtual void OnTraitRemoved()
        {
        }
    }
}