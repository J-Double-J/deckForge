using deckForge.GameConstruction;
using deckForge.GameConstruction.PresetGames.War;
using deckForge.GameElements;
using deckForge.GameElements.Resources;
using deckForge.GameRules;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.PhaseTests
{
    [TestClass]
    public class War_PlayCardsPhaseTest
    {
        [TestMethod]
        public void WarPlaysCardsPhase() {
            IGameMediator gm = new BaseGameMediator(2);
            Table table = new(gm, 2, new Deck());
            BaseSetUpRules spr = new(initHandSize: 26);

            List<IPlayer> players = new();
            for (var i = 0; i < 2; i++)
            {
                List<Card> cards = table.DrawMultipleCardsFromDeck(26)!;
                Deck deck = new(cards);
                players.Add(new WarPlayer(gm, i, deck));
            }

            List<int> playerIDs = new();
            foreach (IPlayer player in players)
            {
                playerIDs.Add(player.PlayerID);
            }

            IPhase phase = new WarPlayCardsPhase(gm, playerIDs, "Play Cards Phase");

            phase.StartPhase();

            table.PrintTableState();

            table.TableState.Count.Should().Be(2, "there are two players at the table");
            foreach (List<Card> playedCardsInFrontOfPlayer in table.TableState) {
                playedCardsInFrontOfPlayer.Count().Should().Be(1, "only one card should be in front of player");
                playedCardsInFrontOfPlayer[0].Facedown.Should().Be(false, "the players were told to flip their cards faceup");
            }
        }


    }
}

