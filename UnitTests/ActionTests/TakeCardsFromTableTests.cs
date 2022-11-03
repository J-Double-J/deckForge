using DeckForge.PhaseActions;
using DeckForge.GameElements;
using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
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
            Table table = new (gm, 2);
            DeckOfPlayingCards deckOne = new DeckOfPlayingCards();
            DeckOfPlayingCards deckTwo = new DeckOfPlayingCards();

            player.AddResourceCollection(deckOne);
            opponent.AddResourceCollection(deckTwo);

            table.AddCardTo_PlayerZone(0, (PlayingCard)player.TakeResourceFromCollection(0)!);
            table.AddCardTo_PlayerZone(0, (PlayingCard)player.TakeResourceFromCollection(0)!);
            table.AddCardTo_PlayerZone(1, (PlayingCard)opponent.TakeResourceFromCollection(0)!);

            IGameAction<IPlayer> gameAction = new TakeAllCards_FromTargetPlayerTable_ToPlayerDeckAction();
            gameAction.Execute(player, opponent);

            player.CountOfResourceCollection(0).Should().Be(51, "they added two cards from their opponent and still have a card on the table");
        }
    }
}
