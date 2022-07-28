// See https://aka.ms/new-console-template for more information

using deckForge.GameConstruction;
using deckForge.GameConstruction.PresetGames.War;

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
