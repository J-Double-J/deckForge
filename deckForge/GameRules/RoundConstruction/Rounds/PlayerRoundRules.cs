using deckForge.PlayerConstruction;
using deckForge.GameRules.RoundConstruction.Interfaces;

namespace deckForge.GameRules.RoundConstruction.Rounds
{
    abstract public class PlayerRoundRules : BaseRoundRules
    {
        abstract override public List<IPhase> Phases { get; }
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
        public List<Player> Players { get; }


        public PlayerRoundRules(List<Player> players, int handlimit = 64, int cardPlayLimit = 1, bool subscribeToAllPhaseEvents = true)
        : base(subscribeToAllPhaseEvents: subscribeToAllPhaseEvents)
        {
            HandLimit = handlimit;
            CardPlayLimit = cardPlayLimit;
            Players = players;
        }

        new virtual public void StartRound()
        {
            base.StartRound();
        }

        new virtual protected void NextPhase(int phaseNum)
        {
            base.NextPhase(phaseNum);
        }

        new virtual public void EndRound()
        {
            base.EndRound();
        }

        new virtual public void SkipToPhase(int phaseNum)
        {
            base.SkipToPhase(phaseNum);
        }

        new virtual public void NextPhaseHook(int phaseNum, out bool repeatPhase) {
            repeatPhase = true;
        }
    }
}
