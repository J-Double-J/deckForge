using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameRules.RoundConstruction.Rounds;
using DeckForge.PlayerConstruction;
using DeckForge.PlayerConstruction.PlayerEvents;
using DeckForge.GameConstruction.PresetGames.War;
using DeckForge.GameRules.RoundConstruction.Phases;

namespace DeckForge.GameConstruction.PresetGames.War
{
    public class WarRoundRules : PlayerRoundRules
    {
        bool atWar = false;

        public WarRoundRules(IGameMediator gm, List<int> players) : base(gm, players: players)
        {
            Phases = new List<IPhase>
            {
                new WarPlayCardsPhase(gm, players, "Play Cards"),
                new WarComparePhase(gm, players, "Compare Cards"),
                new WarPhase(gm, players, "War!")
            };

            SubscribeToAllPhases_SkipToPhaseEvents();
            SubscribeToAllPhases_EndRoundEarlyEvents();
        }

        override protected bool NextPhaseHook(int phaseNum)
        {
            bool handledPhase = false;

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

            return handledPhase;
        }

        public void PlayerRaisedEvent(object? sender, SimplePlayerMessageEventArgs args)
        {
            if (args.message == "LOSE_GAME")
            {
                EndRound();
            }
        }
    }
}
