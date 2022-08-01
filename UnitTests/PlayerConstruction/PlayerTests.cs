using DeckForge.PlayerConstruction;
using DeckForge.GameConstruction;
using DeckForge.GameRules;
using DeckForge.GameElements;
using FluentAssertions;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;

namespace UnitTests.PlayerConstruction
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void PlayerExecutes_PassedDrawCommand()
        {
            IGameMediator gm = new BaseGameMediator(0);
            IPlayer p1 = new BasePlayer(gm);
            PlayerGameAction action = new DrawCardsAction();
            Table table = new(gm, 1, new Deck());

            p1.DrawStartingHand();
            p1.ExecuteGameAction(action);

            p1.HandSize.Should().Be(6, "Player was passed a command to draw a card");
        }

        [TestMethod]
        public void PlayerCannotExecute_DrawCommand_WithTargetting()
        {
            IGameMediator gm = new BaseGameMediator(0);
            IPlayer p1 = new BasePlayer(gm);
            IPlayer p2 = new BasePlayer(gm);
            PlayerGameAction action = new DrawCardsAction();
            Table table = new(gm, 1, new Deck());

            p1.DrawStartingHand();
            Action a = () => p1.ExecuteGameActionAgainstPlayer(action, p2);

            a.Should().Throw<NotSupportedException>("the draw action cannot be targetted against another player");
        }

        //TODO: Write a GM stub in order to command player
        //[TestMethod]
        public void PlayerGetsTheirPlayedCards_FromTable()
        {
            List<Deck> decks = new List<Deck> { new Deck() };
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Table table = new(gm, playerCount: 1, decks);
            var stringReader = new StringReader("0");
            Console.SetIn(stringReader);

            //gm.AddPlayer(p);

            p.PlayCard();

            p.PlayedCards.Count.Should().Be(1, "Player played a card");
        }

        [TestMethod]
        public void PlayerFindsCorrectResourceCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck();

            p.AddResourceCollection(d);
            int pos = p.FindCorrectResourceCollectionID(typeof(Card));

            pos.Should().Be(0, "the deck collection is at the 0th spot");
        }

        [TestMethod]
        public void PlayerDrawsCardFromDeckResource()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck();

            d.AddCardToDeck(new Card(21, "W"), pos: "top");
            p.AddResourceCollection(d);

            Card? c = (Card?)p.TakeResourceFromCollection(0);
            c!.Val.Should().Be(21, "the deck had 21W be added to the top");
        }

        [TestMethod]
        public void PlayerAdds_CardToDeckResource()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck(defaultAddCardPos: "top");

            p.AddResourceCollection(d);
            p.AddResourceToCollection(0, new Card(21, "W"));

            Card? c = (Card?)p.TakeResourceFromCollection(0);
            c!.Val.Should().Be(21, "the card 21W was added to the collection by the player");
        }

        [TestMethod]
        public void PlayerCannotAdd_InvalidResourcesToCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck(defaultAddCardPos: "top");

            p.AddResourceCollection(d);
            Action action = () => p.AddResourceToCollection(0, new BasePlayer(gm, playerID: 1));

            action.Should().Throw<ArgumentException>("player cannot add another player to a collection of cards");
        }

        [TestMethod]
        public void PlayerCanAdd_MultipleResourcesToCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck(defaultAddCardPos: "top");
            List<Card> cards = new() { new Card(21, "W"), new Card(22, "W"), new Card(23, "W"), new Card(24, "W"), };

            p.AddResourceCollection(d);
            List<object> objects = cards.Cast<object>().ToList();

            p.AddMultipleResourcesToCollection(0, objects);
            Card drawnCard = (Card)p.TakeResourceFromCollection(0)!;

            drawnCard.Val.Should().Be(24, "the last card added was 24W");
        }

        [TestMethod]
        public void PlayerCannotAdd_MultipleInvalidResourcesToCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck(defaultAddCardPos: "top");
            List<BasePlayer> players = new() { new BasePlayer(gm, 1), new BasePlayer(gm, 2), new BasePlayer(gm, 3) };

            p.AddResourceCollection(d);
            List<object> objects = players.Cast<object>().ToList();

            Action a = () => p.AddMultipleResourcesToCollection(0, objects);
            a.Should().Throw<ArgumentException>("players cannot be added to a deck");
        }

        [TestMethod]
        public void PlayerCanClear_ResourceCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck();

            p.AddResourceCollection(d);
            p.ClearResourceCollection(0);
            Card? card = (Card?)p.TakeResourceFromCollection(0);

            card.Should().BeNull("there are no card left in the deck");
        }

        [TestMethod]
        public void PlayerCanIncrement_ResourceCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck();

            p.AddResourceCollection(d);
            Action a = () => p.IncrementResourceCollection(0);

            a.Should().Throw<NotImplementedException>("deck cannot increment itself without a resource");
        }

        [TestMethod]
        public void PlayerCanDecrement_ResourceCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck();

            p.AddResourceCollection(d);
            p.DecrementResourceCollection(0);

            d.Count.Should().Be(51, "the deck was decremented and lost a card");
        }

        [TestMethod]
        public void PlayerKnowsTheirDeckCount()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            Deck d = new Deck();

            p.AddResourceCollection(d);
            p.TakeResourceFromCollection(0);
            p.TakeResourceFromCollection(0);
            p.TakeResourceFromCollection(0);

            p.CountOfResourceCollection(0).Should().Be(49, "3 cards were drawn from the standard deck");
        }
    }
}
