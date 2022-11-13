using FluentAssertions;
using DeckForge.GameRules;
using DeckForge.GameConstruction;
using DeckForge.PlayerConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;

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
            BaseSetUpRules spr = new BaseSetUpRules(deckCount: count);

            spr.Decks.Count().Should().Be(count, "SetUpRules was told to create numerous decks");
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public void AttemptToCreateNegativeOrNilDecks(int count)
        {
            Action act = () => new BaseSetUpRules(deckCount: count);
            act.Should().Throw<ArgumentException>("SetUpRules cannot make a game with no decks");
        }

        [TestMethod]
        public void PlayerShouldHave_CorrectInitHandSize()
        {
            IGameMediator gm = new BaseGameMediator(0);
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 1, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            BaseSetUpRules spr = new BaseSetUpRules(initHandSize: 8);
            IPlayer p = new BasePlayer(gm, initHandSize: spr.InitHandSize);
            p.DrawStartingHand(TablePlacementZoneType.PlayerZone);

            p.HandSize.Should().Be(8, "SetUpRules set the initial hand size to 8");
        }
    }
}
