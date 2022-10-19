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

        [TestMethod]
        public void PokerPlayerGetsCorrectPromptOnPreFlop()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 100);
            StringWriter output = new();
            StringReader input = new("3");
            Console.SetOut(output);
            Console.SetIn(input);

            gm.CurrentBet = 10;
            player.ConsolePromptTest(true);

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("Would you like to:\n\t1) Call\n\t2) Raise\n\t3) Fold\n\t4) All In!\n\n", "Player can do any of the preflop options");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("Would you like to:\r\n\t1) Call\r\n\t2) Raise\r\n\t3) Fold\r\n\t4) All In!\r\n", "Player can do any of the preflop options");
            } // Unsure why there needs to be \r here but no where else, but I suspect it has to do with Console.WriteLine() on windows
        }

        [TestMethod]
        public void PokerPlayerGetsCorrectPromptAfterFlop()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 100);
            StringWriter output = new();
            StringReader input = new("3");
            Console.SetOut(output);
            Console.SetIn(input);

            gm.CurrentBet = 10;
            player.SetInvestedCash(10);
            player.ConsolePromptTest(false);

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("Would you like to:\n\t2) Raise\n\t3) Fold\n\t4) All In!\n\t5) Check\n\n", "Player can do most post flop options");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("Would you like to:\r\n\t2) Raise\r\n\t3) Fold\r\n\t4) All In!\r\n\t5) Check\r\n", "Player can do most post flop options");
            }
        }

        [TestMethod]
        public void PokerPlayerGetsCorrectPromptWhenNotMatchingBet()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 100);
            StringWriter output = new();
            StringReader input = new("3");
            Console.SetOut(output);
            Console.SetIn(input);

            gm.CurrentBet = 20;
            player.SetInvestedCash(10);
            player.ConsolePromptTest(false);

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("Would you like to:\n\t1) Call\n\t2) Raise\n\t3) Fold\n\t4) All In!\n", "Player cannot Check as they do not match the bet");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("Would you like to:\r\n\t1) Call\r\n\t2) Raise\r\n\t3) Fold\r\n\t4) All In!\r\n", "Player cannot Check as they do not match the bet");
            }
        }

        [TestMethod]
        public void PokerPlayerGetsCorrectPrompt_NeedsToMatchButCannotRaise()
        {
            PokerGameMediator gm = new(1);
            PokerPlayerWithProgrammedActions player = new(gm, 0, 10);
            StringWriter output = new();
            StringReader input = new("3");
            Console.SetOut(output);
            Console.SetIn(input);

            gm.CurrentBet = 20;
            player.SetInvestedCash(10); // This does not count against Player's current cash
            player.ConsolePromptTest(false);

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("Would you like to:\n\t3) Fold\n\t4) All In!\n\n", "Player can go all in to match");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("Would you like to:\r\n\t3) Fold\r\n\t4) All In!\r\n", "Player can go all in to match");
            }
        }
    }
}
