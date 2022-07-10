using deckForge.GameRules;
using deckForge.GameElements;
using CardNamespace;
using DeckNameSpace;
using deckForge.PlayerConstruction;
using deckForge.GameConstruction;
using deckForge.GameRules.RoundConstruction.Rounds;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class War
    {
        War()
        {
            const short PLAYER_COUNT = 2;

            BaseGameMediator gm = new(PLAYER_COUNT);
            BaseSetUpRules spr = new(initHandSize: 26);
            BaseGameController gameController = new(PLAYER_COUNT);
            Table table = new(gm, PLAYER_COUNT, spr.Decks);
            List<WarPlayer> players = WarPlayerSetUp(gm, table);
            //WarRoundRules wrr = new WarRoundRules(players);

        }

        private List<WarPlayer> WarPlayerSetUp(BaseGameMediator gm, Table table)
        {
            List<WarPlayer> players = new();
            for (var i = 0; i < 2; i++)
            {
                List<Card> cards = table.DrawCardsFromDeck(26)!;
                Deck deck = new(cards);
                players.Add(new WarPlayer(gm, i, deck));
            }

            return players;
        }
    }
}
