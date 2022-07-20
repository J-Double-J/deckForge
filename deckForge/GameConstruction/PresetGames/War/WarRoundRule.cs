using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameRules.RoundConstruction.Rounds;
using deckForge.PlayerConstruction;
using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.GameConstruction.PresetGames.War;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarRoundRules : PlayerRoundRules
    {
        override public List<IPhase> Phases { get; }
        bool atWar = false;

        public WarRoundRules(List<IPlayer> players) : base(players: players)
        {
            Phases = new List<IPhase>();
            Phases.Add(new WarPlayCardsPhase(players: players, "Plays Cards"));
            Phases.Add(new WarComparePhase(players: players, "Compare Cards"));
            Phases.Add(new WarPhase(players: players, "War!"));

            foreach (IPlayer player in players) {
                player.PlayerMessageEvent += PlayerRaisedEvent;
            }
        }

        public override void NextPhaseHook(int phaseNum, out bool repeatPhase)
        {
            repeatPhase = true;

            if (phaseNum == 0)
            {
                atWar = false;

                //Reset WarPhase Counter
                var phase = (WarPhase)Phases[2];
                phase.resetIteration();
            }

            else if (phaseNum == 1)
            {

                if (atWar)
                {
                    //Give ComparePhase the cards flipped from War
                    var warPhase = (WarPhase)Phases[2];
                    var comparePhase = (WarComparePhase)Phases[1];
                    comparePhase.FlippedCards = warPhase.GetFlippedCards();
                }
                else
                {
                    //Give ComparePhase the cards to compare
                    var phase = (WarPlayCardsPhase)Phases[0];
                    var compare = (WarComparePhase)Phases[1];
                    compare.FlippedCards = phase.GetFlippedCards();
                }

            }
            else if (phaseNum == 2)
            {
                //Update War Status, and track num of War iterations
                if (atWar == false)
                {
                    atWar = true;
                }
                else
                {
                    var phase = (WarPhase)Phases[2];
                    phase.increaseIteration();
                }

            }
        }

        public void PlayerRaisedEvent(object? sender, SimplePlayerMessageEventArgs args) {
            if (args.message == "LOSE_GAME") {
                EndRound();

            }

        }
    }
}
