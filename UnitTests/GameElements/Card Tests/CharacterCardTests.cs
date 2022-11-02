using DeckForge.GameConstruction;
using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;
using DeckForge.GameElements.Resources.Cards.Example_Cards;
using DeckForge.PlayerConstruction;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.GameElements.CardTests
{
    [TestClass]
    public class CharacterCardTests
    {
        [TestMethod]
        public void CharacterCardsCanAttackEachOther()
        {
            IGameMediator gm = new BaseGameMediator(0);
            ICharacterCard cardOne = new BaseCharacterCard(gm, 1, 4);
            ICharacterCard cardTwo = new BaseCharacterCard(gm, 2, 5);

            cardOne.Attack(cardTwo);

            ((BaseCharacterCard)cardOne).HealthVal.Should().Be(2);
            ((BaseCharacterCard)cardTwo).HealthVal.Should().Be(4);
        }

        [TestMethod]
        public void CharacterCardsIncreaseStatsBasedOnEvent()
        {
            BaseGameMediator gm = new(1);
            Table table = new(gm, 1);
            TestPlayerMock player = new(gm, 0);
            List<ICard> cards = new List<ICard>()
            {
                new MobPileCharacterCard(gm, 1, 1),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager")
            };

            player.AddCardsToHand(cards);

            for (int i = 0; i < cards.Count; i++)
            {
                player.PlayCard();
            }

            string reasoning = "four other cards were played and the card automatically increases in strength as other" +
                "cards are played";
            ((BaseCharacterCard)table.PlayerZones[0][0]).AttackVal.Should().Be(5, reasoning);
        }

        [TestMethod]
        public void CharacterCardDoesNotIncreaseStatsInHand()
        {
            BaseGameMediator gm = new(1);
            Table table = new(gm, 1);
            TestPlayerMock player = new(gm, 0);
            List<ICard> cards = new List<ICard>()
            {
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new MobPileCharacterCard(gm, 1, 1),
            };

            player.AddCardsToHand(cards);

            for (int i = 0; i < cards.Count; i++)
            {
                player.PlayCard();
            }

            ((CharacterCardWithEffect)cards[2]).AttackVal.Should().Be(1, "card plays while in hand don't affect card");
        }

        [TestMethod]
        public void CharacterCardIncreasesWhenPlayedBeforeAndAfterOtherCards()
        {
            BaseGameMediator gm = new(1);
            Table table = new(gm, 1);
            TestPlayerMock player = new(gm, 0);
            List<ICard> cards = new List<ICard>()
            {
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new MobPileCharacterCard(gm, 1, 1),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
            };

            player.AddCardsToHand(cards);

            for (int i = 0; i < cards.Count; i++)
            {
                player.PlayCard();
            }

            ((CharacterCardWithEffect)cards[2]).AttackVal.Should().Be(3, "card plays while in hand don't affect card");
        }

        [TestMethod]
        public void BrawlerCardIncreasesInStrengthWithOtherCharacterCards()
        {
            BaseGameMediator gm = new(1);
            Table table = new(gm, 1);
            TestPlayerMock player = new(gm, 0);
            List<ICard> cards = new()
            {
                new BarBrawlerCharacterCard(gm),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
            };

            player.AddCardsToHand(cards);

            for (int i = 0; i < cards.Count; i++)
            {
                player.PlayCard();
            }

            ((CharacterCardWithEffect)table.PlayerZones[0][0]).AttackVal.Should()
                .Be(4, "there are two other character cards in play");
        }

        [TestMethod]
        public void BrawlerCardDecreasesInStrengthWhenCharacterCardsDie()
        {
            BaseGameMediator gm = new(1);
            Table table = new(gm, 1);
            TestPlayerMock player = new(gm, 0);
            List<ICard> cards = new()
            {
                new BarBrawlerCharacterCard(gm),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
                new BaseCharacterCard(gm, 1, 1, "Poor Villager"),
            };

            player.AddCardsToHand(cards);

            for (int i = 0; i < cards.Count; i++)
            {
                player.PlayCard();
            }

            ((BaseCharacterCard)cards[1]).Attack((BaseCharacterCard)cards[2]);

            ((CharacterCardWithEffect)table.PlayerZones[0][0]).AttackVal.Should()
                .Be(2, "the two other cards killed themselves and the brawler no longer benefits from them");
        }
    }
}
