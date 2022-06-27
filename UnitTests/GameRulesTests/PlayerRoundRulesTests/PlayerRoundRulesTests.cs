using FluentAssertions;
using deckForge.GameRules.PlayerRoundRules;

namespace UnitTests.PlayerRoundRulesTests
{
    [TestClass]
    public class PlayerRoundRulesTests
    {
        [TestMethod]
        [DataRow(5)]
        public void getRoundHandLimit_SpecifiedLimit(int lim)
        {
            RoundTemplate rt = new(new List<Phase>());
            RoundRules rr = new RoundRules(rt: rt, handlimit: lim);
            rr.HandLimit.Should().Be(lim, "RoundRules was initiliazed with a max hand limit");
        }

        [TestMethod]
        public void getRoundHandLimit_UnSpecifiedLimit()
        {
            RoundTemplate rt = new(new List<Phase>());
            RoundRules rr = new RoundRules(rt: rt);
            rr.HandLimit.Should().Be(64, "RoundRules was initiliazed without a max hand limit");
        }

        [TestMethod]
        public void setRoundHandLimitToInvalidValue()
        {
            RoundTemplate rt = new(new List<Phase>());
            Action init = () => new RoundRules(rt: rt, handlimit: -1);
            init.Should().Throw<ArgumentException>("you can't have a negative hand limit");
        }

        [TestMethod]
        [DataRow(5)]

        public void getCardDrawonNewTurn_SpecifiedLimit(int lim)
        {
            RoundTemplate rt = new(new List<Phase>());
            RoundRules rr = new RoundRules(rt: rt, cardPlayLimit: lim);
            rr.CardPlayLimit.Should().Be(lim, "RoundRules was initiliazed with a max Card Play limit");
        }

        [TestMethod]
        public void getCardDrawonNewTurn_UnSpecifiedLimit()
        {
            RoundTemplate rt = new(new List<Phase>());
            RoundRules rr = new(rt: rt);
            rr.CardPlayLimit.Should().Be(1, "RoundRules was initiliazed with a max Card Play limit");
        }

        [TestMethod]
        public void getPhasesList()
        {
            RoundTemplate rt = new(new List<Phase>());
            RoundRules rr = new(rt: rt);
            List<Phase> ph = rr.Phases;
            ph.Count.Should().BeGreaterThanOrEqualTo(1, "there has to be at least one phase in the game");
        }
    }
}
