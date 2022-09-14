using FluentAssertions;
using DeckForge.GameConstruction.PresetGames.Poker;
using DeckForge.GameConstruction.PresetGames.War;
using DeckForge.PhaseActions;
using UnitTests.PokerTests.TestablePokerPlayer;

namespace UnitTests.PokerTests
{
    [TestClass]
    public class DoPreFlopActionTests
    {
        [TestMethod]
        public void PlayerCanCall_ThroughGameAction()
        {
            PokerGameMediator pGM = new(0);
            PokerPlayerWithProgrammedActions player = new(pGM, 0, 100);
            PlayerGameAction action = new DoPreFlopAction();

            pGM.CurrentBet = 10;
            player.Commands.Add("CALL");

            action.Execute(player);

            player.InvestedCash.Should().Be(10, "the player matched the current bet");
        }

        [TestMethod]
        public void PlayerCanRaise_ThroughGameAction()
        {
            PokerGameMediator pGM = new(0);
            PokerPlayerWithProgrammedActions player = new(pGM, 0, 100);
            PlayerGameAction action = new DoPreFlopAction();

            pGM.CurrentBet = 10;
            player.RaiseToAmount = 30;
            player.Commands.Add("RAISE");

            action.Execute(player);

            player.InvestedCash.Should().Be(30, "the player raised the bet to 30");
        }

        [TestMethod]
        public void NonPokerPlayer_CannotRunAction()
        {
            var stringReader = new StringReader("1");
            Console.SetIn(stringReader);
            PokerGameMediator pGM = new(0);
            WarPlayer player = new(pGM, 0, new DeckForge.GameElements.Resources.DeckOfPlayingCards());
            PlayerGameAction action = new DoPreFlopAction();

            pGM.CurrentBet = 10;
            Action attempt = () => action.Execute(player);

            attempt.Should().Throw<ArgumentException>("a non-poker player does not have PreFlop actions");
        }
    }
}