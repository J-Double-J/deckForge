namespace deckForge.GameRules.PlayerRoundRules
{
    public class BaseRoundRules : IRoundRules
    {
        protected List<Phase> _phases;
        public BaseRoundRules(List<Phase> phases)
        {
            _phases = phases;
        }

        virtual public void StartPhase() { }
        virtual public void EndPhase() { }
    }
}