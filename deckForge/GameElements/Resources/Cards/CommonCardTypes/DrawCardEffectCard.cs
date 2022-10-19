using DeckForge.GameConstruction;
using DeckForge.PhaseActions;

namespace DeckForge.GameElements.Resources.Cards
{
    /// <summary>
    /// A <see cref="ICard"/> that causes the <see cref="IPlayer"/> to draw some cards.
    /// </summary>
    public class DrawCardEffectCard : InstantEffectCard
    {
        private readonly DrawCardsAction cardEffect = new();
        private int drawCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawCardEffectCard"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> used to communicate with other game elements.</param>
        /// <param name="drawCount">Number of <see cref="ICard"/>s to draw.</param>
        /// <param name="facedown">Default orientation of the card.</param>
        public DrawCardEffectCard(IGameMediator gm, int drawCount = 1, bool facedown = true)
            : base(gm, facedown, description: "On play, draws one card.")
        {
            DrawCount = drawCount;
        }

        /// <summary>
        /// Gets or sets the number of <see cref="ICard"/>s to draw when played.
        /// </summary>
        public int DrawCount
        {
            get
            {
                return drawCount;
            }

            set
            {
                drawCount = value;
                cardEffect.DrawCount = drawCount;
            }
        }

        /// <inheritdoc/>
        public override void ExecuteEffect()
        {
            if (OwnedBy is not null)
            {
                cardEffect.Execute(OwnedBy);
            }
        }
    }
}
