﻿using DeckForge.GameConstruction;
using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;
using DeckForge.PhaseActions.NonPlayerActions;
using FluentAssertions;

namespace UnitTests.ActionTests.NonPlayerActionTests
{
    [TestClass]
    public class MoveAllCardsFromTableToTableDeckActionTest
    {
        [TestMethod]
        public void CardsAreAddedToTheDeck()
        {
            IGameMediator gm = new BaseGameMediator(1);
            Table table = new(gm, 1, new DeckOfPlayingCards(), 2);
            BaseGameAction action = new MoveAllCardsFromTableToTableDeckAction(gm, 0);

            table.AddCardsTo_NeutralZone(table.DrawMultipleCardsFromDeck(2)!, 0);
            table.AddCardsTo_NeutralZone(table.DrawMultipleCardsFromDeck(2)!, 1);

            action.Execute();

            table.TableDecks[0].Count.Should().Be(52, "all the cards were readded to the deck");
        }
    }
}
