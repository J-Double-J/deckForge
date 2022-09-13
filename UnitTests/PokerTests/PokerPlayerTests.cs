using FluentAssertions;
using DeckForge.GameConstruction.PresetGames.Poker;

namespace UnitTests.PokerTests
{
    [TestClass]
    public class PokerPlayerTests
    {
        [TestMethod]
        public void PokerPlayerCanCall()
        {
            PokerGameMediator gm = new(1);
            PokerPlayer player = new(gm, 0, 100);

            gm.CurrentBet = 10;
            player.Call();

            player.BettingCash.Should().Be(90, "the player matched the current bet in the game at 10");
            player.InvestedCash.Should().Be(10, "the player has 10 invested in the current round");
        }

        [TestMethod]
        public void PokerPlayerCanRaise()
        {
            var stringReader = new StringReader("20");
            Console.SetIn(stringReader);
            PokerGameMediator gm = new(1);
            PokerPlayer player = new(gm, 0, 100);

            gm.CurrentBet = 10;
            player.Raise();

            gm.CurrentBet.Should().Be(20, "the player raised the bet to 20");
            player.BettingCash.Should().Be(80, "player raised the bet to 20");
            player.InvestedCash.Should().Be(20, "the player has invested 20 in the game");
        }

        [TestMethod]
        public void PokerPlayerCanFold()
        {
            PokerGameMediator gm = new(1);
            PokerPlayer player = new(gm, 0, 100);

            player.Fold();

            player.IsActive.Should().BeFalse("the player withdrew from the current round");
            player.IsOut.Should().BeFalse("the player still has enough money to play future rounds");
        }

        [TestMethod]
        public void PokerPlayerCannotCall_WithInsufficientFunds()
        {
            PokerGameMediator gm = new(1);
            PokerPlayer player = new(gm, 0, 10);

            gm.CurrentBet = 20;
            Action call = () => player.Call();

            call.Should().Throw<InvalidOperationException>("the player does not have enough betting cash to call the current bet");
        }

        // TODO: Determine how to properly test the while loop in the Raise() method
        [DataRow(-1)]
        [DataRow(2)]
        [DataRow(1000)]
        public void PokerPlayerCannotRaiseBet_ToInvalidIntegerValue(int value)
        {
            var stringReader = new StringReader(value.ToString());
            Console.SetIn(stringReader);
            PokerGameMediator gm = new(1);
            PokerPlayer player = new(gm, 0, 100);

            gm.CurrentBet = 10;
            Action raise = () => player.Raise();

            //raise.Should().Throw<>
        }
    }

    /*
    internal class PokerPlayerLoopless : PokerPlayer
    {
        PokerGameMediator pokerGM;

        public PokerPlayerLoopless(PokerGameMediator gm, int playerID, int bettingCash)
            : base(gm, playerID, bettingCash)
        {
            pokerGM = gm;
        }

        public new void Raise()
        {
            string? response;
            int raiseAmount;

            Console.WriteLine("What would you like to raise to?");
            response = Console.ReadLine();

            if (int.TryParse(response, out raiseAmount))
            {
                if (raiseAmount <= BettingCash && raiseAmount > pokerGM.CurrentBet)
                {
                    BettingCash -= raiseAmount;
                    InvestedCash += raiseAmount;
                    pokerGM.CurrentBet = raiseAmount;
                }
                else
                {
                    throw new InvalidOperationException($"Cannot make a bet of {raiseAmount}");
                }
            }
            else
            {
                throw new InvalidOperationException($"Cannot make a bet of {response}");
            }
        }
    }*/
}
