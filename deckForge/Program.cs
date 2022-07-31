// See https://aka.ms/new-console-template for more information

using DeckForge.GameConstruction;
using DeckForge.GameConstruction.PresetGames.War;

Console.WriteLine("Starting game!");

try
{
    War war = new War();
    war.StartGame();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StackTrace);
}
