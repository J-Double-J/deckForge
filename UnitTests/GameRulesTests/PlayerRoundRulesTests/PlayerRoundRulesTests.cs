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
            List<IPlayer> players,  int handlimit = 64, int cardPlayLimit = 1, bool subscribeToAllPhaseEvents = true)
            : base(gm, players: players, handlimit: handlimit, cardPlayLimit: cardPlayLimit, subscribeToAllPhaseEvents: subscribeToAllPhaseEvents) { }
    }

    [TestClass]
    public class PlayerRoundRulesTests
    {
        [TestMethod]
        [DataRow(5)]
        public void getRoundHandLimit_SpecifiedLimit(int lim)
        {
            BaseGameMediator gm = new(0);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 0, decks);
            List<IPlayer> players = new List<IPlayer> { new BasePlayer(gm) };
            PlayerRoundRules rr = new TestPlayerRoundRules(gm, players, handlimit: lim);
            rr.HandLimit.Should().Be(lim, "RoundRules was initiliazed with a max hand limit");
        }

        [TestMethod]
        public void getRoundHandLimit_UnSpecifiedLimit()
        {
            List<IPlayer> players = new();
            BaseGameMediator gm = new(0);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 0, decks);
            players.Add(new BasePlayer(gm));
            PlayerRoundRules rr = new TestPlayerRoundRules(gm, players);
            rr.HandLimit.Should().Be(64, "RoundRules was initiliazed without a max hand limit");
        }

        [TestMethod]
        public void setRoundHandLimitToInvalidValue()
        {
            List<IPlayer> players = new();
            BaseGameMediator gm = new(0);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 0, decks);
            players.Add(new BasePlayer(gm));
            Action init = () => new TestPlayerRoundRules(gm, players, handlimit: -2);
            init.Should().Throw<ArgumentException>("you can't have a negative hand limit (except for -1 which is no limit to card play)");
        }

        [TestMethod]
        [DataRow(5)]

        public void getCardDrawonNewTurn_SpecifiedLimit(int lim)
        {
            List<IPlayer> players = new();
            BaseGameMediator gm = new(0);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 0, decks);
            players.Add(new BasePlayer(gm));
            PlayerRoundRules rr = new TestPlayerRoundRules(gm, players, cardPlayLimit: lim);
            rr.CardPlayLimit.Should().Be(lim, "RoundRules was initiliazed with a max Card Play limit");
        }

        [TestMethod]
        public void getCardDrawonNewTurn_UnSpecifiedLimit()
        {
            List<IPlayer> players = new();
            BaseGameMediator gm = new(0);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 0, decks);
            players.Add(new BasePlayer(gm));
            PlayerRoundRules rr = new TestPlayerRoundRules(gm, players);
            rr.CardPlayLimit.Should().Be(1, "RoundRules was initiliazed with a max Card Play limit");
        }
    }
}
