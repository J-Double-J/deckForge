using deckForge.GameRules.RoundConstruction.Interfaces;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarGameMediator : BaseGameMediator
    {
        IRoundRules _roundRules;
        public WarGameMediator(int playerCount, IRoundRules roundRules) : base(playerCount) {
            _roundRules = roundRules;
        }

        public override void StartGame() {
            //Single perpetual round
            _roundRules.StartRound();
            EndGame();
        }

    }
}
