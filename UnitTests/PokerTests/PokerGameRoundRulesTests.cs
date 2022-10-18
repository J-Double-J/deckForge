using DeckForge.GameConstruction.PresetGames.Poker;
using FluentAssertions;

namespace UnitTests.PokerTests
{
    [TestClass]
    public class PokerGameRoundRulesTests
    {
        [TestMethod]
        public void PokerGoesThroughAllRounds()
        {
            {
                PokerGameMediator pGM = new PokerGameMediator(3);
            }
        }

    }
}
