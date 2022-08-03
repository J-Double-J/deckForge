using DeckForge.GameConstruction;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.PhaseActions;

namespace DeckForge.GameRules.RoundConstruction.Phases
{
    /// <summary>
    /// BasePhase where every element in <see cref="Actions"/> is of type <see cref="IGameAction{T}"/>s.
    /// </summary>
    /// <typeparam name="T">The common object that is targetted by every <see cref="IGameAction{T}"/> in this
    /// <see cref="IPhase"/>.</typeparam>
    public abstract class BasePhase<T> : BasePhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePhase{T}"/> class.
        /// </summary>
        /// <param name="gm">GameMediator that the <see cref="IPhase"/> will use to communicate with the
        /// other elements in the game.</param>
        /// <param name="phaseName">Name of the <see cref="IPhase"/>.</param>
        public BasePhase(IGameMediator gm, string phaseName = "")
            : base(gm, phaseName)
        {
            Actions = new();
            CurrentAction = 0;
        }

        /// <inheritdoc/>
        public override int ActionCount
        {
            get
            {
                return Actions.Count;
            }
        }

        /// <summary>
        /// Gets or Sets list of <see cref="IGameAction{T}"/> that the <see cref="IPhase"/> manages.
        /// </summary>
        protected new List<IGameAction<T>> Actions
        {
            get;
            set;
        }
    }
}
