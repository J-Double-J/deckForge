namespace CardNamespace {
    public class Card {
        public int val;
        public string suit = String.Empty;

        public Card(int val, string suit)
        {
            this.val = val;
            this.suit = suit;
        }

        public string PrintCard()
        {
            return $"{val}{suit}";
        }
    }
}