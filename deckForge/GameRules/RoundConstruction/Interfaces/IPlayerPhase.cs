namespace deckForge.GameRules.RoundConstruction.Interfaces
{
    public interface IPlayerPhase
    {
        public void StartPhase(int playerID);
        public void StartPhase(List<int> playerIDs);
        public void EndPhase();
        public void EndPlayerTurn();
        public void UpdatePlayerList(List<int> newPlayerList);
    }
}
