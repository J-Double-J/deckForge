using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;
using DeckForge.PlayerConstruction;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace UnitTests.PlayerConstruction
{
    [TestClass]
    public class PlayerActionChoicePrompterTests
    {
        // TODO: Abstract out console reading if you want to test this. As of right now, this will be ignored.
        // [TestMethod]
        public void PromptDisplaysCorrectOptions()
        {
            StringWriter output = new();
            Console.SetOut(output);
            List<PlayerGameAction> actionList = new() { new PlayCardAction(), new EndTurnAction() };
            Dictionary<string, (IGameAction<IPlayer> Action, int ActionCount)> actions = new()
            {
                { actionList[0].Name, (actionList[0], 1) },
                { actionList[1].Name, (actionList[1], 1) }
            };
            PlayerActionChoicePrompter prompter = new(actions);

            prompter.Prompt();

            if (OperatingSystem.IsMacOS())
            {
                output.ToString().Should().Be(
                    "Which action would you like to do?\n" +
                    "1) Play Card [1 left]\n" +
                    "2) End Turn [1 left]\n");
            }
            else if (OperatingSystem.IsWindows())
            {
                output.ToString().Should().Be(
                    "Which action would you like to do?\r\n" +
                    "1) Play Card [1 left]\r\n" +
                    "2) End Turn [1 left]\r\n");
            }
        }
    }
}
