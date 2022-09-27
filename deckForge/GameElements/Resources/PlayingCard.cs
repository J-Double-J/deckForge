namespace DeckForge.GameElements.Resources
{
    public class PlayingCard : Card
    {
        public PlayingCard(int val, string suit, bool facedown = true)
        : base(facedown: facedown)
        {
            Val = val;
            Suit = suit;
        }

        public int Val { get; }

        public string Suit { get; }

        public override string PrintCard()
        {
            if (!Facedown)
            {
                return $"{Val}{Suit}";
            }
            else
            {
                return "COVERED";
            }
        }
    }
}
