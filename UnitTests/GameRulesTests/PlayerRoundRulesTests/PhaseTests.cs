using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.PhaseActions;
using FluentAssertions;

namespace UnitTests.PlayerRoundRulesTests
{
    [TestClass]
    public class PhaseTests {
        
        [TestMethod]
        public void SetActionOfAPhase()
        {
            List<GameAction> actions = new List<GameAction>();
            actions.Add(new DrawCardsAction());
            Phase ph = new Phase(actions);

            ph.Actions.Count.Should().Be(1, "there is an action that was added to the phase");
        }


    }
}
