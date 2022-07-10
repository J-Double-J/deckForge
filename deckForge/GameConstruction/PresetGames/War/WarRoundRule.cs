using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameRules.RoundConstruction.Rounds;
using deckForge.PlayerConstruction;
using deckForge.GameConstruction.PresetGames.War;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarRoundRules : PlayerRoundRules
    {
        override public List<IPhase> Phases { get; }
        bool goingToWar = false;

        public WarRoundRules(List<IPlayer> players) : base(players: players)
        {
            Phases = new List<IPhase>();
            Phases.Add(new WarPlayCardsPhase(players: players, "Plays Cards"));
            Phases.Add(new WarComparePhase(players: players, "Compare Cards"));
            Phases.Add(new WarPhase(players: players, "War!"));
        }

        public override void NextPhaseHook(int phaseNum, out bool repeatPhase)
        {
            repeatPhase = true;

            if (phaseNum == 0)
            {
                goingToWar = false;
                var phase = (WarPhase)Phases[2];
                phase.resetIteration();
            }

            else if (phaseNum == 1)
            {

                if (goingToWar)
                {
                    var warPhase = (WarPhase)Phases[2];
                    var comparePhase = (WarComparePhase)Phases[1];
                    comparePhase.FlippedCards = warPhase.GetFlippedCards();
                }
                else
                {
                    var phase = (WarPlayCardsPhase)Phases[0];
                    var compare = (WarComparePhase)Phases[1];
                    compare.FlippedCards = phase.GetFlippedCards();
                }

            }
            else if (phaseNum == 2)
            {

                if (goingToWar == false)
                {
                    goingToWar = true;
                }
                else
                {
                    var phase = (WarPhase)Phases[2];
                    phase.increaseIteration();
                }

            }
        }
    }
}
