using deckForge.GameRules;
using deckForge.GameElements;
using deckForge.PlayerConstruction;
using deckForge.GameConstruction;
using deckForge.GameRules.RoundConstruction.Rounds;
using deckForge.GameElements.Resources;
using deckForge.GameRules.RoundConstruction.Interfaces;


namespace deckForge.GameConstruction.PresetGames.War
{
    public class War
    {
        private IGameMediator gm;
        private BaseSetUpRules spr;
        private Table table;
        private List<IPlayer> players;
        private IRoundRules wrr;

        War()
        {
            const short PLAYER_COUNT = 2;

            gm = new BaseGameMediator(PLAYER_COUNT);
            spr = new(initHandSize: 26);
            table = new(gm, PLAYER_COUNT, spr.Decks);
            players = WarPlayerSetUp(gm, table);
            wrr = new WarRoundRules(players);
        }

        public void StartGame() {
            gm.StartGame();
        }

        private List<IPlayer> WarPlayerSetUp(IGameMediator gm, Table table)
        {
            List<IPlayer> players = new();
            for (var i = 0; i < 2; i++)
            {
                List<Card> cards = table.DrawMultipleCardsFromDeck(26)!;
                Deck deck = new(cards);
                players.Add(new WarPlayer(gm, i, deck));
            }

            return players;
        }
    }
}
