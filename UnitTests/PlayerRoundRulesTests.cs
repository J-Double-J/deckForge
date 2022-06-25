using RoundRulesNamespace;
using FluentAssertions;
using PhaseNamespace;

namespace UnitTests
{
    [TestClass]
    public class PlayerRoundRulesTests
    {
        [TestMethod]
        [DataRow(5)]
        public void getRoundHandLimit_SpecifiedLimit(int lim) {
            RoundRules rr = new RoundRules(lim);
            rr.HandLimit.Should().Be(lim, "RoundRules was initiliazed with a max hand limit");
        }

        [TestMethod]
        public void getRoundHandLimit_UnSpecifiedLimit()
        {
            RoundRules rr = new RoundRules();
            rr.HandLimit.Should().Be(64, "RoundRules was initiliazed without a max hand limit");
        }

        [TestMethod]
        public void setRoundHandLimitToInvalidValue() {
            Action init = () => new RoundRules(-1);
            init.Should().Throw<ArgumentException>("you can't have a negative hand limit");
        }

        [TestMethod]
        [DataRow(5)]

        public void getCardDrawonNewTurn_SpecifiedLimit(int lim) {
            RoundRules rr = new RoundRules(cardPlayLimit: lim);
            rr.CardPlayLimit.Should().Be(lim, "RoundRules was initiliazed with a max Card Play limit");
        }

        [TestMethod]
        public void getCardDrawonNewTurn_UnSpecifiedLimit()
        {
            RoundRules rr = new();
            rr.CardPlayLimit.Should().Be(1, "RoundRules was initiliazed with a max Card Play limit");
        }

        [TestMethod]
        public void getPhasesList() {
            RoundRules rr = new();
            List<Phase> ph = rr.Phases;
            ph.Count.Should().BeGreaterThanOrEqualTo(1, "there has to be at least one phase in the game");
        }
    }
}
