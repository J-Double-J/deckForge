namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// Base class for all cards.
    /// </summary>
    public abstract class Card : ICard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="facedown">Sets the <see cref="Facedown"/> property of <see cref="Card"/>.</param>
        public Card(bool facedown = true)
        {
            Facedown = facedown;
        }

        /// <inheritdoc/>
        public bool Facedown
        {
            get; set;
        }

        /// <inheritdoc/>
        public void Flip()
        {
            if (Facedown)
            {
                Facedown = false;
            }
            else
            {
                Facedown = true;
            }
        }

        /// <inheritdoc/>
        public abstract string PrintCard();
    }
}