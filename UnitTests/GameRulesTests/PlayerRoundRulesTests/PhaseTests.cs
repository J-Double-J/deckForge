using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.PhaseActions;
using deckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.PlayerRoundRulesTests
{
    [TestClass]
    public class PhaseTests
    {

        [TestMethod]
        public void SetActionOfAPhase()
        {
            List<IAction<Player>> actions = new List<IAction<Player>>();
            actions.Add(new DrawCardsAction());
            BasePhase<Player> ph = new BasePhase<Player>(actions);

            ph.ActionCount.Should().Be(1, "there is an action that was added to the phase");
        }


    }
}
