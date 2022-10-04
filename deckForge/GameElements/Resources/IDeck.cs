namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// A collection of <see cref="ICard"/>s.
    /// </summary>
    public interface IDeck : IResourceCollection<ICard>
    {
        /// <summary>
        /// Draw a <see cref="ICard"/> from the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="drawFacedown">Draws the <see cref="ICard"/> facedown if <c>true</c>.</param>
        /// <returns>A nullable <see cref="ICard"/> that can be null if a <see cref="ICard"/> could not
        /// be drawn from the <see cref="IDeck"/>.</returns>
        public ICard? DrawCard(bool drawFacedown = false);

        /// <summary>
        /// Draw multiple <see cref="ICard"/>s from the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="count">Number of <see cref="ICard"/>s to draw.</param>
        /// <returns>A list of nullable <see cref="ICard"/>s that can be null
        /// if a <see cref="ICard"/> could not be drawn from the <see cref="IDeck"/>.</returns>
        public List<ICard?> DrawMultipleCards(int count);

        /// <summary>
        /// Shuffles the <see cref="IDeck"/>.
        /// </summary>
        public void Shuffle();

        /// <summary>
        /// Adds a <see cref="ICard"/> to the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="card"><see cref="ICard"/> to add to the <see cref="IDeck"/>.</param>
        /// <param name="pos">Specifies where to place the <see cref="ICard"/>. Options
        /// are "top", "middle", or "bottom".</param>
        /// <param name="shuffleAfter">If <c>true</c>, shuffles the <see cref="IDeck"/> after adding
        /// the <see cref="ICard"/>.</param>
        public void AddCardToDeck(ICard card, string pos = "bottom", bool shuffleAfter = false);

        /// <summary>
        /// Adds multiple <see cref="ICard"/>s to the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="cards">The list of <see cref="ICard"/>s to add to the <see cref="IDeck"/>.</param>
        /// <param name="pos">Specifies where to place the <see cref="ICard"/>. Options
        /// are "top", "middle", or "bottom".</param>
        /// <param name="shuffleAfter">If <c>true</c>, shuffles the <see cref="IDeck"/> after adding
        /// the <see cref="ICard"/>.</param>
        public void AddMultipleCardsToDeck(List<ICard> cards, string pos = "bottom", bool shuffleAfter = false);
    }
}
