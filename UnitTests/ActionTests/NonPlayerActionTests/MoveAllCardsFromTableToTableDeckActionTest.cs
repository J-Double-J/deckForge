using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.NonPlayerActions;
using FluentAssertions;

namespace UnitTests.ActionTests.NonPlayerActionTests
{
    [TestClass]
    public class MoveAllCardsFromTableToTableDeckActionTest
    {
        [TestMethod]
        public void CardsAreAddedToTheDeck()
        {
            IGameMediator gm = new BaseGameMediator(1);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            BaseGameAction action = new MoveAllCardsFromTableToTableDeckAction(gm, 0);

            table.AddCardsTo_NeutralZone(table.DrawMultipleCardsFromDeck(2, TablePlacementZoneType.PlayerZone)!, 0);
            table.AddCardsTo_NeutralZone(table.DrawMultipleCardsFromDeck(2, TablePlacementZoneType.PlayerZone)!, 1);

            action.Execute();

            table.TableDecks[0].Count.Should().Be(52, "all the cards were readded to the deck");
        }
    }
}
