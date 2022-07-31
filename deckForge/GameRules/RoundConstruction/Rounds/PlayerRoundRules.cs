using DeckForge.GameConstruction;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameRules.RoundConstruction.Rounds
{
    /// <summary>
    /// Base class for all Rounds involving <see cref= "IPlayer"/>s.
    /// Outlines <see cref="IPhase"/>s and the algorithm for <see cref="IPhase"/> looping.
    /// </summary>
    public abstract class PlayerRoundRules : BaseRoundRules, IRoundRules
    {
        private int handLim;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRoundRules"/> class involving <see cref="IPlayer"/>s.
        /// </summary>
        /// <param name="gm">Gamemediator that the Round uses to communicate with other objects.</param>
        /// <param name="players">List of the <see cref="IPlayer"/> IDs.</param>
        /// <param name="handlimit">Hand limit of each <see cref="IPlayer"/> in the game.</param>
        /// <param name="cardPlayLimit">Card play limit of each <see cref="IPlayer"/> in the game.</param>
        public PlayerRoundRules(IGameMediator gm, List<int> players, int handlimit = 64, int cardPlayLimit = 1)
        : base(gm)
        {
            HandLimit = handlimit;
            CardPlayLimit = cardPlayLimit;
            PlayerIDs = players;
            PlayerTurnOrder = players;
        }

        /// <summary>
        /// Gets hand limit of <see cref="IPlayer"/>s in the Round.
        /// A return of -1 is no hand limit.
        /// </summary>
        public int HandLimit
        {
            get
            {
                return handLim;
            }

            private set
            {
                if (value >= -1)
                {
                    this.handLim = value;
                }
                else
                {
                    throw new ArgumentException("Hand Limit cannot be set to lower than 0.", "HandLimit");
                }
            }
        }

        /// <summary>
        /// Gets the limit of card plays an <see cref="IPlayer"/> can execute.
        /// A return of -1 is no card play limit.
        /// </summary>
        public int CardPlayLimit { get; private set; }

        /// <summary>
        /// Gets the IDs of the <see cref="IPlayer"/>s managed by the Round.
        /// </summary>
        public List<int> PlayerIDs { get; }

        /// <summary>
        /// Gets the turns order of the <see cref="IPlayer"/>s in the Round.
        /// </summary>
        public List<int> PlayerTurnOrder
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the turn order that is queued to be updated once an <see cref="IPhase"/> ends.
        /// </summary>
        protected List<int>? ToBeUpdatedTurnOrder
        {
            get; set;
        }

        /// <inheritdoc/>
        public override void StartRound()
        {
            List<int> newTurnOrder = GM.TurnOrder;
            if (PlayerTurnOrder != newTurnOrder)
            {
                PlayerTurnOrder = newTurnOrder;
                UpdatePhasesPlayerTurnOrder(newTurnOrder); // TODO: Evaluate if this is needed
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

        /// <inheritdoc/>
        public override void SkipToPhase(int phaseNum)
        {
            base.SkipToPhase(phaseNum);
        }

        /// <summary>
        /// Updates each <see cref="IPhase"/>'s turn order with the <paramref name="newTurnOrder"/>.
        /// <para>Each <see cref="IPhase"/> must be of <see cref="Type"/> <see cref="PlayerPhase"/>.</para>
        /// </summary>
        /// <param name="newTurnOrder">Turn order of <see cref="IPlayer"/> by their IDs.</param>
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
        /// <param name="newPlayerList">New list of IDs of <see cref="IPlayer"/>s who are still in the game.</param>
        public void UpdatePlayerList(List<int> newPlayerList)
        {
            PlayerTurnOrder = PlayerTurnOrder.Intersect(newPlayerList).ToList();

            foreach (IPhase phase in Phases)
            {
                if (phase is IPlayerPhase playerPhase)
                {
                    playerPhase.UpdateTurnOrder(PlayerTurnOrder);
                }
            }
        }

        /// <inheritdoc/>
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
