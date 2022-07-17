using deckForge.PhaseActions;
using deckForge.GameElements;
using deckForge.GameConstruction;
using deckForge.GameElements.Resources;
using deckForge.PlayerConstruction;
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
            Deck deckOne = new Deck();
            Deck deckTwo = new Deck();

            player.AddResourceCollection(deckOne);
            opponent.AddResourceCollection(deckTwo);

            table.PlaceCardOnTable(0, (Card)player.TakeResourceFromCollection(0)!);
            Console.WriteLine(player.CountOfResourceCollection(0));
            table.PlaceCardOnTable(0, (Card)player.TakeResourceFromCollection(0)!);
            Console.WriteLine(player.CountOfResourceCollection(0));
            table.PlaceCardOnTable(1, (Card)opponent.TakeResourceFromCollection(0)!);
            Console.WriteLine(opponent.CountOfResourceCollection(0));

            IAction<IPlayer> gameAction = new TakeAllCards_FromTargetPlayerTable_ToPlayerDeck();
            gameAction.execute(player, opponent);

            player.CountOfResourceCollection(0).Should().Be(51, "they added a card from their opponent and still have a card on the table");
        }
    }
}
