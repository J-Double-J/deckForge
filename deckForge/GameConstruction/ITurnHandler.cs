namespace DeckForge.GameConstruction
{
    public interface ITurnHandler
    {
        public List<int> TurnOrder { get; }
        public int TurnNum { get; }
        public void incrementTurnOrder();
        public int GetWhoseTurn();
        public int GetWhoseTurnXTurnsFromNow(int turns);
        public void UpdatePlayerList(List<int> newPlayerList);
        public void ShiftTurnOrderClockwise();
        public void ShiftTurnOrderCounterClockwise();
    }
}
