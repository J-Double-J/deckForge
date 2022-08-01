using System.Collections;

namespace DeckForge.GameConstruction.PresetGames
{
    public class PresetGameConstructor
    {
        private bool validGame = false;

        public PresetGameConstructor(string gameName)
        {
            if (PresetGameValidator.IsPresetGameMade(gameName.ToUpper()) > -1)
            {
                validGame = true;
            }
            else
            {
                throw new ArgumentException("Premade Game name not found");
            }
        }

        public bool IsValidGame()
        {
            return validGame;
        }

        // This class only makes sure that there is a preset game matching the string passed
        private static class PresetGameValidator
        {
            private static Hashtable presetGames = new Hashtable()
            {
                { "WAR", 0 }
            };

            // -1 if game DNE otherwise returns ID
            public static int IsPresetGameMade(string s)
            {
                if (presetGames.ContainsKey(s))
                {
                    return (int)presetGames[s]!;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
