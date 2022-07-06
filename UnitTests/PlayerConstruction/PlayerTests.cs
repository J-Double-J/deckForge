using deckForge.PlayerConstruction;
using deckForge.GameConstruction;
using deckForge.GameRules;
using deckForge.GameElements;
using FluentAssertions;

namespace UnitTests.PlayerConstruction
{
    [TestClass]
    public class PlayerTests
    {

        [TestMethod]
        public void PlayerExecutesPassed_DrawCommand()
        {
            IGameMediator gm = new BaseGameMediator(0);
            Player p1 = new(gm);

            p1.ExecuteCommand(() => { p1.DrawCard(); });
            p1.HandSize.Should().Be(6, "Player was passed a command to draw a card");
        }

        //TODO: Make a test GM with Add Player
        //[TestMethod]
        public void PlayerTellsAnotherPlayer_DrawCommand()
        {
            IGameMediator gm = new BaseGameMediator(0);
            Player p1 = new(gm, 0);
            Player p2 = new(gm, 1);

            //gm.AddPlayer(p1);
            //gm.AddPlayer(p2);

            p1.TellAnotherPlayerToExecuteCommand(1, (Player p) => p.DrawCard());
            p2.HandSize.Should().Be(6, "Player 2 was told to draw a card");
        }


        //TODO: Write a GM stub in order to command player
        //[TestMethod]
        public void PlayerGetsTheirPlayedCards_FromTable()
        {
            IGameMediator gm = new BaseGameMediator(0);
            Player p = new(gm, playerID: 0);
            Table table = new(gm, playerCount: 1);
            var stringReader = new StringReader("0");
            Console.SetIn(stringReader);

            //gm.AddPlayer(p);

            p.PlayCard();

            p.PlayedCards.Count.Should().Be(1, "Player played a card");
        }
    }
}
