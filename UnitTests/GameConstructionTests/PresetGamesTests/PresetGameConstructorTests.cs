using DeckForge.GameConstruction.PresetGames;
using FluentAssertions;

namespace UnitTests.GameConstructionTests.PresetGamesTests
{
    [TestClass]
    public class PresetGameConstructorTests
    {

        [TestMethod]
        public void PassInvalidGameName()
        {
            string name = "ThisIsAFakeGameName";

            Action a = () => new PresetGameConstructor(name);
            a.Should().Throw<ArgumentException>($"because there is no game called {name}");
        }

        [TestMethod]
        [DataRow("war")]
        [DataRow("WAR")]
        [DataRow("wAr")]
        public void PassValidGameName_WithVariousCasing(string name)
        {
            PresetGameConstructor pgc = new PresetGameConstructor(name);

            pgc.IsValidGame().Should().Be(true, "the game should be able to be found and be case insensitive");
        }
    }
}
