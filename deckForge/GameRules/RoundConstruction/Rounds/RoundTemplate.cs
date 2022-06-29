using deckForge.GameRules.RoundConstruction.Phases;

namespace deckForge.GameRules.RoundConstruction.Rounds
{
    public class RoundTemplate
    {
        List<Phase> phases;
        int curPhase = 0;

        public RoundTemplate(List<Phase> phases)
        {
            this.phases = phases;
        }

        virtual public void StartRound()
        {
            NextPhase(0);
        }

        virtual protected void NextPhase(int phaseNum)
        {
            try
            {
                if (curPhase + 1 <= phases.Count)
                {
                    phases[curPhase].StartPhase();
                    curPhase++;
                    NextPhase(curPhase);
                }
                else
                {
                    EndRound();
                }
            }
            catch
            {
                throw;
            }

        }

        virtual public void EndRound() { }
    }
}