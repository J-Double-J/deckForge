﻿using FluentAssertions;
using deckForge.GameRules.RoundConstruction.Phases;
using deckForge.GameRules.RoundConstruction.Rounds;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PlayerConstruction;
using deckForge.GameConstruction;
using deckForge.GameElements;
using deckForge.GameElements.Resources;
using deckForge.PhaseActions;
using deckForge.GameConstruction.PresetGames.War;

namespace UnitTests.PlayerRoundRulesTests
{
    public class TestPlayerRoundRules : PlayerRoundRules
    {
        public IGameMediator gm = new BaseGameMediator(0);
        public TestPlayerRoundRules(IGameMediator gm,
            List<int> players, int handlimit = 64, int cardPlayLimit = 1, bool subscribeToAllPhaseEvents = true)
            : base(gm, players: players, handlimit: handlimit, cardPlayLimit: cardPlayLimit) { }
    }

    [TestClass]
    public class PlayerRoundRulesTests
    {
        [TestMethod]
        [DataRow(5)]
        public void GetRoundHandLimit_SpecifiedLimit(int lim)
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
        public void GetRoundHandLimit_UnSpecifiedLimit()
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
        public void SetRoundHandLimitToInvalidValue()
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

        public void GetCardDrawonNewTurn_SpecifiedLimit(int lim)
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
        public void GetCardDrawonNewTurn_UnSpecifiedLimit()
        {
            BaseGameMediator gm = new(1);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 1, decks);
            IPlayer player = new BasePlayer(gm);
            List<int> playerIDs = new() { 0 };
            PlayerRoundRules rr = new TestPlayerRoundRules(gm, playerIDs);
            rr.CardPlayLimit.Should().Be(1, "RoundRules was initiliazed with a max Card Play limit");
        }

        [TestMethod]
        public void RoundLosePlayerMidWayThrough()
        {
            IGameMediator gm = new TestGameMediator(3);
            IGameController gc = new BaseGameController(3);
            Table table = new(gm, 3, new Deck());
            gm.RegisterGameController(gc);

            List<int> playerIDs = new();
            List<IPlayer> players = new();
            for (var i = 0; i < 3; i++)
            {
                players.Add(new WarPlayer(gm, i, new Deck()));
                playerIDs.Add(i);
            }

            List<int> targetPlayOrder = new List<int>() { 0, 2 };

            PlayerRoundRules round = new TestRoundWithLosingPlayer(gm, playerIDs);

            round.StartRound();
            var test = round;

            table.TableState[0].Count.Should().Be(4, "player 1 played cards 4 times");
            table.TableState[1].Count.Should().Be(0, "all cards were removed from the table for the outted player");
            table.TableState[2].Count.Should().Be(4, "player 2 played cards 4 times");
            round.PlayerTurnOrder.Should().BeEquivalentTo(targetPlayOrder);
            gm.TurnOrder.Should().BeEquivalentTo(targetPlayOrder);
        }

        [TestMethod]
        public void RoundEndsEarlyOnceAllPlayersButOneAreOut()
        {
            IGameMediator gm = new TestGameMediator(2);
            IGameController gc = new BaseGameController(2);
            Table table = new(gm, 2, new Deck());
            gm.RegisterGameController(gc);

            List<int> playerIDs = new();
            List<IPlayer> players = new();
            for (var i = 0; i < 2; i++)
            {
                players.Add(new WarPlayer(gm, i, new Deck()));
                playerIDs.Add(i);
            }

            PlayerRoundRules round = new TestRoundWithLosingPlayer(gm, playerIDs);

            round.StartRound();

            table.TableState[0].Count.Should().Be(2, "game ended before player could play 2 more cards as normal in a round");
        }
    }

    internal class TestWarPlayerPlayCardsOnTablePhase : PlayerPhase
    {
        public TestWarPlayerPlayCardsOnTablePhase(IGameMediator gm, List<int> playerIDs, string phaseName = "Test Phase")
        : base(gm, playerIDs, phaseName)
        {
            Actions.Add(new PlayCardsAction());
            Actions.Add(new PlayCardsAction());
        }
    }

    internal class TestRoundWithLosingPlayer : PlayerRoundRules
    {
        public TestRoundWithLosingPlayer(IGameMediator gm, List<int> playerIDs)
        : base(gm, playerIDs)
        {
            Phases = new List<IPhase>()
            {
                new TestWarPlayerPlayCardsOnTablePhase((TestGameMediator)gm, playerIDs, "TestName"),
                new PlayerTwoLosesInPhase((TestGameMediator)gm, PlayerIDs, "TestName"),
                new TestWarPlayerPlayCardsOnTablePhase((TestGameMediator)gm, playerIDs, "TestName")
            };
        }
    }
}
