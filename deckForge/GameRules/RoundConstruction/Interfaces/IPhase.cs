namespace deckForge.GameRules.RoundConstruction.Interfaces

{
    public interface IPhase
    {
        public void StartPhase();
        public void EndPhaseEarly();
        public string PhaseName { get; }

        event EventHandler<SkipToPhaseEventArgs> SkipToPhase;
        event EventHandler<EndRoundEarlyArgs> EndRoundEarly;

    }

    public class SkipToPhaseEventArgs : EventArgs
    {
        public readonly int phaseNum;
        public SkipToPhaseEventArgs(int phaseNum) { this.phaseNum = phaseNum; }
    }

    public class EndRoundEarlyArgs : EventArgs
    {
        public readonly string? message;
        public EndRoundEarlyArgs() { }
        public EndRoundEarlyArgs(string message) { this.message = message; }
    }
}
