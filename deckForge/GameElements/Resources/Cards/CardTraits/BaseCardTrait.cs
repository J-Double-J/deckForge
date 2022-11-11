using DeckForge.GameConstruction;

namespace DeckForge.GameElements.Resources.Cards.CardTraits
{
    /// <summary>
    /// The base outline for any trait that a <see cref="ICard"/> may possess.
    /// </summary>
    public abstract class BaseCardTrait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCardTrait"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to interact with other game elements.</param>
        /// <param name="attachedToCard"><see cref="ICard"/> this trait is attached to.</param>
        public BaseCardTrait(IGameMediator gm, ICard attachedToCard)
        {
            TraitName = string.Empty;
            Description = string.Empty;
            GM = gm;
            AttachedToCard = attachedToCard;
        }

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
        /// Gets the <see cref="IGameMediator"/> used to interact with other game elements.
        /// </summary>
        protected IGameMediator GM { get;  }

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
