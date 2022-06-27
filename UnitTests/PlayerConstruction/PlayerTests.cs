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
        public void PlayerExecutesPassed_DrawCommand() {
            GameMediator gm = new(0);
            Player p1 = new(gm);

            p1.ExecuteCommand(() => { p1.DrawCard(); });
            p1.HandSize.Should().Be(6, "Player was passed a command to draw a card");
        }
    }
}
