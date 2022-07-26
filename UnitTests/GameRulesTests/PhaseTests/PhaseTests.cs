using deckForge.GameConstruction;
using deckForge.GameConstruction.PresetGames.War;
using deckForge.GameElements;
using deckForge.GameElements.Resources;
using deckForge.GameRules.RoundConstruction.Interfaces;
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
        public void PlayerPhaseCanChangeTurnOrder()
        {
            IGameMediator gm = new TestGameMediator(3);
            IGameController gc = new BaseGameController(3);
            gm.RegisterGameController(gc);
            Table table = new(gm, 3, new Deck());

            List<int> playerIDs = new();
            List<IPlayer> players = new();
            for (var i = 0; i < 3; i++)
            {
                players.Add(new WarPlayer(gm, i, new Deck()));
                playerIDs.Add(i);
            }

            PlayerPhase playerPhase = new TestPhase((TestGameMediator)gm, playerIDs, "Test Phase");

            playerPhase.StartPhase();
            gm.RoundEnded();
            playerPhase.UpdateTurnOrder(gm.TurnOrder);
            playerPhase.StartPhase();

            playerPhase.PlayerTurnOrder.Should().BeEquivalentTo(gm.TurnOrder, "the turn order of the phase should be what GameMediator says it is.");
            foreach (IPlayer player in players)
            {
                player.HandSize.Should().Be(6, "each player was told to draw 3 cards twice");
            }
        }

        //DEBUG THIS. Trying to use the method of IsActive and IsOut, and this was partially done, but probably
        //not properly.
        [TestMethod]
        public void PlayerPhaseLosesPlayerMidPhase()
        {
            IGameMediator gm = new TestGameMediator(3);
            IGameController gc = new BaseGameController(3);
            gm.RegisterGameController(gc);
            Table table = new(gm, 3, new Deck());

            List<int> playerIDs = new();
            List<IPlayer> players = new();
            for (var i = 0; i < 3; i++)
            {
                players.Add(new WarPlayer(gm, i, new Deck()));
                playerIDs.Add(i);
            }
            List<int> targetPlayOrder = new List<int>() { 0, 2 };

            PlayerPhase playerPhase = new PlayerTwoLosesInPhase((TestGameMediator)gm, playerIDs, "Test Phase");

            playerPhase.StartPhase();

            table.TableState.Count.Should().Be(3, "even though a player is out, their table spot remains");
            table.TableState[1].Count.Should().Be(0, "all cards belonging to Player 2 should've been picked up");
            gm.TurnOrder.Should().BeEquivalentTo(targetPlayOrder);
            playerPhase.PlayerTurnOrder.Should().BeEquivalentTo(targetPlayOrder);
        }
    }

    //Test Objects
    internal class TestPhase : PlayerPhase
    {
        public TestPhase(TestGameMediator gm, List<int> playerIDs, string name) : base(gm, playerIDs, name)
        {
            Actions.Add(new DrawCardsAction());
            Actions.Add(new DrawCardsAction());
            Actions.Add(new DrawCardsAction());
            gm.RegisterPhase(this);
        }
    }

    internal class PlayerTwoLosesInPhase : PlayerPhase
    {
        public PlayerTwoLosesInPhase(TestGameMediator gm, List<int> playerIDs, string name) : base(gm, playerIDs, name)
        {
            Actions.Add(new DrawCardsAction());
            Actions.Add(new DrawCardsAction());
            Actions.Add(new TestActionPlayerTwoLoses(gm));
            Actions.Add(new DrawCardsAction());
            gm.RegisterPhase(this);
        }
    }

    internal class TestGameMediator : BaseGameMediator
    {
        public TestGameMediator(int playerCount) : base(playerCount)
        {
        }

        PlayerPhase? phase;

        public void RegisterPhase(PlayerPhase phase)
        {
            this.phase = phase;
        }

        public override void RoundEnded()
        {
            GameController!.ShiftTurnOrderClockwise();
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

        public override object? execute(IPlayer player)
        {
            if (player.PlayerID == 1)
            {
                player.LoseGame();
            }
            return null;
        }
    }

}
