namespace DeckForge.GameElements.Resources
{
    public class PlayingCard
    {
        private bool facedown;

        public PlayingCard(int val, string suit, bool facedown = true)
        {
            Val = val;
            Suit = suit;
            this.facedown = facedown;
        }

        public int Val { get; }

        public string Suit { get; }

        public bool Facedown
        {
            get { return facedown; }
            set { facedown = value; }
        }

        public string PrintCard()
        {
            if (!facedown)
            {
                return $"{Val}{Suit}";
            }
            else
            {
                return "COVERED";
            }
        }

        public void Flip()
        {
            if (facedown)
            {
                facedown = false;
            }
            else
            {
                facedown = true;
            }
        }
    }
}
