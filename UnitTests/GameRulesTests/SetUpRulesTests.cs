using DeckNameSpace;
using FluentAssertions;
using deckForge.GameRules;
using deckForge.GameConstruction;
using deckForge.PlayerConstruction;

namespace UnitTests.GameRulesTests
{
    [TestClass]
    public class SetUpRulesTests
    {
        [TestMethod]
        [DataRow(1)]
        [DataRow(3)]
        public void CreateNumerousDeck(int count)
        {
            SetUpRules spr = new SetUpRules(deckCount: count);

            spr.Decks.Count().Should().Be(count, "SetUpRules was told to create numerous decks");
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void AttemptToCreateNegativeOrNilDecks(int count)
        {
            Action act = () => new SetUpRules(deckCount: count);
            act.Should().Throw<ArgumentException>("SetUpRules cannot make a game with no decks");
        }

        [TestMethod]
        public void PlayerShouldHave_CorrectInitHandSize()
        {
            GameMediator gm = new(0);
            SetUpRules spr = new SetUpRules(initHandSize: 8);
            Player p = new(gm, initHandSize: spr.InitHandSize);

            p.HandSize.Should().Be(8, "SetUpRules set the initial hand size to 8");
        }
    }
}
