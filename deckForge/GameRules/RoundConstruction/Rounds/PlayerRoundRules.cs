using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.PlayerConstruction;
using deckForge.PhaseActions;
using deckForge.GameRules.RoundConstruction.Interfaces;

namespace deckForge.GameRules.RoundConstruction.Rounds
{
    public class PlayerRoundRules : BaseRoundRules
    {
        int _handLim;

        public int HandLimit
        {
            get { return _handLim; }
            private set
            {
                if (value >= -1)
                {
                    _handLim = value;
                }
                else
                {
                    throw new ArgumentException("Hand Limit cannot be set to lower than 0.", "HandLimit");
                }
            }
        }
        public int CardPlayLimit { get; private set; }
        public Player Player { get; }


        public PlayerRoundRules(List<IPhase> phases, Player p, int handlimit = 64, int cardPlayLimit = 1) : base(phases: phases)
        {
            HandLimit = handlimit;
            CardPlayLimit = cardPlayLimit;
            Player = p;
        }
    }
}
