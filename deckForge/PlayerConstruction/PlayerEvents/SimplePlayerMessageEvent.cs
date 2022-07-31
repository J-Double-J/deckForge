namespace DeckForge.PlayerConstruction.PlayerEvents
{
    public class SimplePlayerMessageEventArgs : EventArgs
    {
        public readonly string message;
        public SimplePlayerMessageEventArgs(string message)
        {
            this.message = message;
        }
    }
}