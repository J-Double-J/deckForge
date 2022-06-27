using deckForge.PlayerConstruction;
using deckForge.GameConstruction;
using deckForge.GameRules;
using FluentAssertions;

namespace UnitTests.PlayerConstruction
{
    [TestClass]
    public class PlayerTests
    {

        [TestMethod]
        public void PlayerExecutesPassed_DrawCommand()
        {
            GameMediator gm = new(0);
            Player p1 = new(gm);

            p1.ExecuteCommand(() => { p1.DrawCard(); });
            p1.HandSize.Should().Be(6, "Player was passed a command to draw a card");
        }

        [TestMethod]
        public void PlayerTellsAnotherPlayer_DrawCommand()
        {
            GameMediator gm = new(0);
            Player p1 = new(gm, 0);
            Player p2 = new(gm, 1);

            gm.AddPlayer(p1);
            gm.AddPlayer(p2);

            p1.TellAnotherPlayerToExecuteCommand(1, (Player p) => p.DrawCard());
            p2.HandSize.Should().Be(6, "Player 2 was told to draw a card");
        }
    }
}
