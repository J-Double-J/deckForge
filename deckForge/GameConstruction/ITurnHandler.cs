namespace deckForge.GameConstruction
{
    public interface ITurnHandler
    {
        public List<int> TurnOrder { get; }
        public int TurnNum { get; }
        public void incrementTurnOrder();
        public int GetWhoseTurn();
        public int GetWhoseTurnXTurnsFromNow(int turns);
    }
}
