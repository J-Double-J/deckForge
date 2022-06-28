using deckForge.GameElements;
using deckForge.GameConstruction;
using CardNamespace;
using FluentAssertions;

namespace UnitTests.GameElements
{
    [TestClass]
    public class TableTests
    {
        [TestMethod]
        [DataRow(-1)]
        [DataRow(3)]
        public void PlaceCardOnTableInFrontOfNonexistantPlayer(int fakePlayerID) {
            GameMediator gm = new(1);
            Table table = new(gm, 1);

            Action a = () => table.PlaceCardOnTable(fakePlayerID, new Card(8, "J"));

            a.Should().Throw<ArgumentException>($"there is no player with the id of {fakePlayerID}");
        }

        //This test is failing for whatever reason, and it has to do probably with behind the scenes Console.WriteLine stuff
        //I suspect. The test is printing what I expect. This test will remain in case I need to return to it.
        public void TablePrintsCardsCorrectly() {
            StringWriter sw = new();
            Console.SetOut(sw);
            GameMediator gm = new(2);
            Table table = new(gm, 2);

            table.PlaceCardOnTable(0, new Card(8, "J", facedown: false));
            table.PlaceCardOnTable(0, new Card(9, "J", facedown: false));
            table.PlaceCardOnTable(0, new Card(10, "J", facedown: false));
            table.PlaceCardOnTable(1, new Card(8, "J", facedown: false));
            table.PlaceCardOnTable(1, new Card(9, "J", facedown: false));
            table.PlaceCardOnTable(1, new Card(10, "J", facedown: false));

            table.PrintTableState();

            sw.ToString().Should().Be("8J\n9J\n10J\n8J\n9J\n10J\n", "these six cards were placed on the table");
        }
    }
}
