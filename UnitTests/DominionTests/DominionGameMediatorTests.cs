using DeckForge.GameConstruction.PresetGames.Dominion;
using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameConstruction.PresetGames.Dominion.DominionTableAreas;
using DeckForge.GameElements.Table;
using DeckForge.HelperObjects;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.DominionTests
{
    [TestClass]
    public class DominionGameMediatorTests
    {
        [TestMethod]
        public void MediatorDeclaresCorrectWinner()
        {
            var output = new ConsoleOutputMock();
            DominionGameMediator gm = new(new ConsoleInputMock(new List<string>()), output, 4);
            List<TableArea> playerAreas = new()
            {
                new DominionPlayerTableArea(0), new DominionPlayerTableArea(1), new DominionPlayerTableArea(2), new DominionPlayerTableArea(3),
            };
            Table table = new(
                gm,
                new()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, playerAreas),
                    new TableZone(TablePlacementZoneType.NeutralZone, new() { new DominionMarketTableArea(new()) })
                });

            List<DominionPlayer> players = new();
            for (int i = 0; i < 4; i++)
            {
                players.Add(new DominionPlayer(gm, i));
            }

            players[0].AddCardsToHand(new() { new ProvinceCard(), new DuchyCard(), new EstateCard() }); // 10 pts
            players[1].AddCardsToHand(new() { new ProvinceCard(), new ProvinceCard(), new EstateCard() }); // 13 pts
            players[2].AddCardsToHand(new() { new ProvinceCard(), new EstateCard() }); // 7 pts
            players[3].AddCardsToHand(new() { new ProvinceCard(), new EstateCard(), new EstateCard() }); // 8 pts

            gm.EndGame();

            // All players have 3 points in their starting deck.
            List<string> expectedOutput = new()
            {
                "Player 1 wins!\n",
                "Player 1: 16 points",
                "Player 0: 13 points",
                "Player 3: 11 points",
                "Player 2: 10 points"
            };

            output.CompleteOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
