// See https://aka.ms/new-console-template for more information

using deckForge.GameConstruction;

Console.WriteLine("Starting game!");

try
{
    BaseGameMediator gm = new(2);
    gm.StartGame();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StackTrace);
}
