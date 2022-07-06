using deckForge.GameConstruction;
using CardNamespace;
using FluentAssertions;

namespace UnitTests.GameConstructionTests
{
    [TestClass]
    public class GameMediatorTests
    {
        [TestMethod]
        public void GetPlayerByID_ThrowsOnInvalidID()
        {
            IGameMediator gm = new BaseGameMediator(2);
            Action a = () => gm.GetPlayerByID(3);
            a.Should().Throw<ArgumentException>("an invalid player ID was passed to the GameMediator");
        }

        [TestMethod]
        public void GameMediatorCanDrawCard()
        {
            IGameMediator gm = new BaseGameMediator(0);

            Card c = gm.DrawCardFromDeck()!;

            c.Should().NotBeNull("a new deck was created so it should have cards");
        }

        [TestMethod]
        public void GameMediatorCannotDrawFromEmptyDeck()
        {
            IGameMediator gm = new BaseGameMediator(0);
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
    }
}
