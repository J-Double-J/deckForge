namespace DeckForge.GameElements.Resources
{
    public interface ICard 
    {
        /// <summary>
        /// Gets or sets whether the <see cref="ICard"/> is facedown or not. If true,
        /// <see cref="ICard"/> is facedown.
        /// </summary>
        public bool Facedown { get; set; }

        /// <summary>
        /// Gets the string representing the card.
        /// </summary>
        /// <returns>Returns the string representation of the card.</returns>
        public string PrintCard();

        /// <summary>
        /// Swaps the boolean of <see cref="Facedown"/>.
        /// </summary>
        public void Flip();
    }
}