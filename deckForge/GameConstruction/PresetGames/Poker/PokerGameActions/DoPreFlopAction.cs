using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.Poker
{
    public class DoPreFlopAction : PlayerGameAction
    {
        public DoPreFlopAction(
            string name = "Pre Flop Action",
            string description = "Lets the player choose an action before the flop."
        ) : base(name, description)
        {
        }

        public override string Execute(IPlayer player)
        {
            PokerPlayer? pokerPlayer = player as PokerPlayer;
            if (pokerPlayer is null)
            {
                throw new ArgumentException("Poker Actions require Poker Players");
            }
            else
            {
                return pokerPlayer!.GetPreFlopBettingAction();
            }
        }
    }
}