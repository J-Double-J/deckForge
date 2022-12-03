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
            TableZone zone = new(TablePlacementZoneType.NeutralZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            BaseGameAction action = new MoveAllCardsFromTableToTableDeckAction(gm, TablePlacementZoneType.NeutralZone, true);
            var neutral = TablePlacementZoneType.NeutralZone;

            table.PlayCardsToZone(table.DrawMultipleCardsFromDeck(2, neutral)!, neutral, 0);
            table.PlayCardsToZone(table.DrawMultipleCardsFromDeck(2, neutral)!, neutral, 1);

            action.Execute();

            table.TableZones[0].GetDeckFromZone(0)!.Count.Should().Be(52, "all the cards were readded to the deck");
        }
    }
}
