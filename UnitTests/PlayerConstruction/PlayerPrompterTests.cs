using DeckForge.PlayerConstruction;
using FluentAssertions;

namespace UnitTests.PlayerConstruction
{
    [TestClass]
    public class PlayerPrompterTests
    {
        [TestMethod]
        public void PlayerCanChooseFromPrompt_WithValidChoice() {
            var stringReader = new StringReader("1");
            Console.SetIn(stringReader);
            string prompt = "What number is your favorite?\n" +
                "1) 1!\n" +
                "2) 2!\n" +
                "3) Neither of these.";
            PlayerPrompter pp = new(prompt, 3);

            pp.Prompt().Should().Be(1, "the Player chose option 1.");
        }

        [TestMethod]
        public void PlayerCannotChooseFromPrompt_WithInvalidChoice() {
            var stringReader = new StringReader("J");
            Console.SetIn(stringReader);
            var stringReaderThree = new StringReader("3");
            Console.SetIn(stringReaderThree);
            var stringReaderTwo = new StringReader("100");
            Console.SetIn(stringReaderTwo);
            
            string prompt = "What number is your favorite?\n" +
                "1) 1!\n" +
                "2) 2!\n" +
                "3) Neither of these.";
            PlayerPrompter pp = new(prompt, 3);

            pp.Prompt().Should().Be(3, "the Player chose option 1.");
        }
    }
}
