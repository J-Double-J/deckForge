using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.Poker
{
    public class DoPreFlopAction : BaseGameAction
    {
        private PokerGameMediator pGM;

        public DoPreFlopAction(
            PokerGameMediator pGM,
            string name = "Pre Flop Action",
            string description = "Lets the player choose an action before the flop.")
        : base(name, description)
        {
            this.pGM = pGM;
        }

        /// <inheritdoc/>
        public override object? Execute()
        {
            pGM.PlayersBet();
            return null;
        }
    }
}