using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.GameRules.RoundConstruction.Phases;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;
using DeckForge.PlayerConstruction;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.GameRulesTests.PhaseTests
{
    [TestClass]
    public class TurnBasedPhaseTests
    {
        [TestMethod]
        public void PlayerCanDrawCard_WhenPhaseGivesThemATurn()
        {
            IGameMediator gm = new BaseGameMediator(1);
            Table table = new(gm, new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, 1, new DeckOfPlayingCards()) });
            List<PlayerGameAction> actions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            PlayerWithActionChoicesMock playerOne = new(gm, 0)
            {
                Actions = new() { { actions[0].Name, (actions[0], 1) }, { actions[1].Name, (actions[1], 1) } },
                ActionChoices = new() { actions[0].Name, actions[1].Name }
            };
            TurnBasedPhase phase = new(gm, new() { 0 });

            phase.StartPhase();

            playerOne.HandSize.Should().Be(1);
        }

        [TestMethod]
        public void ThreePlayersCanDrawCard_WhenPhaseGivesThemATurn()
        {
            IGameMediator gm = new BaseGameMediator(3);
            Table table = new(gm, new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, 1, new DeckOfPlayingCards()) });
            List<PlayerGameAction> actions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            PlayerWithActionChoicesMock playerOne = new(gm, 0)
            {
                Actions = new() { { actions[0].Name, (actions[0], 1) }, { actions[1].Name, (actions[1], 1) } },
                ActionChoices = new() { actions[0].Name, actions[1].Name }
            };
            PlayerWithActionChoicesMock playerTwo = new(gm, 1)
            {
                Actions = new() { { actions[0].Name, (actions[0], 1) }, { actions[1].Name, (actions[1], 1) } },
                ActionChoices = new() { actions[0].Name, actions[1].Name }
            };
            PlayerWithActionChoicesMock playerThree = new(gm, 2)
            {
                Actions = new() { { actions[0].Name, (actions[0], 1) }, { actions[1].Name, (actions[1], 1) } },
                ActionChoices = new() { actions[0].Name, actions[1].Name }
            };

            TurnBasedPhase phase = new(gm, new() { 0, 1, 2 });

            phase.StartPhase();

            playerOne.HandSize.Should().Be(1);
            playerTwo.HandSize.Should().Be(1);
            playerThree.HandSize.Should().Be(1);
        }
    }
}
