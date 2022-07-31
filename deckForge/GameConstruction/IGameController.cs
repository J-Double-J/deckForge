namespace DeckForge.GameConstruction
{
    public interface IGameController
    {
        public int PlayerCount { get; }
        public List<int> TurnOrder { get; }
        public int NextPlayerTurn();
        public void UpdatePlayerList(List<int> newPlayerList);
        public int PlayerTurnXTurnsFromNow(int turns);
        public void ShiftTurnOrderClockwise();
        public void ShiftTurnOrderCounterClockwise();
        public int GetCurrentPlayer();
        public void EndGame();
    }
}
