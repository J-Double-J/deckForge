using DeckForge.GameConstruction.PresetGames.Poker;
using FluentAssertions;
using UnitTests.PokerTests.TestablePokerPlayer;

namespace UnitTests.PokerTests
{
    [TestClass]
    public class PokerPlayerTests
    {
        [TestMethod]
        public void PokerPlayerCanCall()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 100);

            gm.CurrentBet = 10;
            player.Call();

            player.BettingCash.Should().Be(90, "the player matched the current bet in the game at 10");
            player.InvestedCash.Should().Be(10, "the player has 10 invested in the current round");
        }

        [TestMethod]
        public void PokerPlayerCanRaise()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 100);

            gm.CurrentBet = 10;
            player.Commands.Add("RAISE");
            player.GetPreFlopBettingAction();

            gm.CurrentBet.Should().Be(20, "the player raised the bet to 20");
            player.BettingCash.Should().Be(80, "player raised the bet to 20");
            player.InvestedCash.Should().Be(20, "the player has invested 20 in the game");
        }

        [TestMethod]
        public void PokerPlayerCanFold()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 100);

            player.Fold();

            player.IsActive.Should().BeFalse("the player withdrew from the current round");
            player.IsOut.Should().BeFalse("the player still has enough money to play future rounds");
        }

        [TestMethod]
        public void PokerPlayerCannotCall_WithInsufficientFunds()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 10);

            gm.CurrentBet = 20;
            Action call = () => player.Call();

            call.Should().Throw<InvalidOperationException>("the player does not have enough betting cash to call the current bet");
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(2)]
        [DataRow(1000)]
        public void PokerPlayerCannotRaiseBet_ToInvalidIntegerValue(int value)
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 100);

            gm.CurrentBet = 10;
            player.RaiseToAmount = value;
            player.Commands.Add("RAISE");
            Action raise = () => player.GetPreFlopBettingAction();

            raise.Should().Throw<ArgumentException>($"the player attempted to raise to the invalid value {value}");
        }

        [TestMethod]
        public void PokerPlayerInvestedCash_IsCorrect_AfterMultipleRaises()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 100);

            gm.CurrentBet = 10;
            player.RaiseToAmount = 20;
            player.Commands.AddRange(new List<string> { "RAISE", "RAISE", "RAISE" });
            player.GetPreFlopBettingAction();
            player.RaiseToAmount = 30;
            player.GetPreFlopBettingAction();
            player.RaiseToAmount = 35;
            player.GetPreFlopBettingAction();

            player.InvestedCash.Should().Be(35, "the player raised 3 times to eventually 35");
            player.BettingCash.Should().Be(65, "the player has invested a total of 35 to the current round");
        }

        [TestMethod]
        public void PokerPlayerInvestedCash_IsCorrect_AfterRaisingAndCalling()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 100);

            gm.CurrentBet = 10;
            player.RaiseToAmount = 20;
            player.Commands.AddRange(new List<string> { "RAISE", "CALL" });
            player.GetPreFlopBettingAction();
            player.GetPreFlopBettingAction();

            player.InvestedCash.Should().Be(20, "the player raised to 20 and stayed at that amount even when erroneously calling instead of checking");
        }
    }
}
