namespace deckForge.GameRules.RoundConstruction.Interfaces

{
    public interface IPhase
    {
        public void StartPhase();
        public void EndPhase();
        public string PhaseName { get; }

    }
}