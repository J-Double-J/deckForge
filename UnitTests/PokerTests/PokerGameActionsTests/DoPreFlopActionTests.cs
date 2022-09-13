using FluentAssertions;
using DeckForge.GameConstruction.PresetGames.Poker;
using DeckForge.GameConstruction.PresetGames.War;
using DeckForge.PhaseActions;

namespace UnitTests.PokerTests
{
    [TestClass]
    public class DoPreFlopActionTests
    {
        [TestMethod]
        public void PlayerCanCall_ThroughGameAction()
        {
            var stringReader = new StringReader("1");
            Console.SetIn(stringReader);
            PokerGameMediator pGM = new(0);
            PokerPlayer player = new(pGM, 0, 100);
            PlayerGameAction action = new DoPreFlopAction();

            pGM.CurrentBet = 10;
            action.Execute(player);

            player.InvestedCash.Should().Be(10, "the player matched the current bet");
        }

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