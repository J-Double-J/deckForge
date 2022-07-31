namespace DeckForge.GameRules.RoundConstruction.Interfaces

{
    public interface IPhase
    {
        public void StartPhase();
        public void EndPhase();
        public void EndPhaseEarly();
        public string PhaseName { get; }

        event EventHandler<SkipToPhaseEventArgs> SkipToPhase;
        event EventHandler<EndRoundEarlyArgs> EndRoundEarly;
        event EventHandler<PhaseEndedArgs> PhaseEnded;
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

    public class PhaseEndedArgs : EventArgs
    {
        public readonly string? message;
        public readonly string phaseName;
        public PhaseEndedArgs(string phaseName) { this.phaseName = phaseName; }
        public PhaseEndedArgs(string phaseName, string message)
        {
            this.phaseName = phaseName;
            this.message = message;
        }
    }
}
