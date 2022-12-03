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
            IGameMediator gm = new BaseGameMediator(1);
            Table table = new(
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
            Table table = new(
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
            Table table = new(
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
            Table table = new(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { new DominionPlayerTableArea(0) }) });
            DominionPlayer player = new(gm, 0);

            player.DiscardPile.AddMultipleCardsToDeck(new List<ICard>() { new EstateCard(), new DuchyCard() });
            player.AddCardsToHand(new List<ICard>() { new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() });

            // Add 3 starting estate cards in deck.
            player.Score().Should().Be(12, "that is the sum of all the victory cards");
        }

        [TestMethod]
        public void WhenPlayerTurnEndsAllCardsNotInDeckGoToDiscard()
        {
            IGameMediator gm = new BaseGameMediator(1);
            DominionPlayerTableArea presetArea = new DominionPlayerTableArea(0);
            presetArea.PlaceCard(new SilverCard());
            Table table = new(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }) });
            DominionPlayer player = new(gm, 0);

            player.AddCardsToHand(new List<ICard>() { new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() });
            player.EndTurn();

            player.DiscardPile.Count.Should().Be(5, "4 cards were removed from the hand and one from play");
        }

        [TestMethod]
        public void PlayerEndsTheirTurnWithANewHand()
        {
            IGameMediator gm = new BaseGameMediator(1);
            DominionPlayerTableArea presetArea = new DominionPlayerTableArea(0);
            presetArea.PlaceCard(new SilverCard());
            Table table = new(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }) });
            DominionPlayer player = new(gm, 0);

            player.AddCardsToHand(new List<ICard>() { new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() });
            player.EndTurn();

            player.HandSize.Should().Be(5, "5 cards were drawn from their deck");
        }

        [TestMethod]
        public void PlayerLosesAllRemainingCoinsOnEndTurn()
        {
            IGameMediator gm = new BaseGameMediator(1);
            DominionPlayerTableArea presetArea = new DominionPlayerTableArea(0);
            presetArea.PlaceCard(new SilverCard());
            Table table = new(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }) });
            DominionPlayer player = new(gm, 0);

            player.AddCardsToHand(new List<ICard>() { new ProvinceCard(), new CurseCard(), new CopperCard(), new SilverCard() });
            player.IncreaseCoins(1);
            player.EndTurn();

            player.Coins.Should().Be(0, "player should no longer have coins after their turn ends");
        }

        [TestMethod]
        public void PlayerGainsBenefitsFromPlayingVillageCard()
        {
            IGameMediator gm = new BaseGameMediator(1);
            PlayerGameAction action = new PlayCardAction();
            DominionPlayerTableArea presetArea = new DominionPlayerTableArea(0);
            Table table = new(
                gm,
                new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, new List<TableArea>() { presetArea }) });
            DominionPlayer player = new(new ConsoleInputMock(new() { "0" }), new ConsoleOutputMock(), gm, 0);

            player.AddCardToHand(new VillageCard());
            player.PlayCard();

            player.Actions[action.Name].ActionCount.Should().Be(2, "2 actions were gained from playing the Village");
            player.HandSize.Should().Be(1, "player drew a card from playing the Village");
        }
    }
}
