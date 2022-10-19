using DeckForge.GameConstruction;

namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// Abstract class that outlines methods for any cards with effects that can be triggered.
    /// </summary>
    public abstract class EffectCard : Card
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffectCard"/> class.
        /// </summary>
        /// <param name="gm">Sets the <see cref="IGameMediator"/> that will be used to communicate with other game elements.</param>
        /// <param name="facedown">Sets the <see cref="Facedown"/> property of <see cref="Card"/>.</param>
        public EffectCard(IGameMediator gm, bool facedown)
            : base(facedown)
        {
            GM = gm;
        }

        /// <summary>
        /// Gets the <see cref="IGameMediator"/> that is used by the card to communicate with other game elements.
        /// </summary>
        protected IGameMediator GM { get; }

        /// <summary>
        /// Executes an <see cref="IGameAction"/> when called.
        /// </summary>
        public abstract void ExecuteEffect();
    }
}
