using DeckForge.GameConstruction;

namespace DeckForge.GameElements.Resources.Cards.CardTraits
{
    /// <summary>
    /// The base outline for any trait that a <see cref="ICard"/> may possess.
    /// </summary>
    public class BaseCardTrait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCardTrait"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to interact with other game elements.</param>
        public BaseCardTrait(IGameMediator gm)
        {
            TraitName = string.Empty;
            Description = string.Empty;
            GM = gm;
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
        /// Gets the <see cref="IGameMediator"/> used to interact with other game elements.
        /// </summary>
        protected IGameMediator GM { get;  }

        /// <summary>
        /// Executes when the <see cref="ICard"/> this trait is attached to is played.
        /// </summary>
        public void OnPlay()
        {
        }

        /// <summary>
        /// Executes when the <see cref="ICard"/> this trait is attached to is placed.
        /// </summary>
        public void OnPlace()
        {
        }

        /// <summary>
        /// Executes when the <see cref="ICard"/> this trait is attached to is removed.
        /// </summary>
        public void OnRemoval()
        {
        }
    }
}
