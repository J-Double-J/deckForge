using deckForge.GameConstruction;
using deckForge.GameConstruction.PresetGames.War;
using deckForge.GameElements;
using deckForge.GameElements.Resources;
using deckForge.GameRules.RoundConstruction.Interfaces;
using deckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.GameRulesTests.PlayerRoundRulesTests
{
    [TestClass]
    public class WarRoundTest
    {
        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        IGameMediator gm;
        Table table;
        List<IPlayer> players;
        List<int> playerIDs;
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [TestInitialize()]
        public void InitializeTableTests()
        {
            gm = new BaseGameMediator(2);
            table = new(gm, 2, new Deck());

            players = new();
            for (var i = 0; i < 2; i++)
            {
                List<Card> cards = table.DrawMultipleCardsFromDeck(26)!;
                Deck deck = new(cards, defaultAddCardPos: "top");
                players.Add(new WarPlayer(gm, i, deck));
            }

            playerIDs = new();
            foreach (IPlayer player in players)
            {
                playerIDs.Add(player.PlayerID);
            }
        }

        [TestMethod]
        public void WarRoundGoesThroughAllPhasesCorrectly() {
            List<Card> riggedCardsForPlayerZero = new List<Card>() {
                new Card(10, "H"), //Win in 2nd comparison phase
                new Card(2, "H"),
                new Card(3, "H")   //Tie in 1st comparison phase
            };

            List<Card> riggedCardsForPlayerOne = new List<Card>() {
                new Card(7, "C"),
                new Card(2, "C"),
                new Card(3, "C")
            };

            List<object> resourcesToAdd = riggedCardsForPlayerZero.Cast<object>().ToList();
            players[0].AddMultipleResourcesToCollection(0, resourcesToAdd);

            resourcesToAdd = riggedCardsForPlayerOne.Cast<object>().ToList();
            players[1].AddMultipleResourcesToCollection(0, resourcesToAdd);
            
            IRoundRules rr = new WarRoundRules(gm, playerIDs);

            rr.StartRound();

            table.PrintTableState();
            List<List<Card>> tableState = table.TableState;

            tableState.Count.Should().Be(2, "there are two players at the table");
            
            //Since players never played directly from their starting deck, 26 starting cards + 3 Player 0 cards + 3 Player 1 cards
            players[0].CountOfResourceCollection(0).Should().Be(32, "they added 6 cards to their deck");
            players[1].CountOfResourceCollection(0).Should().Be(26, "player 1 lost 3 of their cards");
        }
    }
}
