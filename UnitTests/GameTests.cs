using FluentAssertions;
using DeckForge.GameConstruction;

namespace UnitTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]

        public void GetNextPlayerTurn_WithOrWithoutInitTurnRandomization(bool randomize)
        {
            BaseGameController g = new(2, randomize);

            int curPlayer = g.GetCurrentPlayer();
            int nextPlayer = g.NextPlayerTurn();

            nextPlayer.Should().NotBe(curPlayer, "the next player should be different from the current player");
        }

        [TestMethod]
        public void GetPlayerWhoseTurnItIs_5TurnsFromNow()
        {
            BaseGameController g = new BaseGameController(3);

            g.PlayerTurnXTurnsFromNow(5).Should().Be(2, "the player whose turn it is 5 turns from now is player 2 (counting from 0)");
        }


    }
}
