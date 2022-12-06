using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.Dominion;
using DeckForge.GameConstruction.PresetGames.War;

Console.WriteLine("Starting game!");

try
{
    // War war = new War();
    // war.StartGame();

    Dominion dominion = new(2);
    dominion.StartGame();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StackTrace);
}
