namespace deckForge.PlayerConstruction.PlayerEvents
{
    public class SimplePlayerMessageEvent : EventArgs
    {
        public readonly string message;
        public SimplePlayerMessageEvent(string message)
        {
            this.message = message;
        }
    }
}