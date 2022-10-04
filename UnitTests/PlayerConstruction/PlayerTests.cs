using DeckForge.GameConstruction;
using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction;
using FluentAssertions;
using UnitTests.Mocks;

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
            Table table = new(gm, 1, new DeckOfPlayingCards());

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
            Table table = new(gm, 1, new DeckOfPlayingCards());

            p1.DrawStartingHand();
            Action a = () => p1.ExecuteGameActionAgainstPlayer(action, p2);

            a.Should().Throw<NotSupportedException>("the draw action cannot be targetted against another player");
        }

        [TestMethod]
        public void PlayerGetsTheirPlayedCards_FromTable()
        {
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            IGameMediator gm = new BaseGameMediator(0);
            TestPlayerMock player = new(gm, playerID: 0);
            Table table = new(gm, playerCount: 1, decks);
            player.AddCardToHand(new PlayingCard(10, "J"));

            player.PlayCard();

            player.PlayedCards.Count.Should().Be(1, "Player played a card");
        }

        [TestMethod]
        public void PlayerFindsCorrectResourceCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            DeckOfPlayingCards d = new DeckOfPlayingCards();

            p.AddResourceCollection(d);
            int pos = p.FindCorrectResourceCollectionID(typeof(PlayingCard));

            pos.Should().Be(0, "the deck collection is at the 0th spot");
        }

        [TestMethod]
        public void PlayerDrawsCardFromDeckResource()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            DeckOfPlayingCards d = new DeckOfPlayingCards();

            d.AddCardToDeck(new PlayingCard(21, "W"), pos: "top");
            p.AddResourceCollection(d);

            PlayingCard? c = (PlayingCard?)p.TakeResourceFromCollection(0);
            c!.Val.Should().Be(21, "the deck had 21W be added to the top");
        }

        [TestMethod]
        public void PlayerAdds_CardToDeckResource()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            DeckOfPlayingCards d = new(defaultAddCardPos: "top");

            p.AddResourceCollection(d);
            p.AddResourceToCollection(0, new PlayingCard(21, "W"));

            PlayingCard? c = (PlayingCard?)p.TakeResourceFromCollection(0);
            c!.Val.Should().Be(21, "the card 21W was added to the collection by the player");
        }

        [TestMethod]
        public void PlayerCannotAdd_InvalidResourcesToCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            DeckOfPlayingCards d = new(defaultAddCardPos: "top");

            p.AddResourceCollection(d);
            Action action = () => p.AddResourceToCollection(0, new BasePlayer(gm, playerID: 1));

            action.Should().Throw<ArgumentException>("player cannot add another player to a collection of cards");
        }

        [TestMethod]
        public void PlayerCanAdd_MultipleResourcesToCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            DeckOfPlayingCards d = new(defaultAddCardPos: "top");
            List<PlayingCard> cards = new() { new PlayingCard(21, "W"), new PlayingCard(22, "W"), new PlayingCard(23, "W"), new PlayingCard(24, "W"), };

            p.AddResourceCollection(d);
            List<object> objects = cards.Cast<object>().ToList();

            p.AddMultipleResourcesToCollection(0, objects);
            PlayingCard drawnCard = (PlayingCard)p.TakeResourceFromCollection(0)!;

            drawnCard.Val.Should().Be(24, "the last card added was 24W");
        }

        [TestMethod]
        public void PlayerCannotAdd_MultipleInvalidResourcesToCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            DeckOfPlayingCards d = new DeckOfPlayingCards(defaultAddCardPos: "top");
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
            DeckOfPlayingCards d = new();

            p.AddResourceCollection(d);
            p.ClearResourceCollection(0);
            PlayingCard? card = (PlayingCard?)p.TakeResourceFromCollection(0);

            card.Should().BeNull("there are no card left in the deck");
        }

        [TestMethod]
        public void PlayerCannotIncrement_DeckResourceCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            DeckOfPlayingCards d = new();

            p.AddResourceCollection(d);
            Action a = () => p.IncrementResourceCollection(0);

            a.Should().Throw<NotImplementedException>("deck cannot increment itself without a resource");
        }

        [TestMethod]
        public void PlayerCanDecrement_ResourceCollection()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            DeckOfPlayingCards d = new();

            p.AddResourceCollection(d);
            p.DecrementResourceCollection(0);

            d.Count.Should().Be(51, "the deck was decremented and lost a card");
        }

        [TestMethod]
        public void PlayerKnowsTheirDeckCount()
        {
            IGameMediator gm = new BaseGameMediator(0);
            BasePlayer p = new(gm, playerID: 0);
            DeckOfPlayingCards d = new();

            p.AddResourceCollection(d);
            p.TakeResourceFromCollection(0);
            p.TakeResourceFromCollection(0);
            p.TakeResourceFromCollection(0);

            p.CountOfResourceCollection(0).Should().Be(49, "3 cards were drawn from the standard deck");
        }

        [TestMethod]
        public void PlayerCanAddCardToHand()
        {
            IGameMediator gm = new BaseGameMediator(1);
            IPlayer p = new BasePlayer(gm, 0);
            IDeck deck = new DeckOfPlayingCards();

            ICard? drawnCard = deck.DrawCard();
            p.AddCardToHand(drawnCard!);

            p.HandSize.Should().Be(1, "because a card was added to the player's hand");
        }
    }
}
