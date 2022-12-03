using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.Dominion;
using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameConstruction.PresetGames.Dominion.Table;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.DominionTests
{
    [TestClass]
    public class DominionPlayerTests
    {
        [TestMethod]
        public void PlayerGetsCoinsAfterPlayingTreasureCard()
        {
            IGameMediator gm = new BaseGameMediator(1);
            Table table = new Table(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { new DominionPlayerTableArea(0) }) });
            DominionPlayer player = new(new ConsoleInputMock(new List<string>() { "0" }), new ConsoleOutputMock(), gm, 0);
            player.AddCardToHand(new CopperCard());
            player.PlayCard();

            player.Coins.Should().Be(1, "player played a copper coin card");
        }

        [TestMethod]
        public void PlayerGetsManyCoins_AfterPlayingMultipleTreasureCards()
        {
            IGameMediator gm = new BaseGameMediator(1);
            Table table = new Table(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { new DominionPlayerTableArea(0) }) });
            DominionPlayer player = new(new ConsoleInputMock(new List<string>() { "0", "0", "0" }), new ConsoleOutputMock(), gm, 0);

            player.AddCardsToHand(new List<ICard>() { new CopperCard(), new SilverCard(), new GoldCard() });
            player.PlayCard();
            player.PlayCard();
            player.PlayCard();

            player.Coins.Should().Be(6, "player played three different treasure cards");
        }

        [TestMethod]
        public void PlayerScoresTheirDeckCorrectly()
        {
            IGameMediator gm = new BaseGameMediator(1);
            Table table = new Table(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { new DominionPlayerTableArea(0) }) });
            DominionPlayer player = new(gm, 0);
            List<ICard> scoreCards = new() { new EstateCard(), new DuchyCard(), new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() };

            player.PlayerDeck.AddMultipleCardsToDeck(scoreCards);

            // Add 3 starting estate cards in deck.
            player.Score().Should().Be(12, "that is the sum of all the victory cards");
        }

        [TestMethod]
        public void PlayerScoresAllLocationsCorrectly()
        {
            IGameMediator gm = new BaseGameMediator(1);
            Table table = new Table(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { new DominionPlayerTableArea(0) }) });
            DominionPlayer player = new(gm, 0);

            player.DiscardPile.AddMultipleCardsToDeck(new List<ICard>() { new EstateCard(), new DuchyCard() });
            player.AddCardsToHand(new List<ICard>() { new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() });

            // Add 3 starting estate cards in deck.
            player.Score().Should().Be(12, "that is the sum of all the victory cards");
        }
    }
}
