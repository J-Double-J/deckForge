using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using FluentAssertions;

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
    }
}
