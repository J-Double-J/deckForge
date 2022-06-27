using deckForge.GameRules;

namespace deckForge.GameConstruction.PresetGames.War
{
    public class War
    {
        War()
        {
            SetUpRules spr = new(initHandSize: 26);

            GameMediator gm = new(2);

        }
    }
}
