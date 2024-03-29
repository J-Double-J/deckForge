﻿using DeckForge.PhaseActions;
using FluentAssertions;
using DeckForge.GameConstruction;
using DeckForge.PlayerConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Table;

namespace UnitTests.ActionTests
{
    [TestClass]
    public class DrawCardsTest
    {
        [TestMethod]
        public void DrawAction_MakesPlayerDrawCard()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            IPlayer p = new BasePlayer(gm);
            PlayerGameAction action = new DrawCardsAction(TablePlacementZoneType.PlayerZone);

            int initHandSize = p.HandSize;

            action.Execute(p);
            p.HandSize.Should().Be(initHandSize + 1, "player was told to draw a card.");

            PlayerGameAction specifiedDraw = new DrawCardsAction(TablePlacementZoneType.PlayerZone, drawCount: 5);
            specifiedDraw.Execute(p);
            p.HandSize.Should().Be(initHandSize + 6, "player was told to draw 5 more cards");
        }

        [TestMethod]
        public void DrawAction_CantDrawFromEmptyDeck()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            IPlayer p = new BasePlayer(gm);
            PlayerGameAction action = new DrawCardsAction(TablePlacementZoneType.PlayerZone, drawCount: 5);

            int cardsToDraw = 52 - p.HandSize;
            for (int i = 0; i < cardsToDraw; i++)
            {
                p.DrawCard(TablePlacementZoneType.PlayerZone);
            }

            action.Execute(p);
            p.HandSize.Should().Be(52, "the deck was completely drawn from, so there should be no more cards to gain");
        }

        [TestMethod]
        public void UnsupportedExecutes_ThrowErrors()
        {
            IGameMediator gm = new BaseGameMediator(0);
            List<IDeck> decks = new() { new DeckOfPlayingCards() };
            TableZone zone = new(TablePlacementZoneType.PlayerZone, 2, new DeckOfPlayingCards());
            Table table = new(gm, new List<TableZone>() { zone });
            IPlayer p = new BasePlayer(gm);
            IPlayer p2 = new BasePlayer(gm);
            IPlayer p3 = new BasePlayer(gm);
            List<IPlayer> targetPlayers = new List<IPlayer>
            {
                p2, p3
            };

            PlayerGameAction action = new DrawCardsAction(TablePlacementZoneType.PlayerZone, drawCount: 5);

            Action a = () => action.Execute(p, p2);
            Action b = () => action.Execute(p, targetPlayers);

            a.Should().Throw<NotSupportedException>("this method does not allow Players to target draws against one another");
            b.Should().Throw<NotSupportedException>("this method does not allow Players to target draws against one another");
        }
    }
}
