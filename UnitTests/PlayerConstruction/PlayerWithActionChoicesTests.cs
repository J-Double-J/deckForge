using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.PlayerActions;
using DeckForge.PlayerConstruction;
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

        [TestMethod]
        public void ActionCanBeGained()
        {
            IGameMediator gm = new BaseGameMediator(1);
            List<PlayerGameAction> playerActions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            List<string> actionNames = new() { playerActions[0].Name, playerActions[1].Name };
            PlayerWithActionChoicesMock player = new(gm, 0)
            {
                Actions = new Dictionary<string, (IGameAction<DeckForge.PlayerConstruction.IPlayer> Action, int ActionCount)>()
                {
                    { playerActions[0].Name, (playerActions[0], 1) },
                    { playerActions[1].Name, (playerActions[1], 1) }
                }
            };

            player.GainAction(new DrawCardsAction(TablePlacementZoneType.PlayerZone), 1);
            player.Actions[actionNames[0]].ActionCount.Should().Be(2);
        }

        [TestMethod]
        public void NewActionCanBeGained()
        {
            IGameMediator gm = new BaseGameMediator(1);
            List<PlayerGameAction> playerActions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            PlayerGameAction newAction = new PlayCardAction();
            PlayerWithActionChoicesMock player = new(gm, 0)
            {
                Actions = new Dictionary<string, (IGameAction<DeckForge.PlayerConstruction.IPlayer> Action, int ActionCount)>()
                {
                    { playerActions[0].Name, (playerActions[0], 1) },
                    { playerActions[1].Name, (playerActions[1], 1) }
                }
            };

            player.GainAction(newAction);
            player.Actions[newAction.Name].ActionCount.Should().Be(1);
        }

        [TestMethod]
        public void ActionCanBeLost()
        {
            IGameMediator gm = new BaseGameMediator(1);
            List<PlayerGameAction> playerActions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            List<string> actionNames = new() { playerActions[0].Name, playerActions[1].Name };
            PlayerWithActionChoicesMock player = new(gm, 0)
            {
                Actions = new Dictionary<string, (IGameAction<DeckForge.PlayerConstruction.IPlayer> Action, int ActionCount)>()
                {
                    { playerActions[0].Name, (playerActions[0], 1) },
                    { playerActions[1].Name, (playerActions[1], 1) }
                }
            };

            player.LoseAction(new DrawCardsAction(TablePlacementZoneType.PlayerZone));
            player.Actions[actionNames[0]].ActionCount.Should().Be(0);

            player.LoseAction(new DrawCardsAction(TablePlacementZoneType.PlayerZone));
            player.Actions[actionNames[0]].ActionCount.Should().NotBe(-1);
        }

        [TestMethod]
        public void ActionThatPlayer_DoesNotHave_CannotBeLost_DoesNothing()
        {
            IGameMediator gm = new BaseGameMediator(1);
            List<PlayerGameAction> playerActions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            List<string> actionNames = new() { playerActions[0].Name, playerActions[1].Name };
            PlayerGameAction newAction = new PlayCardAction();
            PlayerWithActionChoicesMock player = new(gm, 0)
            {
                Actions = new Dictionary<string, (IGameAction<DeckForge.PlayerConstruction.IPlayer> Action, int ActionCount)>()
                {
                    { playerActions[0].Name, (playerActions[0], 1) },
                    { playerActions[1].Name, (playerActions[1], 1) }
                }
            };

            player.LoseAction(newAction);

            player.Actions[actionNames[0]].ActionCount.Should().Be(1);
            player.Actions[actionNames[1]].ActionCount.Should().Be(1);
            player.Actions.ContainsKey(newAction.Name).Should().BeFalse();
        }

        [TestMethod]
        public void DefaultActionsCanBeGained_AndSeenAfterEndTurn()
        {
            IGameMediator gm = new BaseGameMediator(1);
            List<PlayerGameAction> playerActions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            List<string> actionNames = new() { playerActions[0].Name, playerActions[1].Name };
            PlayerGameAction newAction = new PlayCardAction();
            PlayerWithActionChoicesMock player = new(gm, 0)
            {
                Actions = new Dictionary<string, (IGameAction<DeckForge.PlayerConstruction.IPlayer> Action, int ActionCount)>()
                {
                    { playerActions[0].Name, (playerActions[0], 1) },
                    { playerActions[1].Name, (playerActions[1], 1) }
                }
            };

            player.GainDefaultAction(newAction);
            player.DefaultActions[newAction.Name].ActionCount.Should().Be(1);

            player.EndTurn();
            player.Actions[newAction.Name].ActionCount.Should().Be(1);
        }

        [TestMethod]
        public void DefaultActionsCanBeLost_AndSeenAfterEndTurn()
        {
            IGameMediator gm = new BaseGameMediator(1);
            List<PlayerGameAction> playerActions = new() { new DrawCardsAction(TablePlacementZoneType.PlayerZone), new EndTurnAction() };
            List<string> actionNames = new() { playerActions[0].Name, playerActions[1].Name };
            PlayerWithActionChoicesMock player = new(gm, 0)
            {
                Actions = new Dictionary<string, (IGameAction<DeckForge.PlayerConstruction.IPlayer> Action, int ActionCount)>()
                {
                    { playerActions[0].Name, (playerActions[0], 1) },
                    { playerActions[1].Name, (playerActions[1], 1) }
                }
            };

            player.SetDefaultActions(player.Actions);

            player.LoseDefaultAction(new DrawCardsAction(TablePlacementZoneType.PlayerZone));
            player.DefaultActions[actionNames[0]].ActionCount.Should().Be(0);

            player.EndTurn();
            player.Actions[actionNames[0]].ActionCount.Should().Be(0);
        }
    }
}
