using deckForge.PlayerConstruction;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PhaseActions;
using deckForge.GameConstruction;
using deckForge.GameRules.RoundConstruction.Phases;

namespace deckForge.GameRules.RoundConstruction.Rounds
{
    abstract public class PlayerRoundRules : BaseRoundRules, IRoundRules
    {
        private int _handLim;
        abstract override public List<IPhase> Phases { get; }
        protected List<int>? ToBeUpdatedTurnOrder;

        public PlayerRoundRules(IGameMediator gm, List<int> players, int handlimit = 64, int cardPlayLimit = 1)
        : base(gm)
        {
            HandLimit = handlimit;
            CardPlayLimit = cardPlayLimit;
            PlayerIDs = players;
            PlayerTurnOrder = players;
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
        public List<int> PlayerTurnOrder
        {
            get;
            private set;
        }

        new virtual public void StartRound()
        {
            List<int> newTurnOrder = GM.TurnOrder;
            if (PlayerTurnOrder != newTurnOrder)
            {
                PlayerTurnOrder = GM.TurnOrder;
                UpdatePhasesPlayerTurnOrder(newTurnOrder);
            }

            CurPhase = 0;
            NextPhase(0);
        }

        override protected void NextPhase(int phaseNum)
        {
            NextPhaseHook(phaseNum, out bool handledPhase);
            if (!handledPhase)
            {
                Phases[phaseNum].StartPhase();
            }
            //If a round is ended early by a phase, CurPhase will be -1
            if (CurPhase >= 0)
            {
                CurPhase++;
                if (!(CurPhase > Phases.Count - 1))
                {
                    NextPhase(CurPhase);
                }
                else
                {
                    EndRound();
                }
            }
            else
            {
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

        /// <summary>
        /// Updates each <see cref="IPhase"/>'s turn order with the <paramref name="newTurnOrder"/>.
        /// <para>Each <see cref="IPhase"/> must be of <see cref="Type"/> <see cref="PlayerPhase"/>.</para>
        /// </summary>
        /// <param name="newTurnOrder">Turn order of <see cref="IPlayer"/> by their IDs</param>
        public void UpdatePhasesPlayerTurnOrder(List<int> newTurnOrder)
        {
            ToBeUpdatedTurnOrder = newTurnOrder;
            foreach (IPhase phase in Phases)
            {
                var playerPhase = (PlayerPhase)phase;
                playerPhase.UpdateTurnOrder(newTurnOrder);
            }
        }

        public void UpdatePlayerList(List<int> newPlayerList)
        {
            PlayerTurnOrder = PlayerTurnOrder.Intersect(newPlayerList).ToList();

            foreach (IPhase phase in Phases)
            {
                if (phase is IPlayerPhase)
                {
                    IPlayerPhase playerPhase = (IPlayerPhase)phase;
                    playerPhase.UpdateTurnOrder(PlayerTurnOrder);
                }
            }
        }

        protected override void PhaseEndedEvent(object? sender, PhaseEndedArgs e)
        {
            base.PhaseEndedEvent(sender, e);
            if (ToBeUpdatedTurnOrder is not null)
            {
                PlayerTurnOrder = ToBeUpdatedTurnOrder;
                ToBeUpdatedTurnOrder = null;
            }
        }
    }
}
