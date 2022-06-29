using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Interfaces;

namespace deckForge.GameRules.RoundConstruction.Rounds
{
    public class BaseRoundRules : IRoundRules
    {

        protected List<Phase> _phases;
        protected int _curPhase = 0;
        public BaseRoundRules(List<Phase> phases)
        {
            _phases = phases;
        }

        virtual public void StartRound() {
            //Loop makes sure that if a phase is skipped it handles it correctly
            while (_curPhase < _phases.Count) {
                _phases[_curPhase].StartPhase();
                _curPhase++;

                if (_curPhase >= _phases.Count()) {
                    break;
                }
            }

            EndRound();
        }

        virtual public void EndRound() {}
    }
}