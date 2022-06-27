using deckForge.GameRules.PlayerRoundRules;
using FluentAssertions;

namespace UnitTests.PlayerRoundRulesTests
{
    [TestClass]
    public class RoundTemplateTests
    {

        [TestMethod]
        public void RoundTemplateGoesThroughPhasesErrorFree()
        {
            List<Phase> phases = new List<Phase> { new Phase(), new Phase(), new Phase() };
            RoundTemplate rt = new(phases);

            Action a = () => rt.StartRound();
            a.Should().NotThrow();
        }

    }
}
