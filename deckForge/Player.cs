using GameNamespace;
using CardNamespace;

public class Player
{
    GameMediator gm;
    int cardPlays;
    int cardDraws;
    int startingHand;
    List<Card> hand = new List<Card>();

    public Player(GameMediator gm)
    {
        this.gm = gm;

        //TODO: remove this is for sake of testing.
        startingHand = 5;
        cardPlays = 1;
        cardDraws = 1;

        for (var i = 0; i < startingHand; i++)
        {
            DrawCard();
        }
    }

    public void StartTurn()
    {
        for (var i = 0; i < cardDraws; i++)
        {
            DrawCard();
        }
        for (var j = 0; j < cardPlays; j++)
        {
            PlayCard();
        }
        gm.EndPlayerTurn();
    }

    public void DrawCard()
    {
        Card? c = gm.DrawCardFromDeck();
        if (c != null) {
            hand.Add(c);
        } else {
            Console.WriteLine("Deck is Empty.");
        }
    }

    public void PlayCard()
    {
        if (hand.Count == 0)
        {
            gm.EndGame();
        }
        else
        {
            Console.WriteLine("Which card would you like to play?");
            for (var i = 0; i < hand.Count; i++)
            {
                Console.WriteLine($"{i}) {hand[i].PrintCard()}");
            }
            string? input;
            int selectedVal;
            do
            {
                input = Console.ReadLine();
            } while (int.TryParse(input, out selectedVal) && (selectedVal > hand.Count || selectedVal < 0));

            Card c = hand[selectedVal];
            hand.RemoveAt(selectedVal);

            gm.PlayerPlayedCard(c);
        }
    }
}
