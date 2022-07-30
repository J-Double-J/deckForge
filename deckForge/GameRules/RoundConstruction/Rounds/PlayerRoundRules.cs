using deckForge.PlayerConstruction;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.GameConstruction;
using deckForge.GameRules.RoundConstruction.Phases;

namespace deckForge.GameRules.RoundConstruction.Rounds
{
    /// <summary>
    /// Base class for all Rounds involving <see cref= "IPlayer"/>s. 
    /// Outlines <see cref="IPhase"/>s and the algorithm for <see cref="IPhase"/> looping.
    /// </summary>
    public abstract class PlayerRoundRules : BaseRoundRules, IRoundRules
    {
        private int _handLim;
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

        public override void StartRound()
        {
            List<int> newTurnOrder = GM.TurnOrder;
            if (PlayerTurnOrder != newTurnOrder)
            {
                PlayerTurnOrder = newTurnOrder;
                UpdatePhasesPlayerTurnOrder(newTurnOrder); //TODO: Evaluate if this is needed
            }

            for (CurPhase = 0; CurPhase < Phases.Count; CurPhase++)
            {
                if (!NextPhaseHook(CurPhase))
                {
                    Phases[CurPhase].StartPhase();
                }
                if (CurPhase < 0)
                {
                    EndRound();
                    break;
                }
            }

            if (!(CurPhase < 0))
            {
                EndRound();
            }
        }

        public override void SkipToPhase(int phaseNum)
        {
            base.SkipToPhase(phaseNum);
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

        /// <summary>
        /// Updates the Player Turn Order to the <paramref name="newPlayerList"/> for the Round
        /// and all <see cref="IPhase"/>s managed by it.
        /// </summary>
        /// <param name="newPlayerList"></param>
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
