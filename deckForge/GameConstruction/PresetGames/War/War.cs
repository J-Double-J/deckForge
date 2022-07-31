﻿using DeckForge.GameElements;
using DeckForge.GameElements.Resources;
using DeckForge.GameRules;
using DeckForge.GameRules.RoundConstruction.Interfaces;
using DeckForge.PlayerConstruction;

namespace DeckForge.GameConstruction.PresetGames.War
{
    /// <summary>
    /// Play the classic game of War.
    /// </summary>
    public class War
    {
        private IGameMediator gm;
        private IGameController gc;
        private BaseSetUpRules spr;
        private Table table;
        private List<IPlayer> players;
        private IRoundRules wrr;

        /// <summary>
        /// Initializes a new instance of the <see cref="War"/> class.
        /// </summary>
        public War()
        {
            const short PLAYER_COUNT = 2;

            gm = new WarGameMediator(PLAYER_COUNT);
            gc = new BaseGameController(2);
            gm.RegisterGameController(gc);
            spr = new (initHandSize: 26);
            table = new (gm, PLAYER_COUNT, spr.Decks);
            players = WarPlayerSetUp(gm, table);

            List<int> playerIDs = new ();
            foreach (IPlayer player in players)
            {
                playerIDs.Add(player.PlayerID);
            }

            wrr = new WarRoundRules(gm, playerIDs);
        }

        /// <summary>
        /// Starts the game of War.
        /// </summary>
        public void StartGame()
        {
            gm.StartGame();
        }

        /// <summary>
        /// Creates a list of <see cref="WarPlayer"/>s and sets up their requirements for <see cref="War"/>.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that all the <see cref="IPlayer"/>s will use to communicate
        /// with other game elements.</param>
        /// <param name="table"><see cref="Table"/> that the <see cref="IPlayer"/>s will draw their 
        /// <see cref="Card"/>s from.</param>
        /// <returns>List of prepared <see cref="IPlayer"/>s for <see cref="War"/>.</returns>
        private static List<IPlayer> WarPlayerSetUp(IGameMediator gm, Table table)
        {
            List<IPlayer> players = new ();
            for (var i = 0; i < 2; i++)
            {
                List<Card> cards = table.DrawMultipleCardsFromDeck(26)!;
                Deck deck = new (cards);
                IPlayer player = new WarPlayer(gm, i, deck);
                players.Add(player);
            }

            return players;
        }
    }
}
