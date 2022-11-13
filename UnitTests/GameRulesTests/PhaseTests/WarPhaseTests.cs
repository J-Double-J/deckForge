using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.War;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.GameRules;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.PhaseTests
{
    [TestClass]
    public class WarPhaseTests
    {
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static IGameMediator gm;
        private static Table table;
        private List<IPlayer> players;
        private List<int> playerIDs;
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void InitializeTableTests()
        {
            gm = new BaseGameMediator(2);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            table = new(gm, new List<TableZone>() { zone });

            players = new();
            for (var i = 0; i < 2; i++)
            {
                List<PlayingCard> cards = table.DrawMultipleCardsFromDeck(26, TablePlacementZoneType.PlayerZone)!.ConvertAll(c => (PlayingCard)c!);
                DeckOfPlayingCards deck = new(cards);
                players.Add(new WarPlayer(gm, i, deck));
            }

            playerIDs = new();
            foreach (IPlayer player in players)
            {
                playerIDs.Add(player.PlayerID);
            }
        }

        [TestMethod]
        public void WarPlaysCardsPhase()
        {

            IPhase phase = new WarPlayCardsPhase(gm, playerIDs, "Play Cards Phase");

            phase.StartPhase();

            table.PrintTableState();

            table.TableState.Count.Should().Be(2, "there are two players at the table");
            foreach (List<ICard> playedCardsInFrontOfPlayer in table.TableState) {
                playedCardsInFrontOfPlayer.Count().Should().Be(1, "only one card was drawn and played in front of each player");
                playedCardsInFrontOfPlayer[0].Facedown.Should().Be(false, "the players were told to flip their cards faceup");
            }
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)] // Player two wins
        public void WarComparesCardsPhase_PlayerWins(bool playerZeroWins)
        {
            PlayingCard cardOne = new(7, "C");
            PlayingCard cardTwo = new(5, "H");

            WarComparePhase comparePhase = new(gm, playerIDs, "Compare Phase");
            if (playerZeroWins)
            {
                table.PlayCardTo_PlayerZone(0, cardOne);
                table.PlayCardTo_PlayerZone(1, cardTwo);
                comparePhase.FlippedCards.Add(cardOne);
                comparePhase.FlippedCards.Add(cardTwo);
            }
            else
            {
                table.PlayCardTo_PlayerZone(1, cardOne);
                table.PlayCardTo_PlayerZone(0, cardTwo);
                comparePhase.FlippedCards.Add(cardTwo);
                comparePhase.FlippedCards.Add(cardOne);
            }

            IPhase phase = comparePhase;
            phase.StartPhase();

            // Note: Player doesn't play out of their deck in this test, so they have init hand size (26)+2 cards picked up
            if (playerZeroWins)
            {
                players[0].CountOfResourceCollection(0).Should().Be(28, "IPlayer took their card and opponent's off the Table and put it in their deck.");
            }
            else
            {
                players[1].CountOfResourceCollection(0).Should().Be(28, "IPlayer took their card and opponent's off the Table and put it in their deck.");
            }
        }

        [TestMethod]
        public void NoClearWinnerInComparePhase()
        {
            PlayingCard cardOne = new(5, "C");
            PlayingCard cardTwo = new(5, "H");

            table.PlayCardTo_PlayerZone(0, cardOne);
            table.PlayCardTo_PlayerZone(1, cardTwo);

            WarComparePhase comparePhase = new(gm, playerIDs, "Compare Phase");
            comparePhase.FlippedCards.Add(cardOne);
            comparePhase.FlippedCards.Add(cardTwo);

            IPhase phase = comparePhase;
            phase.StartPhase();

            // Player did not play a card directly out of their deck, so they have init deck size (26)
            players[0].CountOfResourceCollection(0).Should().Be(26, "their card is still on the table and they didn't pick up any other cards");
            players[1].CountOfResourceCollection(0).Should().Be(26, "their card is still on the table and they didn't pick up any other cards");
        }

        [TestMethod]
        public void WarPhasePlacesMoreCardsOnTable()
        {
            table.PlayCardTo_PlayerZone(0, new PlayingCard(1, "H"));
            table.PlayCardTo_PlayerZone(1, new PlayingCard(2, "H"));

            IPhase phase = new WarPhase(gm, playerIDs, "War!");
            phase.StartPhase();

            table.PrintTableState();
            IReadOnlyList<IReadOnlyList<ICard>> tableState = table.TableState;

            tableState.Count.Should().Be(2, "there are two players at the table");
            foreach (List<ICard> playedCardsInFrontOfPlayer in tableState)
            {
                playedCardsInFrontOfPlayer.Count.Should().Be(3, "player played 1 card before, then in War! phase played 2 more cards");
                playedCardsInFrontOfPlayer[1].Facedown.Should().Be(true, "the 2nd card should be facedown");
                playedCardsInFrontOfPlayer[2].Facedown.Should().Be(false, "the 3rd card was flipped faceup");
            }
        }
    }
}
