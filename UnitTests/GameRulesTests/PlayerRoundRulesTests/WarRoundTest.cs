using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.War;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.GameRulesTests.PlayerRoundRulesTests
{
    [TestClass]
    public class WarRoundTest
    {
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private IGameMediator gm;
        private ITurnHandler th;
        private Table table;
        private List<IPlayer> players;
        private List<int> playerIDs;
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize]
        public void InitializeTableTests()
        {
            gm = new BaseGameMediator(2);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            table = new(gm, new List<TableZone>() { zone });
            th = new TurnHandler(2, false);
            gm.RegisterTurnHandler(th);

            players = new();
            for (var i = 0; i < 2; i++)
            {
                List<PlayingCard> cards = table.DrawMultipleCardsFromDeck(26, TablePlacementZoneType.PlayerZone)!.ConvertAll(c => (PlayingCard)c!);
                DeckOfPlayingCards deck = new(cards, defaultAddCardPos: "top");
                players.Add(new WarPlayer(gm, i, deck));
            }

            playerIDs = new();
            foreach (IPlayer player in players)
            {
                playerIDs.Add(player.PlayerID);
            }
        }

        [TestMethod]
        public void WarRoundGoesThroughAllPhasesCorrectly()
        {
            List<PlayingCard> riggedCardsForPlayerZero = new List<PlayingCard>() {
                new PlayingCard(10, "H"), // Win in 2nd comparison phase
                new PlayingCard(2, "H"),
                new PlayingCard(3, "H") // Tie in 1st comparison phase
            };

            List<PlayingCard> riggedCardsForPlayerOne = new()
            {
                new PlayingCard(7, "C"),
                new PlayingCard(2, "C"),
                new PlayingCard(3, "C")
            };

            List<object> resourcesToAdd = riggedCardsForPlayerZero.Cast<object>().ToList();
            players[0].AddMultipleResourcesToCollection(0, resourcesToAdd);

            resourcesToAdd = riggedCardsForPlayerOne.Cast<object>().ToList();
            players[1].AddMultipleResourcesToCollection(0, resourcesToAdd);

            IRoundRules rr = new WarRoundRules(gm, playerIDs);

            rr.StartRound();

            table.PrintTableState();
            IReadOnlyList<IReadOnlyList<ICard>> tableState = table.TableState;

            tableState.Count.Should().Be(2, "there are two players at the table");

            // Since players never played directly from their starting deck, 26 starting cards + 3 Player 0 cards + 3 Player 1 cards
            players[0].CountOfResourceCollection(0).Should().Be(32, "they added 6 cards to their deck");
            players[1].CountOfResourceCollection(0).Should().Be(26, "player 1 lost 3 of their cards");
        }
    }
}
