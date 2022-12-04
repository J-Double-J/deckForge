using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.Dominion;
using DeckForge.GameConstruction.PresetGames.Dominion.Cards;
using DeckForge.GameConstruction.PresetGames.Dominion.Table;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
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
            IGameMediator gm = new DominionGameMediator(1);
            Table table = new(
                gm,
                new List<TableZone>()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { new DominionPlayerTableArea(0) }),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { new DominionMarketTableArea(new()) })
                });
            DominionPlayer player = new(new ConsoleInputMock(new List<string>() { "1" }), new ConsoleOutputMock(), gm, 0);
            player.AddCardToHand(new CopperCard());
            player.PlayCard();

            player.Coins.Should().Be(1, "player played a copper coin card");
        }

        [TestMethod]
        public void PlayerGetsManyCoins_AfterPlayingMultipleTreasureCards()
        {
            IGameMediator gm = new DominionGameMediator(1);
            Table table = new(
                gm,
                new List<TableZone>()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { new DominionPlayerTableArea(0) }),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { new DominionMarketTableArea(new()) })
                });
            DominionPlayer player = new(new ConsoleInputMock(new List<string>() { "1", "1", "1" }), new ConsoleOutputMock(), gm, 0);

            player.AddCardsToHand(new List<ICard>() { new CopperCard(), new SilverCard(), new GoldCard() });
            player.PlayCard();
            player.PlayCard();
            player.PlayCard();

            player.Coins.Should().Be(6, "player played three different treasure cards");
        }

        [TestMethod]
        public void PlayerScoresTheirDeckCorrectly()
        {
            IGameMediator gm = new DominionGameMediator(1);
            Table table = new(
                gm,
                new List<TableZone>() {
                    new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { new DominionPlayerTableArea(0) }),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { new DominionMarketTableArea(new()) })
                });
            DominionPlayer player = new(gm, 0);
            List<ICard> scoreCards = new() { new EstateCard(), new DuchyCard(), new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() };

            player.PlayerDeck.AddMultipleCardsToDeck(scoreCards);

            // Add 3 starting estate cards in deck.
            player.Score().Should().Be(12, "that is the sum of all the victory cards");
        }

        [TestMethod]
        public void PlayerScoresAllLocationsCorrectly()
        {
            IGameMediator gm = new DominionGameMediator(1);
            Table table = new(
                gm,
                new List<TableZone>() {
                    new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { new DominionPlayerTableArea(0) }),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { new DominionMarketTableArea(new()) })
                });
            DominionPlayer player = new(gm, 0);

            player.DiscardPile.AddMultipleCardsToDeck(new List<ICard>() { new EstateCard(), new DuchyCard() });
            player.AddCardsToHand(new List<ICard>() { new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() });

            // Add 3 starting estate cards in deck.
            player.Score().Should().Be(12, "that is the sum of all the victory cards");
        }

        [TestMethod]
        public void WhenPlayerTurnEndsAllCardsNotInDeckGoToDiscard()
        {
            IGameMediator gm = new DominionGameMediator(1);
            DominionPlayerTableArea presetArea = new(0);
            presetArea.PlaceCard(new SilverCard());
            Table table = new(
                gm,
                new List<TableZone>()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { new DominionMarketTableArea(new()) })
                });
            DominionPlayer player = new(gm, 0);

            player.AddCardsToHand(new List<ICard>() { new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() });
            player.EndTurn();

            player.DiscardPile.Count.Should().Be(5, "4 cards were removed from the hand and one from play");
        }

        [TestMethod]
        public void PlayerEndsTheirTurnWithANewHand()
        {
            IGameMediator gm = new DominionGameMediator(1);
            DominionPlayerTableArea presetArea = new(0);
            presetArea.PlaceCard(new SilverCard());
            Table table = new(
                gm,
                new List<TableZone>()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { new DominionMarketTableArea(new()) })
                });
            DominionPlayer player = new(gm, 0);

            player.AddCardsToHand(new List<ICard>() { new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() });
            player.EndTurn();

            player.HandSize.Should().Be(5, "5 cards were drawn from their deck");
        }

        [TestMethod]
        public void PlayerLosesAllRemainingCoinsOnEndTurn()
        {
            IGameMediator gm = new DominionGameMediator(1);
            DominionPlayerTableArea presetArea = new(0);
            presetArea.PlaceCard(new SilverCard());
            Table table = new(
                gm,
                new List<TableZone>()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { new DominionMarketTableArea(new()) })
                });
            DominionPlayer player = new(gm, 0);

            player.AddCardsToHand(new List<ICard>() { new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() });
            player.IncreaseCoins(1);
            player.EndTurn();

            player.Coins.Should().Be(0, "player should no longer have coins after their turn ends");
        }

        [TestMethod]
        public void PlayerCanBuyFromMarketPlace()
        {
            IGameMediator gm = new DominionGameMediator(1);
            DominionPlayerTableArea presetArea = new(0);
            List<IDeck> decksInMarket = new()
            {
                new MonotoneDeck(typeof(CopperCard), 4),
                new MonotoneDeck(typeof(SilverCard), 4),
                new MonotoneDeck(typeof(GoldCard), 4)
            };
            DominionMarketTableArea marketArea = new(decksInMarket);
            Table table = new(
                gm,
                new List<TableZone>()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { marketArea })
                });
            List<string> playerInputs = new() { "2" };
            DominionPlayer player = new(new ConsoleInputMock(playerInputs), new ConsoleOutputMock(), gm, 0);

            player.IncreaseCoins(4);
            ICard purchasedCard = player.Buy()!;

            purchasedCard.Should().BeAssignableTo(typeof(SilverCard), "they purchased the silver");
            player.Coins.Should().Be(1, "they have one coin left");
            player.DiscardPile.Count.Should().Be(1);
        }

        [TestMethod]
        public void PlayerCannotBuy_ExpensiveCard_FromMarketPlace()
        {
            IGameMediator gm = new DominionGameMediator(1);
            DominionPlayerTableArea presetArea = new(0);
            List<IDeck> decksInMarket = new()
            {
                new MonotoneDeck(typeof(CopperCard), 4),
                new MonotoneDeck(typeof(SilverCard), 4),
                new MonotoneDeck(typeof(GoldCard), 4)
            };
            DominionMarketTableArea marketArea = new(decksInMarket);
            Table table = new(
                gm,
                new List<TableZone>()
                {
                    new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }),
                    new TableZone(TablePlacementZoneType.NeutralZone, new List<TableArea>() { marketArea })
                });
            List<string> playerInputs = new() { "3", "-1" };
            DominionPlayer player = new(new ConsoleInputMock(playerInputs), new ConsoleOutputMock(), gm, 0);

            player.IncreaseCoins(4);
            ICard? purchasedCard = player.Buy();

            purchasedCard.Should().BeNull("player didn't end up purchasing a card");
            player.Coins.Should().Be(4, "player didn't end up purchasing a card");
            player.DiscardPile.Count.Should().Be(0, "player didn't end up purchasing a card");
        }
    }
}
