using deckForge.PlayerConstruction;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PhaseActions;
using deckForge.GameConstruction;


namespace deckForge.GameRules.RoundConstruction.Rounds
{
    abstract public class PlayerRoundRules : BaseRoundRules, IRoundRules
    {
        abstract override public List<IPhase> Phases { get; }
        int _handLim;

        public PlayerRoundRules(IGameMediator gm, List<int> players, int handlimit = 64, int cardPlayLimit = 1)
        : base(gm)
        {
            HandLimit = handlimit;
            CardPlayLimit = cardPlayLimit;
            PlayerIDs = players;
        }

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
        public List<int> PlayerIDs { get; }

        new virtual public void StartRound()
        {
            CurPhase = 0;
            NextPhase(0);
        }

        override protected void NextPhase(int phaseNum)
        {
            NextPhaseHook(phaseNum, out bool handledPhase);
            if (!handledPhase) {
                Phases[phaseNum].StartPhase();
            }
            //If a round is ended early by a phase, CurPhase will be -1
            if (CurPhase >= 0) {
                CurPhase++;
                if (!(CurPhase > Phases.Count - 1))
                {
                    NextPhase(CurPhase);
                }
                else
                {
                    EndRound();
                }
            } else {
                EndRound();
            }
        }

        new virtual public void SkipToPhase(int phaseNum)
        {
            base.SkipToPhase(phaseNum);
        }

        new virtual public void NextPhaseHook(int phaseNum, out bool handledPhase)
        {
            handledPhase = false;
        }
    }
}
