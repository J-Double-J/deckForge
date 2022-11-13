using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;
using DeckForge.GameElements.Table;
using DeckForge.HelperObjects;
using FluentAssertions;

namespace UnitTests.GameElements
{
    [TestClass]
    public class TableZoneTests
    {
        [TestMethod]
        public void CanPlayCardToZone()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 1);

            tableZone.PlayCardToArea(card, 0);

            tableZone.GetCardsInArea(0)[0].Should().BeSameAs(card, "the card was added to the area");
        }

        [TestMethod]
        public void CanPlayCardToSpecificPlaceInArea()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 1, 5);

            tableZone.PlayCardToArea(card, 0, 3);

            tableZone.GetCardsInArea(0)[3].Should().BeSameAs(card, "this card was added to a specific spot");
        }

        [TestMethod]
        public void CannotPlayCardToSpecificPlaceInArea_WithoutSpecifiedAreaSize()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 1);

            Action a = () => tableZone.PlayCardToArea(card, 0, 3);

            a.Should().Throw<ArgumentOutOfRangeException>("it is unclear where to put a card in an area without bounds");
        }

        public void CannotPlayCardToArea_IfAllSpotsFilled()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 1, 2);

            tableZone.PlayCardToArea(new PlayingCard(2, "S"), 0);
            tableZone.PlayCardToArea(new PlayingCard(3, "S"), 0);
            Action a = () => tableZone.PlayCardToArea(card, 0);

            a.Should().Throw<InvalidOperationException>("there is no where to place the card as all spots are filled");
        }

        [TestMethod]
        public void CannotPlayCardToArea_WithPlacementSpecified_IfAllSpotsAreFilled()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 1, 2);

            tableZone.PlayCardToArea(new PlayingCard(2, "S"), 0);
            tableZone.PlayCardToArea(new PlayingCard(3, "S"), 0);
            Action a = () => tableZone.PlayCardToArea(card, 0, 1);

            a.Should().Throw<InvalidOperationException>("this spot is already filled");
        }

        [TestMethod]
        public void CanPlayMultipleCardsToZone()
        {
            List<ICard> cards = new() { new PlayingCard(1, "S"), new PlayingCard(2, "S") };
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 1);

            tableZone.PlayCardsToArea(cards, 0);

            tableZone.GetCardsInArea(0)[0].Should().BeSameAs(cards[0], "the card was added to the area");
            tableZone.GetCardsInArea(0)[1].Should().BeSameAs(cards[1], "the card was added to the area");
        }

        [TestMethod]
        public void CanPlaceCardToZone()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new TableZone(TablePlacementZoneType.PlayerZone, 1);

            tableZone.PlaceCardToArea(card, 0);

            tableZone.GetCardsInArea(0)[0].Should().BeSameAs(card, "this card was placed to the area");
        }

        [TestMethod]
        public void CanPlaceCardToZone_ToSpecificPlaceInArea()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new TableZone(TablePlacementZoneType.PlayerZone, 1, 2);

            tableZone.PlaceCardToArea(card, 0, 1);

            tableZone.GetCardsInArea(0)[1].Should().BeSameAs(card, "this card was placed to the area");
        }

        [TestMethod]
        public void CanRemoveCardFromZone()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 1);

            tableZone.PlayCardToArea(card, 0);

            tableZone.RemoveCard(card, 0).Should().Be(true, "card should be located and removed");
            tableZone.GetCardsInArea(0).Count.Should().Be(0, "there should be no cards left in area");
        }

        [TestMethod]
        public void AreaIsFilledWithNullCards_IfAreaLimitSpecified()
        {
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 1, 4);

            tableZone.GetCardsInArea(0).Count.Should().Be(4, "there are 4 NullCards");

            foreach (ICard card in tableZone.GetCardsInArea(0))
            {
                card.Should().BeAssignableTo(typeof(NullCard), "all cards should be null cards");
            }
        }

        [TestMethod]
        public void CanDrawFromDeck_InZone()
        {
            IDeck deck = new DeckOfPlayingCards();
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 1, deck);

            tableZone.DrawCardFromZone();

            tableZone.Decks![0].Count.Should().Be(51, "a card was drawn from the deck");
        }

        [TestMethod]
        public void CanDrawFromDeck_InArea()
        {
            List<IDeck> decks = new() { new DeckOfPlayingCards(), new DeckOfPlayingCards(), new DeckOfPlayingCards() };
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 3, decks);

            tableZone.DrawCardFromZone(2);

            tableZone.Decks![0].Count.Should().Be(52, "the deck was untouched");
            tableZone.Decks![2].Count.Should().Be(51, "a card was drawn");
        }

        [TestMethod]
        public void PlayingCardToZone_TriggersOnPlayEffect()
        {
            IGameMediator gm = new BaseGameMediator(1);
            ICard card = new BaseCharacterCard(gm, 1, 1);
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 2);

            tableZone.PlayCardToArea(card, 1);

            gm.GetCurrentCardModifierValue(CardModifiers.CharacterCardsInPlayerZones).Should().Be(1);
        }

        [TestMethod]
        public void PlayingCardToZone_TriggersOnPlaceEffect()
        {
            IGameMediator gm = new BaseGameMediator(1);
            ICard card = new BaseCharacterCard(gm, 1, 1);
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 2);

            tableZone.PlaceCardToArea(card, 1);

            gm.GetCurrentCardModifierValue(CardModifiers.CharacterCardsInPlayerZones).Should().Be(1);
        }

        [TestMethod]
        public void RemovingCardFromZone_WithPlacementSpecification_TriggersOnRemovalEffect()
        {
            IGameMediator gm = new BaseGameMediator(1);
            ICard card = new BaseCharacterCard(gm, 1, 1);
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 2);

            tableZone.PlayCardToArea(card, 1);
            tableZone.RemoveCard(1, 0);

            gm.GetCurrentCardModifierValue(CardModifiers.CharacterCardsInPlayerZones).Should().Be(0);
        }

        [TestMethod]
        public void RemovingCardFromZone_WithCardAndArea_TriggersOnRemovalEffect()
        {
            IGameMediator gm = new BaseGameMediator(1);
            ICard card = new BaseCharacterCard(gm, 1, 1);
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 2);

            tableZone.PlayCardToArea(card, 1);
            tableZone.RemoveCard(card, 1);

            gm.GetCurrentCardModifierValue(CardModifiers.CharacterCardsInPlayerZones).Should().Be(0);
        }

        [TestMethod]
        public void RemovingCardFromZone_WithCardAndPlacementSpecification_TriggersOnRemovalEffect()
        {
            IGameMediator gm = new BaseGameMediator(1);
            ICard card = new BaseCharacterCard(gm, 1, 1);
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 2);

            tableZone.PlayCardToArea(card, 1);
            tableZone.RemoveCard(card, 1, 0);

            gm.GetCurrentCardModifierValue(CardModifiers.CharacterCardsInPlayerZones).Should().Be(0);
        }

        [TestMethod]
        public void InvalidAreaPassedTo_RemoveCard()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 2);

            tableZone.PlayCardToArea(card, 1);
            Action a = () => tableZone.RemoveCard(card, 2, 0);
            Action b = () => tableZone.RemoveCard(card, -1, 0);

            a.Should().Throw<ArgumentOutOfRangeException>("there is no Area ID 2, only 0 and 1");
            b.Should().Throw<ArgumentOutOfRangeException>("there is no Area ID -1, only 0 and 1");
        }

        [TestMethod]
        public void InvalidPlacementPassedTo_RemoveCard()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 2);

            tableZone.PlayCardToArea(card, 1);
            Action a = () => tableZone.RemoveCard(card, 1, 4);
            Action b = () => tableZone.RemoveCard(card, 1, -1);

            a.Should().Throw<ArgumentOutOfRangeException>("there is no Area ID 2, only 0 and 1");
            b.Should().Throw<ArgumentOutOfRangeException>("there is no Area ID -1, only 0 and 1");
        }

        [TestMethod]
        public void RemoveCard_WhereOnlyNullCardsExist()
        {
            ICard card = new PlayingCard(1, "S");
            TableZone tableZone = new(TablePlacementZoneType.PlayerZone, 2, 4);

            tableZone.RemoveCard(1, 3).Should().Be(false, "no card was removed because only NullCards are there");
        }
    }
}
