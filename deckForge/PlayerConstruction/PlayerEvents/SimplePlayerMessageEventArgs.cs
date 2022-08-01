namespace DeckForge.PlayerConstruction.PlayerEvents
{
    public class SimplePlayerMessageEventArgs : EventArgs
    {
        public SimplePlayerMessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}