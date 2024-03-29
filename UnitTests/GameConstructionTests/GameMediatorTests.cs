﻿using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.War;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.GameRules.RoundConstruction.Rounds;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.GameConstructionTests
{
    [TestClass]
    public class GameMediatorTests
    {
        private static StringWriter output = new();

        [TestMethod]
        public void GetPlayerByID_ThrowsOnInvalidID()
        {
            IGameMediator gm = new BaseGameMediator(2);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 1, decks);
            Table table = new(gm, new List<TableZone>() { zone });
            new BasePlayer(gm, 0);
            new BasePlayer(gm, 1);

            IPlayer? foundPlayer = gm.GetPlayerByID(3);

            foundPlayer.Should().BeNull("an invalid player ID was passed to the GameMediator");
        }

        [TestMethod]
        public void GameMediatorCanDrawCard()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });

            ICard card = gm.DrawCardFromDeck(TablePlacementZoneType.PlayerZone)!;

            card.Should().NotBeNull("a new deck was created so it should have cards");
        }

        [TestMethod]
        public void GameMediatorCannotDrawFromEmptyDeck()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });

            ICard? card;

            for (var i = 0; i < 52; i++)
            {
                card = gm.DrawCardFromDeck(TablePlacementZoneType.PlayerZone)!;
            }

            card = gm.DrawCardFromDeck(TablePlacementZoneType.PlayerZone);

            card.Should().BeNull("the deck was exhausted and there are no more cards to draw");
        }

        [TestMethod]
        public void InitiateGameMediatorWithNegativePlayers()
        {
            Action a = () => new BaseGameMediator(-1);

            a.Should().Throw<ArgumentException>("a game cannot have negative players");
        }

        [TestMethod]
        public void InitiateGameMediatorWithTooManyPlayers()
        {
            Action a = () => new BaseGameMediator(13);

            a.Should().Throw<ArgumentException>("a game cannot have more than 12 players at the moment");
        }

        [TestMethod]
        public void GameMediatorCanTellPlayerToExecuteGameAction()
        {
            IGameMediator gm = new BaseGameMediator(1);
            IPlayer player = new BasePlayer(gm);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 1, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            PlayerGameAction action = new DrawCardsAction(TablePlacementZoneType.PlayerZone);

            gm.TellPlayerToDoAction(0, action);

            player.HandSize.Should().Be(1, "the player drew a card into their hand");
        }

        [TestMethod]
        public void GameMediatorCannotTellPlayer_ToExecuteInvalidGameAction()
        {
            IGameMediator gm = new BaseGameMediator(2);
            IPlayer player = new BasePlayer(gm, 0);
            IPlayer target = new BasePlayer(gm, 1);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            PlayerGameAction action = new DrawCardsAction(TablePlacementZoneType.PlayerZone);

            Action a = () => gm.TellPlayerToDoActionAgainstAnotherPlayer(0, 1, action);

            a.Should().Throw<NotSupportedException>("draw cannot be targetted against another player");
        }

        [TestMethod]
        public void GameMediatorLoopsThroughRoundsCorrectlyAndEndsWithWinner()
        {
            Console.SetOut(output);
            IGameMediator gm = new BaseGameMediator(2);
            ITurnHandler th = new TurnHandler(2, false);
            gm.RegisterTurnHandler(th);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });

            List<int> playerIDs = new();
            List<IPlayer> players = new();
            for (var i = 0; i < 2; i++)
            {
                players.Add(new WarPlayer(gm, i, new DeckOfPlayingCards()));
                playerIDs.Add(i);
            }

            new TestRoundWithTwoPlayCardsActions(gm, playerIDs);
            new RoundWithArbitraryLosingRules(gm, playerIDs);

            gm.StartGame();

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("Player 1 wins!\n");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("Player 1 wins!\r\n");
            }
        }

        [TestMethod]
        public void GameMediatorCanDealCardsToPlayers()
        {
            IGameMediator gm = new BaseGameMediator(1);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 3, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            List<IPlayer> players = new()
            {
                new BasePlayer(gm, 0),
                new BasePlayer(gm, 1),
                new BasePlayer(gm, 2)
            };

            gm.DealCardsFromDeckToAllPlayers(2, TablePlacementZoneType.PlayerZone);

            foreach (IPlayer player in players)
            {
                player.HandSize.Should().Be(2, "because each player should have been dealt two cards");
            }
        }

        [TestMethod]
        public void GameMediatorDealsAsManyCardsAsItCanToPlayers()
        {
            IGameMediator gm = new BaseGameMediator(1);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 3, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            List<IPlayer> players = new()
            {
                new BasePlayer(gm, 0),
                new BasePlayer(gm, 1),
                new BasePlayer(gm, 2)
            };

            gm.DealCardsFromDeckToAllPlayers(20, TablePlacementZoneType.PlayerZone);

            players[0].HandSize.Should().Be(18, "there were not enough cards to get to 20 and the last extra card was dealt to the first player");
            players[1].HandSize.Should().Be(17, "there were not enough cards to get to 20");
            players[2].HandSize.Should().Be(17, "there were not enough cards to get to 20");
        }
    }

    internal class PlayerLosesIfPlayer0AndHasAtLeast3CardsAction : PlayerGameAction
    {
        private IGameMediator gm;

        public PlayerLosesIfPlayer0AndHasAtLeast3CardsAction(
            IGameMediator gm,
            string name = "PlayerLosesIfPlayer0AndHas3CardsAction",
            string description = "Very specific test action")
            : base(name, description)
        {
            this.gm = gm;
        }

        public override object? Execute(IPlayer player)
        {
            var tableState = gm.CurrentTableState;
            if (player.PlayerID == 0 && tableState[0].Count >= 3)
            {
                player.LoseGame();
            }

            return null;
        }
    }

    internal class CheckIfPlayerLosesArbitrarilyPhase : PlayerPhase
    {
        public CheckIfPlayerLosesArbitrarilyPhase(
            IGameMediator gm,
            List<int> playerIDs,
            string name = "CheckIfPlayerLosesArbitrarilyPhase")
            : base(gm, playerIDs, name)
        {
            Actions.Add(new PlayerLosesIfPlayer0AndHasAtLeast3CardsAction(gm));
        }
    }

    internal class RoundWithArbitraryLosingRules : PlayerRoundRules
    {
        public RoundWithArbitraryLosingRules(
            IGameMediator gm,
            List<int> playerIDs)
            : base(gm, playerIDs)
        {
            Phases.Add(new CheckIfPlayerLosesArbitrarilyPhase(gm, playerIDs));
        }
    }

    internal class TestRoundWithTwoPlayCardsActions : PlayerRoundRules
    {
        public TestRoundWithTwoPlayCardsActions(IGameMediator gm, List<int> playerIDs)
            : base(gm, playerIDs)
        {
            Phases.Add(new TestPhaseWithTwoPlayCardsActions(gm, playerIDs));
        }
    }

    internal class TestPhaseWithTwoPlayCardsActions : PlayerPhase
    {
        public TestPhaseWithTwoPlayCardsActions(IGameMediator gm, List<int> playerIDs)
            : base(gm, playerIDs)
        {
            Actions.Add(new PlayMultipleCardsAction(2));
        }
    }
}
