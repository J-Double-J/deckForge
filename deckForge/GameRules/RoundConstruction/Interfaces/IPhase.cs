namespace deckForge.GameRules.RoundConstruction.Interfaces

{
    public interface IPhase
    {
        public void StartPhase();
        public string PhaseName { get; }
        event EventHandler<SkipToPhaseEventArgs> SkipToPhase;

    }

    public class SkipToPhaseEventArgs : EventArgs
    {
        public readonly int phaseNum;
        public SkipToPhaseEventArgs(int phaseNum) { this.phaseNum = phaseNum; }
    }
}
