namespace DeckForge.GameElements.Resources
{
    /// <summary>
    /// <see cref="ICharacterCard"/> is a <see cref="ICard"/> that can attack other cards.
    /// </summary>
    public interface ICharacterCard : ICard
    {
        // TODO: Attack the player directly.

        /// <summary>
        /// <see cref="ICharacterCard"/> attacks another <see cref="ICharacterCard"/>.
        /// </summary>
        /// <param name="target">The <see cref="ICharacterCard"/> targetted by this <see cref="ICharacterCard"/>.</param>
        public void Attack(ICharacterCard target);

        /// <summary>
        /// This <see cref="ICharacterCard"/> dies and does any on-death effects.
        /// Raises CardIsRemovedFromTableEventArgs after effects to be removed.
        /// </summary>
        public void Die();
    }
}
