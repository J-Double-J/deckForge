using deckForge.GameConstruction;
using deckForge.GameRules;

namespace deckForge.GameConstruction.PresetGames
{
    public class War
    {
        War() {
            SetUpRules spr = new(initHandSize: 26);

            GameMediator gm = new(2);

        }
    }
}
