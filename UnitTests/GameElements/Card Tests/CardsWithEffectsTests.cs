using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;
using DeckForge.GameElements.Table;
using DeckForge.PlayerConstruction;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.GameElements.Card_Tests
{
    [TestClass]
    public class CardsWithEffectsTests
    {
        [TestMethod]
        public void CardEffectCausesPlayerToGainCard_WhenPlayed()
        {
            IGameMediator gm = new BaseGameMediator(1);
            IPlayer player = new TestPlayerMock(gm, 0);
            Table table = new(gm, 1, new DeckOfPlayingCards());
            ICard card = new DrawCardEffectCard(gm);

            player.AddCardToHand(card);
            player.PlayCard();

            player.HandSize.Should().Be(1, "Player gains a card from playing the draw effect card");
        }

        [TestMethod]
        public void CardCanChangeNumberOfCardsDrawn_WhenPlayerd()
        {
            IGameMediator gm = new BaseGameMediator(1);
            IPlayer player = new TestPlayerMock(gm, 0);
            Table table = new(gm, 1, new DeckOfPlayingCards());
            ICard card = new DrawCardEffectCard(gm);

            player.AddCardToHand(card);
            ((DrawCardEffectCard)card).DrawCount = 3;
            player.PlayCard();

            player.HandSize.Should().Be(3, "Player gains a card from playing the draw effect card");
        }
    }
}
