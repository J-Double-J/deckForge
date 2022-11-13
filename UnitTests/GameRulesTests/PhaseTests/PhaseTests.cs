using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.War;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.PlayerRoundRulesTests
{
    [TestClass]
    public class PhaseTests
    {
        [TestMethod]
        public void PlayerPhaseCanChangeTurnOrder()
        {
            IGameMediator gm = new TestGameMediator(3);
            ITurnHandler th = new TurnHandler(3, false);
            gm.RegisterTurnHandler(th);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 3, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });


            List<int> playerIDs = new();
            List<IPlayer> players = new();
            for (var i = 0; i < 3; i++)
            {
                players.Add(new WarPlayer(gm, i, new DeckOfPlayingCards()));
                playerIDs.Add(i);
            }

            PlayerPhase playerPhase = new TestPhase((TestGameMediator)gm, playerIDs, "Test Phase");

            playerPhase.StartPhase();
            TestGameMediator tgm = (TestGameMediator)gm;
            tgm.PhaseEnded();
            playerPhase.UpdateTurnOrder(gm.TurnOrder);
            playerPhase.StartPhase();

            playerPhase.PlayerTurnOrder.Should().BeEquivalentTo(gm.TurnOrder, "the turn order of the phase should be what GameMediator says it is.");
            foreach (IPlayer player in players)
            {
                player.HandSize.Should().Be(6, "each player was told to draw 3 cards twice");
            }
        }

        [TestMethod]
        public void PlayerPhaseLosesPlayerMidPhase()
        {
            IGameMediator gm = new TestGameMediator(3);
            ITurnHandler th = new TurnHandler(3, false);
            gm.RegisterTurnHandler(th);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 3, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });

            List<int> playerIDs = new();
            List<IPlayer> players = new();
            for (var i = 0; i < 3; i++)
            {
                players.Add(new WarPlayer(gm, i, new DeckOfPlayingCards()));
                playerIDs.Add(i);
            }

            List<int> targetPlayOrder = new() { 0, 2 };

            PlayerPhase playerPhase = new PlayerTwoLosesInPhase((TestGameMediator)gm, playerIDs, "Test Phase");

            playerPhase.StartPhase();

            table.TableState.Count.Should().Be(3, "even though a player is out, their table spot remains");
            table.TableState[1].Count.Should().Be(0, "all cards belonging to Player 2 should've been picked up");
            gm.TurnOrder.Should().BeEquivalentTo(targetPlayOrder);
            playerPhase.PlayerTurnOrder.Should().BeEquivalentTo(targetPlayOrder);
        }
    }

    // Test Objects
    internal class TestPhase : PlayerPhase
    {
        public TestPhase(TestGameMediator gm, List<int> playerIDs, string name) : base(gm, playerIDs, name)
        {
            Actions.Add(new DrawCardsAction(TablePlacementZoneType.PlayerZone));
            Actions.Add(new DrawCardsAction(TablePlacementZoneType.PlayerZone));
            Actions.Add(new DrawCardsAction(TablePlacementZoneType.PlayerZone));
            gm.RegisterPhase(this);
        }
    }

    internal class PlayerTwoLosesInPhase : PlayerPhase
    {
        public PlayerTwoLosesInPhase(TestGameMediator gm, List<int> playerIDs, string name)
            : base(gm, playerIDs, name)
        {
            Actions.Add(new DrawCardsAction(TablePlacementZoneType.PlayerZone));
            Actions.Add(new DrawCardsAction(TablePlacementZoneType.PlayerZone));
            Actions.Add(new TestActionPlayerTwoLoses(gm));
            Actions.Add(new DrawCardsAction(TablePlacementZoneType.PlayerZone));
            gm.RegisterPhase(this);
        }
    }

    internal class TestGameMediator : BaseGameMediator
    {
        public TestGameMediator(int playerCount)
            : base(playerCount)
        {
        }

        PlayerPhase? phase;

        public void RegisterPhase(PlayerPhase phase)
        {
            this.phase = phase;
        }

        public void PhaseEnded()
        {
            TurnHandler!.ShiftTurnOrderClockwise();
        }

        public override void PlayerLost(int playerID)
        {
            base.PlayerLost(playerID);
            phase!.UpdateTurnOrder(TurnOrder);
        }
    }

    internal class TestActionPlayerTwoLoses : PlayerGameAction
    {
        IGameMediator gm;

        public TestActionPlayerTwoLoses(IGameMediator gm)
        {
            this.gm = gm;
        }

        public override object? Execute(IPlayer player)
        {
            if (player.PlayerID == 1)
            {
                player.LoseGame();
            }

            return null;
        }
    }

}
