using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;
using FluentAssertions;
using UnitTests.Mocks;

namespace UnitTests.PlayerConstruction
{
    [TestClass]
    public class PlayerWithActionChoicesTests
    {
        [TestMethod]
        public void PlayerCanDrawACardByChoice()
        {
            IGameMediator gm = new BaseGameMediator(1);
            Table table = new(gm, new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, 1, new DeckOfPlayingCards()) });
            List<PlayerGameAction> playerActions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            List<string> actionNames = new() { playerActions[0].Name, playerActions[1].Name };
            PlayerWithActionChoicesMock player = new(gm, 0)
            {
                ActionChoices = actionNames,
                Actions = new Dictionary<string, (IGameAction<DeckForge.PlayerConstruction.IPlayer> Action, int ActionCount)>()
                {
                    { playerActions[0].Name, (playerActions[0], 1) },
                    { playerActions[1].Name, (playerActions[1], 1) }
                }
            };

            player.StartTurn();

            player.HandSize.Should().Be(1, "the player drew a card");
        }

        [TestMethod]
        public void AfterActionItsCountIsReduced()
        {
            IGameMediator gm = new BaseGameMediator(1);
            Table table = new(gm, new List<TableZone>() { new TableZone(TablePlacementZoneType.PlayerZone, 1, new DeckOfPlayingCards()) });
            List<PlayerGameAction> playerActions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            List<string> actionNames = new() { playerActions[0].Name, playerActions[1].Name };
            PlayerWithActionChoicesMock player = new(gm, 0)
            {
                ActionChoices = actionNames,
                Actions = new Dictionary<string, (IGameAction<DeckForge.PlayerConstruction.IPlayer> Action, int ActionCount)>()
                {
                    { playerActions[0].Name, (playerActions[0], 1) },
                    { playerActions[1].Name, (playerActions[1], 1) }
                }
            };

            player.StartTurn(false);

            player.Actions[playerActions[0].Name].ActionCount.Should().Be(0, "it was 1 before and it was done once");
        }
    }
}
