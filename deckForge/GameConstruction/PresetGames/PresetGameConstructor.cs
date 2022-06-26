using System.Collections;

namespace deckForge.GameConstruction.PresetGames
{
    public class PresetGameConstructor
    {
        private bool _validGame = false;
        public PresetGameConstructor(string gameName) {
            if (PresetGameValidator.isPresetGameMade(gameName.ToUpper()) > -1)
            {
                _validGame = true;
            }
            else {
                throw new ArgumentException("Premade Game name not found");
            }
        }

        public bool isValidGame() {
            return _validGame;
        }

        //This class only makes sure that there is a preset game matching the string passed
        private static class PresetGameValidator {
            static Hashtable presetGames = new Hashtable() {
                { "WAR", 0 }
            };

            //-1 if game DNE otherwise returns ID
            static public int isPresetGameMade(string s) {
                if (presetGames.ContainsKey(s))
                {
                    return (int)presetGames[s]!;
                }
                else
                    return -1;
            }
            
        }
    }
}
