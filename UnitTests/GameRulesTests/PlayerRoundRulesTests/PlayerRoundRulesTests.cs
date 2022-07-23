using FluentAssertions;
using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Rounds;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PlayerConstruction;
using deckForge.GameConstruction;
using deckForge.GameElements;
using deckForge.GameElements.Resources;

namespace UnitTests.PlayerRoundRulesTests
{
    internal class TestPlayerRoundRules : PlayerRoundRules
    {
        public List<IPhase> testPhases = new();
        public IGameMediator gm = new BaseGameMediator(0);
        override public List<IPhase> Phases { get { return testPhases; } }
        public TestPlayerRoundRules(IGameMediator gm,
            List<int> players,  int handlimit = 64, int cardPlayLimit = 1, bool subscribeToAllPhaseEvents = true)
            : base(gm, players: players, handlimit: handlimit, cardPlayLimit: cardPlayLimit) { }
    }

    [TestClass]
    public class PlayerRoundRulesTests
    {
        [TestMethod]
        [DataRow(5)]
        public void getRoundHandLimit_SpecifiedLimit(int lim)
        {
            BaseGameMediator gm = new(1);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 1, decks);
            IPlayer player = new BasePlayer(gm);
            List<int> playerIDs = new() { 0 }; 
            PlayerRoundRules rr = new TestPlayerRoundRules(gm, playerIDs, handlimit: lim);
            rr.HandLimit.Should().Be(lim, "RoundRules was initiliazed with a max hand limit");
        }

        [TestMethod]
        public void getRoundHandLimit_UnSpecifiedLimit()
        {
            BaseGameMediator gm = new(1);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 1, decks);
            IPlayer player = new BasePlayer(gm);
            List<int> playerIDs = new() { 0 };
            PlayerRoundRules rr = new TestPlayerRoundRules(gm, playerIDs);
            rr.HandLimit.Should().Be(64, "RoundRules was initiliazed without a max hand limit");
        }

        [TestMethod]
        public void setRoundHandLimitToInvalidValue()
        {
            BaseGameMediator gm = new(1);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 1, decks);
            IPlayer player = new BasePlayer(gm);
            List<int> playerIDs = new() { 0 };
            Action init = () => new TestPlayerRoundRules(gm, playerIDs, handlimit: -2);
            init.Should().Throw<ArgumentException>("you can't have a negative hand limit (except for -1 which is no limit to card play)");
        }

        [TestMethod]
        [DataRow(5)]

        public void getCardDrawonNewTurn_SpecifiedLimit(int lim)
        {
            
            BaseGameMediator gm = new(1);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 1, decks);
            IPlayer player = new BasePlayer(gm);
            List<int> playerIDs = new() { 0 };
            PlayerRoundRules rr = new TestPlayerRoundRules(gm, playerIDs, cardPlayLimit: lim);
            rr.CardPlayLimit.Should().Be(lim, "RoundRules was initiliazed with a max Card Play limit");
        }

        [TestMethod]
        public void getCardDrawonNewTurn_UnSpecifiedLimit()
        {
            BaseGameMediator gm = new(1);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 1, decks);
            IPlayer player = new BasePlayer(gm);
            List<int> playerIDs = new() { 0 };
            PlayerRoundRules rr = new TestPlayerRoundRules(gm, playerIDs);
            rr.CardPlayLimit.Should().Be(1, "RoundRules was initiliazed with a max Card Play limit");
        }
    }
}
