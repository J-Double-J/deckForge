using deckForge.PlayerConstruction;
using deckForge.GameConstruction;
using deckForge.GameRules;
using deckForge.GameElements;
using FluentAssertions;
using DeckNameSpace;

namespace UnitTests.PlayerConstruction
{
    [TestClass]
    public class PlayerTests
    {
        //Decide if execute command is valuable.
        //[TestMethod]
        public void PlayerExecutesPassed_DrawCommand()
        {
            IGameMediator gm = new BaseGameMediator(0);
            IPlayer p1 = new BasePlayer(gm);

            //p1.ExecuteCommand(() => { p1.DrawCard(); });
            p1.HandSize.Should().Be(6, "Player was passed a command to draw a card");
        }

        //TODO: Make a test GM with Add Player
        //[TestMethod]
        public void PlayerTellsAnotherPlayer_DrawCommand()
        {
            IGameMediator gm = new BaseGameMediator(0);
            IPlayer p1 = new BasePlayer(gm, 0);
            IPlayer p2 = new BasePlayer(gm, 1);

            //gm.AddPlayer(p1);
            //gm.AddPlayer(p2);

            //p1.TellAnotherPlayerToExecuteCommand(1, (IPlayer p) => p.DrawCard());
            p2.HandSize.Should().Be(6, "Player 2 was told to draw a card");
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
    }
}
