using deckForge.GameRules;
using deckForge.GameElements;
using deckForge.PlayerConstruction;
using deckForge.GameConstruction;
using deckForge.GameRules.RoundConstruction.Rounds;
using deckForge.GameElements.Resources;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class War
    {
        War()
        {
            const short PLAYER_COUNT = 2;

            IGameMediator gm = new BaseGameMediator(PLAYER_COUNT);
            BaseSetUpRules spr = new(initHandSize: 26);
            IGameController gameController = new BaseGameController(PLAYER_COUNT);
            Table table = new(gm, PLAYER_COUNT, spr.Decks);
            List<IPlayer> players = WarPlayerSetUp(gm, table);
            WarRoundRules wrr = new WarRoundRules(players);
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
