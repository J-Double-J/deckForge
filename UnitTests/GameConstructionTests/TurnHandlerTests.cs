using DeckForge.GameConstruction;
using FluentAssertions;

namespace UnitTests.GameConstructionTests
{


    [TestClass]
    public class TurnHandlerTests
    {
        [TestMethod]
        public void TurnHandlerRotatesTurnOrderClockwise() {
            TurnHandler turnHandler = new(5, false);
            turnHandler.ShiftTurnOrderClockwise();

            for (int i = 0; i < 5; i++) {
                Console.Write(turnHandler.TurnOrder[i]);
            }
            List<int> targetTurnOrder = new List<int>() { 1, 2, 3, 4, 0 };

            turnHandler.TurnOrder.Should().BeEquivalentTo(targetTurnOrder);
        }

        [TestMethod]
        public void TurnHandlerRotatesTurnOrderCounterClockwise() {
            TurnHandler turnHandler = new(5, false);
            turnHandler.ShiftTurnOrderCounterClockwise();

            for (int i = 0; i < 5; i++)
            {
                Console.Write(turnHandler.TurnOrder[i]);
            }
            List<int> targetTurnOrder = new List<int>() { 4, 0, 1, 2, 3 };

            turnHandler.TurnOrder.Should().BeEquivalentTo(targetTurnOrder);
        }
    }
}
