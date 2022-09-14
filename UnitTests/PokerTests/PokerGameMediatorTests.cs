using DeckForge.GameConstruction.PresetGames.Poker;
using FluentAssertions;
using UnitTests.PokerTests.TestablePokerPlayer;

namespace UnitTests.PokerTests
{
    [TestClass]
    public class PokerGameMediatorTests
    {
        [TestMethod]
        public void MediatorGetsEachPlayersCall()
        {
            PokerGameMediator pGM = new(4);
            for (int i = 0; i < 4; i++)
            {
                PokerPlayerWithProgrammedActions player = new(pGM, i, 100);
                player.Commands.Add("CALL");
            }

            pGM.CurrentBet = 10;
            pGM.PlayersBet();

            for (int i = 0; i < 4; i++)
            {
                PokerPlayerWithProgrammedActions player = (pGM.GetPlayerByID(i) as PokerPlayerWithProgrammedActions)!;
                player.BettingCash.Should().Be(90, "each player put 10 to match the current bet");
                player.InvestedCash.Should().Be(10, "each player put 10 to match the current bet");
            }
        }

        [TestMethod]
        public void MediatorGetsEachAction_OnePlayerInMiddleFolds()
        {
            PokerGameMediator pGM = new(4);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 100);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 1, 100);
            PokerPlayerWithProgrammedActions playerThree = new(pGM, 2, 100);
            PokerPlayerWithProgrammedActions playerFour = new(pGM, 3, 100);

            playerOne.Commands.Add("CALL");
            playerTwo.Commands.Add("CALL");
            playerThree.Commands.Add("FOLD");
            playerFour.Commands.Add("CALL");
            pGM.CurrentBet = 10;
            pGM.PlayersBet();

            playerOne.InvestedCash.Should().Be(10, "the player called the current bet");
            playerTwo.InvestedCash.Should().Be(10, "the player called the current bet");
            playerThree.InvestedCash.Should().Be(0, "the player folded through the betting phase");
            playerFour.InvestedCash.Should().Be(10, "the player called the current bet");
        }

        [TestMethod]
        public void MediatorGetsEachAction_LastPlayerFolds()
        {
            PokerGameMediator pGM = new(4);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 100);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 1, 100);
            PokerPlayerWithProgrammedActions playerThree = new(pGM, 2, 100);
            PokerPlayerWithProgrammedActions playerFour = new(pGM, 3, 100);

            playerOne.Commands.Add("CALL");
            playerTwo.Commands.Add("CALL");
            playerThree.Commands.Add("CALL");
            playerFour.Commands.Add("FOLD");
            pGM.CurrentBet = 10;
            pGM.PlayersBet();

            playerOne.InvestedCash.Should().Be(10, "the player called the current bet");
            playerTwo.InvestedCash.Should().Be(10, "the player called the current bet");
            playerThree.InvestedCash.Should().Be(10, "the player called the current bet");
            playerFour.InvestedCash.Should().Be(0, "the player folded through the betting phase");
        }

        [TestMethod]
        public void MediatorGetsEachAction_PlayerRaisesMidway()
        {
            PokerGameMediator pGM = new(4);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 100);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 1, 100);
            PokerPlayerWithProgrammedActions playerThree = new(pGM, 2, 100);
            PokerPlayerWithProgrammedActions playerFour = new(pGM, 3, 100);

            playerOne.Commands.AddRange(new List<string> { "CALL", "CALL" });
            playerTwo.Commands.AddRange(new List<string> { "CALL", "CALL" });
            playerThree.RaiseToAmount = 20;
            playerThree.Commands.Add("RAISE");
            playerFour.Commands.Add("CALL");
            pGM.CurrentBet = 10;
            pGM.PlayersBet();

            playerOne.InvestedCash.Should().Be(20, "the player called the current bet");
            playerTwo.InvestedCash.Should().Be(20, "the player called the current bet");
            playerThree.InvestedCash.Should().Be(20, "the player raised during the betting phase");
            playerFour.InvestedCash.Should().Be(20, "the player called the current bet");
        }

        [TestMethod]
        public void MediatorGetsEachAction_TwoPlayersRaisesMidway()
        {
            PokerGameMediator pGM = new(4);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 100);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 1, 100);
            PokerPlayerWithProgrammedActions playerThree = new(pGM, 2, 100);
            PokerPlayerWithProgrammedActions playerFour = new(pGM, 3, 100);

            playerOne.Commands.AddRange(new List<string> { "CALL", "CALL", "CALL" });
            playerTwo.RaiseToAmount = 30;
            playerTwo.Commands.AddRange(new List<string> { "CALL", "RAISE" });
            playerThree.RaiseToAmount = 20;
            playerThree.Commands.AddRange(new List<string> { "RAISE", "CALL" });
            playerFour.Commands.AddRange(new List<string> { "CALL", "CALL" });
            pGM.CurrentBet = 10;
            pGM.PlayersBet();

            playerOne.InvestedCash.Should().Be(30, "the player called the current bet three times");
            playerTwo.InvestedCash.Should().Be(30, "the player called and then raised the bet");
            playerThree.InvestedCash.Should().Be(30, "the player raised the bet then called");
            playerFour.InvestedCash.Should().Be(30, "the player called the current bet twice");
        }

        [TestMethod]
        public void MediatorGetsEachAction_OneRaiseOneFold()
        {
            PokerGameMediator pGM = new(4);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 100);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 1, 100);
            PokerPlayerWithProgrammedActions playerThree = new(pGM, 2, 100);
            PokerPlayerWithProgrammedActions playerFour = new(pGM, 3, 100);

            playerOne.Commands.AddRange(new List<string> { "CALL", "FOLD" });
            playerTwo.Commands.AddRange(new List<string> { "CALL", "CALL" });
            playerThree.RaiseToAmount = 20;
            playerThree.Commands.AddRange(new List<string> { "RAISE" });
            playerFour.Commands.AddRange(new List<string> { "CALL" });
            pGM.CurrentBet = 10;
            pGM.PlayersBet();

            playerOne.InvestedCash.Should().Be(10, "the player called the current bet but folded the second time");
            playerTwo.InvestedCash.Should().Be(20, "the player called the current bet");
            playerThree.InvestedCash.Should().Be(20, "the player raised the bet");
            playerFour.InvestedCash.Should().Be(20, "the player called the current bet");
        }

        [TestMethod]
        public void MediatorGetsEachAction_AllFoldButOne()
        {
            PokerGameMediator pGM = new(4);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 100);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 1, 100);
            PokerPlayerWithProgrammedActions playerThree = new(pGM, 2, 100);
            PokerPlayerWithProgrammedActions playerFour = new(pGM, 3, 100);

            playerOne.Commands.AddRange(new List<string> { "FOLD" });
            playerTwo.Commands.AddRange(new List<string> { "FOLD" });
            playerThree.Commands.AddRange(new List<string> { "CALL" });
            playerFour.Commands.AddRange(new List<string> { "FOLD" });
            pGM.CurrentBet = 10;
            pGM.PlayersBet();

            playerOne.InvestedCash.Should().Be(0, "the player folded through the betting phase");
            playerTwo.InvestedCash.Should().Be(0, "the player folded through the betting phase");
            playerThree.InvestedCash.Should().Be(10, "the player called the bet");
            playerFour.InvestedCash.Should().Be(0, "the player folded through the betting phase");
        }
    }
}
