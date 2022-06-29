using FluentAssertions;
using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Rounds;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PlayerConstruction;
using deckForge.GameConstruction;

namespace UnitTests.PlayerRoundRulesTests
{
    [TestClass]
    public class PlayerRoundRulesTests
    {
        [TestMethod]
        [DataRow(5)]
        public void getRoundHandLimit_SpecifiedLimit(int lim)
        {
            PlayerRoundRules rr = new PlayerRoundRules(new List<IPhase>(), new Player(new GameMediator(0)), handlimit: lim);
            rr.HandLimit.Should().Be(lim, "RoundRules was initiliazed with a max hand limit");
        }

        [TestMethod]
        public void getRoundHandLimit_UnSpecifiedLimit()
        {
            PlayerRoundRules rr = new PlayerRoundRules(new List<IPhase>(), new Player(new GameMediator(0)));
            rr.HandLimit.Should().Be(64, "RoundRules was initiliazed without a max hand limit");
        }

        [TestMethod]
        public void setRoundHandLimitToInvalidValue()
        {
            var gm = new GameMediator(0);
            Action init = () => new PlayerRoundRules(new List<IPhase>(), new Player(gm), handlimit: -2);
            init.Should().Throw<ArgumentException>("you can't have a negative hand limit (except for -1 which is no limit to card play)");
        }

        [TestMethod]
        [DataRow(5)]

        public void getCardDrawonNewTurn_SpecifiedLimit(int lim)
        {
            PlayerRoundRules rr = new PlayerRoundRules(new List<IPhase>(), new Player(new GameMediator(0)), cardPlayLimit: lim);
            rr.CardPlayLimit.Should().Be(lim, "RoundRules was initiliazed with a max Card Play limit");
        }

        [TestMethod]
        public void getCardDrawonNewTurn_UnSpecifiedLimit()
        {
            PlayerRoundRules rr = new PlayerRoundRules(new List<IPhase>(), new Player(new GameMediator(0)));
            rr.CardPlayLimit.Should().Be(1, "RoundRules was initiliazed with a max Card Play limit");
        }
    }
}
