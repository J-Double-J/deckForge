using DeckForge.GameConstruction;
using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
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
    }
}
