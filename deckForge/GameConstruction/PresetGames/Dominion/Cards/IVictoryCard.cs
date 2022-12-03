namespace DeckForge.GameConstruction.PresetGames.Dominion.Cards
{
    /// <summary>
    /// <see cref="DominionCard"/>s that are worth points.
    /// </summary>
    public interface IVictoryCard
    {
        /// <summary>
        /// Gets the number of victory points this card is worth.
        /// </summary>
        public int VictoryPoints { get; }
    }
}
