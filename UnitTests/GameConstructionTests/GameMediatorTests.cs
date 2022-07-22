using deckForge.GameConstruction;
using FluentAssertions;
using deckForge.GameElements;
using deckForge.PlayerConstruction;
using deckForge.GameElements.Resources;
using deckForge.PhaseActions;

namespace UnitTests.GameConstructionTests
{
    [TestClass]
    public class GameMediatorTests
    {
        [TestMethod]
        public void GetPlayerByID_ThrowsOnInvalidID()
        {
            IGameMediator gm = new BaseGameMediator(2);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 0, decks);
            new BasePlayer(gm, 0);
            new BasePlayer(gm, 1);
            Action a = () => gm.GetPlayerByID(3);
            a.Should().Throw<ArgumentException>("an invalid player ID was passed to the GameMediator");
        }

        [TestMethod]
        public void GameMediatorCanDrawCard()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 0, decks);

            Card c = gm.DrawCardFromDeck()!;

            c.Should().NotBeNull("a new deck was created so it should have cards");
        }

        [TestMethod]
        public void GameMediatorCannotDrawFromEmptyDeck()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<Deck> decks = new() { new Deck() };
            Table table = new(gm, 0, decks);

            Card? c;

            for (var i = 0; i < 52; i++)
            {
                c = gm.DrawCardFromDeck()!;
            }

            c = gm.DrawCardFromDeck();


            c.Should().BeNull("the deck was exhausted and there are no more cards to draw");
        }

        [TestMethod]
        public void InitiateGameMediatorWithNegativePlayers()
        {
            Action a = () => new BaseGameMediator(-1);

            a.Should().Throw<ArgumentException>("a game cannot have negative players");
        }

        [TestMethod]
        public void InitiateGameMediatorWithTooManyPlayers()
        {
            Action a = () => new BaseGameMediator(13);

            a.Should().Throw<ArgumentException>("a game cannot have more than 12 players at the moment");
        }

        [TestMethod]
        public void GameMediatorCanTellPlayerToExecuteGameAction() {
            IGameMediator gm = new BaseGameMediator(1);
            IPlayer player = new BasePlayer(gm);
            Table table = new Table(gm, 1, new Deck());
            PlayerGameAction action = new DrawCardsAction();

            gm.TellPlayerToDoAction(0, action);

            player.HandSize.Should().Be(1, "the player drew a card into their hand");
        }

        [TestMethod]
        public void GameMediatorCannotTellPlayer_ToExecuteInvalidGameAction() {
            IGameMediator gm = new BaseGameMediator(2);
            IPlayer player = new BasePlayer(gm);
            IPlayer target = new BasePlayer(gm);
            Table table = new Table(gm, 2, new Deck());
            PlayerGameAction action = new DrawCardsAction();

            Action a = () => gm.TellPlayerToDoActionAgainstAnotherPlayer(0, 1, action);

            a.Should().Throw<NotSupportedException>("draw cannot be targetted against another player");
        }
    }
}
