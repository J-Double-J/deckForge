namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// A collection of <see cref="PlayingCard"/>s.
    /// </summary>
    public interface IDeck : IResourceCollection<PlayingCard>
    {
        /// <summary>
        /// Draw a <see cref="PlayingCard"/> from the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="drawFacedown">Draws the <see cref="PlayingCard"/> facedown if <c>true</c>.</param>
        /// <returns>A nullable <see cref="PlayingCard"/> that can be null if a <see cref="PlayingCard"/> could not
        /// be drawn from the <see cref="IDeck"/>.</returns>
        public PlayingCard? DrawCard(bool drawFacedown = false);

        /// <summary>
        /// Draw multiple <see cref="PlayingCard"/>s from the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="count">Number of <see cref="PlayingCard"/>s to draw.</param>
        /// <returns>A list of nullable <see cref="PlayingCard"/>s that can be null
        /// if a <see cref="PlayingCard"/> could not be drawn from the <see cref="IDeck"/>.</returns>
        public List<PlayingCard?> DrawMultipleCards(int count);

        /// <summary>
        /// Shuffles the <see cref="IDeck"/>.
        /// </summary>
        public void Shuffle();

        /// <summary>
        /// Adds a <see cref="PlayingCard"/> to the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="card"><see cref="PlayingCard"/> to add to the <see cref="IDeck"/>.</param>
        /// <param name="pos">Specifies where to place the <see cref="PlayingCard"/>. Options
        /// are "top", "middle", or "bottom".</param>
        /// <param name="shuffleAfter">If <c>true</c>, shuffles the <see cref="IDeck"/> after adding
        /// the <see cref="PlayingCard"/>.</param>
        public void AddCardToDeck(PlayingCard card, string pos = "bottom", bool shuffleAfter = false);

        /// <summary>
        /// Adds multiple <see cref="PlayingCard"/>s to the <see cref="IDeck"/>.
        /// </summary>
        /// <param name="cards">The list of <see cref="PlayingCard"/>s to add to the <see cref="IDeck"/>.</param>
        /// <param name="pos">Specifies where to place the <see cref="PlayingCard"/>. Options
        /// are "top", "middle", or "bottom".</param>
        /// <param name="shuffleAfter">If <c>true</c>, shuffles the <see cref="IDeck"/> after adding
        /// the <see cref="PlayingCard"/>.</param>
        public void AddMultipleCardsToDeck(List<PlayingCard> cards, string pos = "bottom", bool shuffleAfter = false);
    }
}
