using DeckForge.GameConstruction.PresetGames.Poker;
using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
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

        [TestMethod]
        public void MediatorAwardsPlayersForWinningRound()
        {
            PokerGameMediator pGM = new(2);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 0);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 1, 0);
            Table table = new(pGM, 2, 1);

            playerOne.SetInvestedCash(25);
            playerTwo.SetInvestedCash(25);

            table.AddCardsTo_NeutralZone(
                new List<PlayingCard>()
                {
                    new PlayingCard(2, "C"), new PlayingCard(3, "C"), new PlayingCard(4, "C")
                },
                0);

            table.PlayerPlayedCards[0].AddRange(new List<PlayingCard>() { new PlayingCard(10, "S"), new PlayingCard(5, "S") });
            table.PlayerPlayedCards[1].AddRange(new List<PlayingCard>() { new PlayingCard(1, "S"), new PlayingCard(2, "S") });

            pGM.EvaluateWinner();

            playerOne.BettingCash.Should().Be(50, "first player won the pot");
            playerTwo.BettingCash.Should().Be(0, "the player did not win");
        }

        [TestMethod]
        public void MediatorAwardsPlayersCorrectlyDuringATie()
        {
            PokerGameMediator pGM = new(3);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 0);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 1, 0);
            PokerPlayerWithProgrammedActions playerThree = new(pGM, 2, 0);
            Table table = new(pGM, 3, 1);

            playerOne.SetInvestedCash(20);
            playerTwo.SetInvestedCash(20);
            playerThree.SetInvestedCash(20);

            table.AddCardsTo_NeutralZone(
                new List<PlayingCard>()
                {
                    new PlayingCard(2, "C"), new PlayingCard(3, "C"), new PlayingCard(4, "C")
                },
                0);

            table.PlayerPlayedCards[0].AddRange(new List<PlayingCard>() { new PlayingCard(10, "S"), new PlayingCard(5, "S") });
            table.PlayerPlayedCards[1].AddRange(new List<PlayingCard>() { new PlayingCard(10, "D"), new PlayingCard(5, "D") });
            table.PlayerPlayedCards[2].AddRange(new List<PlayingCard>() { new PlayingCard(1, "S"), new PlayingCard(2, "S") });

            pGM.EvaluateWinner();

            playerOne.BettingCash.Should().Be(30, "player won half the pot");
            playerTwo.BettingCash.Should().Be(30, "player won half the pot");
            playerThree.BettingCash.Should().Be(0, "player lost the round");
        }

        [TestMethod]
        public void MediatorMarksPlayersThatAreBroke()
        {
            PokerGameMediator pGM = new(3);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 0);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 0, 100);
            PokerPlayerWithProgrammedActions playerThree = new(pGM, 0, 100);

            pGM.HandlePotentialBrokePlayers();

            playerOne.IsOut.Should().Be(true, "player was broke and is out");
            playerOne.IsActive.Should().Be(false, "player was broke and is out");
        }

        [TestMethod]
        public void GameEndsWhenAllPlayersButOneIsBroke()
        {
            StringWriter output = new();
            Console.SetOut(output);
            PokerGameMediator pGM = new(3);
            PokerPlayerWithProgrammedActions playerOne = new(pGM, 0, 100);
            PokerPlayerWithProgrammedActions playerTwo = new(pGM, 0, 0);
            PokerPlayerWithProgrammedActions playerThree = new(pGM, 0, 0);

            pGM.HandlePotentialBrokePlayers();

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be("Player 0 wins!\n");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be("Player 0 wins!\r\n");
            }
        }
    }
}
