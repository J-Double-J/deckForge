using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.ActionTests
{
    [TestClass]
    public class TakeCardsFromTableTests
    {
        [TestMethod]
        public void PlayerCanTakeCards_FromOpponenetTable_AndGainCardsToDeck() {
            IGameMediator gm = new BaseGameMediator(2);
            IPlayer player = new BasePlayer(gm);
            IPlayer opponent = new BasePlayer(gm, 1);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2);
            Table table = new(gm, new List<TableZone>() { zone });
            DeckOfPlayingCards deckOne = new();
            DeckOfPlayingCards deckTwo = new();

            player.AddResourceCollection(deckOne);
            opponent.AddResourceCollection(deckTwo);

            table.PlayCardToZone((ICard)player.TakeResourceFromCollection(0)!, TablePlacementZoneType.PlayerZone, 0);
            table.PlayCardToZone((ICard)player.TakeResourceFromCollection(0)!, TablePlacementZoneType.PlayerZone, 0);
            table.PlayCardToZone((ICard)opponent.TakeResourceFromCollection(0)!, TablePlacementZoneType.PlayerZone, 1);

            IGameAction<IPlayer> gameAction = new TakeAllCards_FromTargetPlayerTable_ToPlayerDeckAction();
            gameAction.Execute(player, opponent);

            player.CountOfResourceCollection(0).Should().Be(51, "they added two cards from their opponent and still have a card on the table");
        }
    }
}
