using DeckForge.GameConstruction;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameRules.RoundConstruction.Phases.PhaseEventArgs;
using DeckForge.PhaseActions;

namespace DeckForge.GameRules.RoundConstruction.Phases
{
    /// <summary>
    /// Base class for all <see cref="IPhase"/>s that outlines functions for any <see cref="IPhase"/>.
    /// All <see cref="IPhase"/>s are required to inherit from this class.
    /// </summary>
    /// <typeparam name="T">The type of object that is targetted by the <see cref="IPhase"/>. This is
    /// primarily for <see cref="IPhase"/>s that have <see cref="IGameAction{T}"/> that target types of objects.</typeparam>
    public abstract class BasePhase<T> : IPhase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasePhase{T}"/> class.
        /// </summary>
        /// <param name="gm">GameMediator that the <see cref="IPhase"/> will use to communicate with the
        /// other elements in the game.</param>
        /// <param name="phaseName">Name of the <see cref="IPhase"/>.</param>
        public BasePhase(IGameMediator gm, string phaseName = "")
        {
            Actions = new ();
            PhaseName = phaseName;
            GM = gm;
            CurrentAction = 0;
        }

        /// <inheritdoc/>
        public event EventHandler<SkipToPhaseEventArgs>? SkipToPhase;

        /// <inheritdoc/>
        public event EventHandler<EndRoundEarlyArgs>? EndRoundEarly;

        /// <inheritdoc/>
        public event EventHandler<PhaseEndedArgs>? PhaseEnded;

        /// <inheritdoc/>
        public string PhaseName
        {
            get;
        }

        /// <summary>
        /// Gets the number of Actions that the <see cref="IPhase"/> manages.
        /// </summary>
        public int ActionCount
        {
            get
            {
                return Actions.Count;
            }
        }

        /// <summary>
        /// Gets the GameMediator that the <see cref="IPhase"/> uses to interact with other game elements.
        /// </summary>
        protected IGameMediator GM
        {
            get;
        }

        /// <summary>
        /// Gets or sets the CurrentAction index as the <see cref="IPhase"/> iterates through its <see cref="IGameAction{T}"/>s.
        /// </summary>
        protected int CurrentAction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets list of <see cref="IGameAction{T}"/> that the <see cref="IPhase"/> manages.
        /// </summary>
        protected List<IGameAction<T>> Actions
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public virtual void StartPhase()
        {
        }

        /// <inheritdoc/>
        public virtual void EndPhase()
        {
            CurrentAction = -1;
        }

        /// <inheritdoc/>
        public void EndPhaseEarly()
        {
            CurrentAction = -1;
        }

        /// <summary>
        /// Invokes <see cref="SkipToPhase"/> and sets the CurrentAction to -1 to end the <see cref="IPhase"/>.
        /// </summary>
        /// <param name="e">Arguments for skipping to an <see cref="IPhase"/>.</param>
        protected virtual void OnSkipToPhase(SkipToPhaseEventArgs e)
        {
            CurrentAction = -1;
            SkipToPhase?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes <see cref="EndRoundEarly"/> and ends the Round prematurely.
        /// </summary>
        /// <param name="e">Arguments for ending the Round early.</param>
        protected virtual void OnEndRoundEarly(EndRoundEarlyArgs e)
        {
            EndRoundEarly?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes <see cref="PhaseEnded"/> and informs interested subscribers that the <see cref="IPhase"/>
        /// finished iterating through its <see cref="IGameAction{T}"/>s.
        /// </summary>
        /// <param name="e">Arguments for the <see cref="IPhase"/> ending.</param>
        protected virtual void OnPhaseEnded(PhaseEndedArgs e)
        {
            PhaseEnded?.Invoke(this, e);
        }
    }
}
