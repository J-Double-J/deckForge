namespace deckForge.GameRules.RoundConstruction.Interfaces
{
    public interface IRoundRules
    {
        public void StartRound();
        public void EndRound();
        public void SkipToPhase(int phaseNum);
    }
}