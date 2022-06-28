namespace CardNamespace {
    public class Card {
        public int val;
        public string suit = String.Empty;
        private bool facedown;

        public Card(int val, string suit, bool facedown = true)
        {
            this.val = val;
            this.suit = suit;
            this.facedown = facedown;
        }

        public bool Facedown {
            get { return facedown; }
            set { facedown = value; }
        }
        public string PrintCard()
        {
            if (!facedown)
                return $"{val}{suit}";
            else
                return "COVERED";
        }

        public void Flip() {
            if (facedown)
                facedown = false;
            else
                facedown = true;
        }

    }
}