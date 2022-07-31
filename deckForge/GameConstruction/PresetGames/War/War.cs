﻿using DeckForge.GameRules;
using DeckForge.GameElements;
using DeckForge.PlayerConstruction;
using DeckForge.GameConstruction;
using DeckForge.GameRules.RoundConstruction.Rounds;
using DeckForge.GameElements.Resources;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace DeckForge.GameConstruction.PresetGames.War
{
    public class War
    {
        private IGameMediator gm;
        private IGameController gc;
        private BaseSetUpRules spr;
        private Table table;
        private List<IPlayer> players;
        private IRoundRules wrr;

        public War()
        {
            const short PLAYER_COUNT = 2;

            gm = new WarGameMediator(PLAYER_COUNT);
            gc = new BaseGameController(2);
            gm.RegisterGameController(gc);
            spr = new(initHandSize: 26);
            table = new(gm, PLAYER_COUNT, spr.Decks);
            players = WarPlayerSetUp(gm, table);

            List<int> playerIDs = new();
            foreach (IPlayer player in players)
            {
                playerIDs.Add(player.PlayerID);
            }
            wrr = new WarRoundRules(gm, playerIDs);
        }
        public void StartGame()
        {
            gm.StartGame();
        }

        private List<IPlayer> WarPlayerSetUp(IGameMediator gm, Table table)
        {
            List<IPlayer> players = new();
            for (var i = 0; i < 2; i++)
            {
                List<Card> cards = table.DrawMultipleCardsFromDeck(26)!;
                Deck deck = new(cards);
                IPlayer player = new WarPlayer(gm, i, deck);
                players.Add(player);
            }

            return players;
        }
    }
}
