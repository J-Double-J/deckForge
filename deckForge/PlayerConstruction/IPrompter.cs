namespace DeckForge.PlayerConstruction
{
    /// <summary>
    /// An interface for objects that prompt a user for some kind of input.
    /// </summary>
    public interface IPrompter
    {
        /// <summary>
        /// Presents the Player with the given prompt.
        /// </summary>
        /// <returns>An integer that is the response that the <see cref="IPlayer"/> chose to execute.</returns>
        public int Prompt();
    }
}