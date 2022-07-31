using DeckForge.GameConstruction;
using DeckForge.GameRules.RoundConstruction.Interfaces;

namespace DeckForge.GameRules.RoundConstruction.Phases
{
    /// <summary>
    /// Non-Player-Controlled Phases are <see cref="IPhase"/>s that do not involve <see cref="IPlayer"/>s
    /// direct involvement and happens mostly outside of their control.
    /// </summary>
    /// <typeparam name="T">Type of object that will be controlled by the <see cref="NPCPhase{T}"/>.</typeparam>
    public class NPCPhase<T> : BasePhase<T>, INPCPhase<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPCPhase{T}"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that <see cref="NPCPhase{T}"/> will use to communicate
        /// with other game elements.</param>
        /// <param name="phaseName">Name of the phase.</param>
        public NPCPhase(IGameMediator gm, string phaseName = "")
            : base(gm, phaseName: phaseName)
        {
        }

        /// <summary>
        /// Starts the phase with <paramref name="t"/> being targeted.
        /// </summary>
        /// <param name="t">Object involved in the <see cref="NPCPhase{T}"/>.</param>
        public virtual void StartPhase(T t)
        {
            DoPhaseActions(t);
        }

        /// <summary>
        /// Executes all the <see cref="PhaseActions.IGameAction{T}"/> in order on <paramref name="t"/>.
        /// </summary>
        /// <param name="t">The object that executes or targetted by the <see cref="PhaseActions.IGameAction{T}"/>s.</param>
        protected virtual void DoPhaseActions(T t)
        {
            for (var actionNum = 0; actionNum < Actions.Count; actionNum++)
            {
                if (!PhaseActionLogic())
                {
                    Actions[actionNum].Execute(t);
                }
            }

            EndPhase();
        }

        /// <summary>
        /// Any logic or extra function calls should be overriden here and will be called
        /// before each <see cref="PhaseActions.IGameAction{T}"/>.
        /// </summary>
        /// <returns>
        /// Returns true if an <see cref="PhaseActions.IGameAction{T}"/> is handled in this function call, else false.
        /// </returns>
        protected virtual bool PhaseActionLogic()
        {
            return false;
        }
    }
}
