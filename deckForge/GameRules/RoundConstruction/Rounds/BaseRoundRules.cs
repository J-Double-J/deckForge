using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Interfaces;

namespace deckForge.GameRules.RoundConstruction.Rounds
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