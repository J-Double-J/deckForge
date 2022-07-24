using deckForge.GameRules.RoundConstruction.Interfaces;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class WarGameMediator : BaseGameMediator
    {
        public WarGameMediator(int playerCount) : base(playerCount) {}

        public override void StartGame() {
            RoundRules![0].StartRound();
        }

        override public void RoundEnded() {
            GameController!.ShiftTurnOrderClockwise();
            RoundRules![0].StartRound();
        }
    }
}
