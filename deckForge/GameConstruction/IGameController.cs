namespace deckForge.GameConstruction
{
    public interface IGameController
    {
        public int PlayerCount { get; }
        public int NextPlayerTurn();
        public int PlayerTurnXTurnsFromNow(int turns);
        public int GetCurrentPlayer();
        public void EndGame();
    }
}
