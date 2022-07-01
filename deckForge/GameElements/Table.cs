using deckForge.GameConstruction;
using CardNamespace;

namespace deckForge.GameElements
{
    public class Table
    {
        GameMediator _gm;
        List<List<Card>> playedCards;
        public Table(GameMediator mediator, int playerCount)
        {
            _gm = mediator;

            playedCards = new();
            for (var i = 0; i < playerCount; i++)
            {
                List<Card> cards = new();
                playedCards.Add(cards);
            }
        }

        public void PrintTableState()
        {
            foreach (List<Card> player in playedCards)
            {
                foreach (Card c in player)
                {
                    Console.WriteLine(c.PrintCard());
                }
            }
        }

        public List<Card> GetCardsForSpecificPlayer(int playerID)
        {
            return playedCards[playerID];
        }

        public void PlaceCardOnTable(int playerID, Card c)
        {
            try
            {
                playedCards[playerID].Add(c);
            }
            catch
            {
                throw;
            }
        }

        public void Flip_AllCardsOneWay_SpecificPlayer(int playerID, bool facedown = false)
        {
            try
            {
                foreach (Card c in playedCards[playerID])
                {
                    if (c.Facedown != facedown)
                    {
                        c.Flip();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void Flip_AllCardsOneWay_AllPLayers(bool facedown = false)
        {
            for (var i = 0; i < playedCards.Count; i++)
            {
                Flip_AllCardsOneWay_SpecificPlayer(i, facedown);
            }
        }

        public void Flip_AllCardsEitherWay_SpecificPlayer(int playerID)
        {
            try
            {
                foreach (Card c in playedCards[playerID])
                {
                    c.Flip();
                }
            }
            catch
            {
                throw;
            }

        }

        public void Flip_AllCardsEitherWay_AllPlayers()
        {
            for (var i = 0; i < playedCards.Count; i++)
            {
                Flip_AllCardsEitherWay_SpecificPlayer(i);
            }
        }

        //Returns which card this command was on
        public Card Flip_SpecificCard_SpecificPlayer(int playerID, int cardPos)
        {
            try { 
                playedCards[playerID][cardPos].Flip(); 
                return playedCards[playerID][cardPos];
            }
            catch { throw; }
        }

        //Returns which card this command was on
        public Card Flip_SpecificCard_SpecificPlayer_SpecificWay(int playerID, int cardPos, bool facedown = false)
        {
            if (playedCards[playerID][cardPos].Facedown != facedown)
            {
                playedCards[playerID][cardPos].Flip();
                return playedCards[playerID][cardPos];
            } else {
                return playedCards[playerID][cardPos];
            }
        }

        public Card RemoveSpecificCard_FromPlayer(int playerID, int cardPos)
        {
            try
            {
                Card c = playedCards[playerID][cardPos];
                playedCards[playerID].RemoveAt(cardPos);
                return c;
            }
            catch
            {
                throw;
            }
        }

        public List<Card> PickUpAllCards_FromPlayer(int playerID)
        {
            try
            {
                List<Card> cards = new();

                // for (var i = 0; i < playedCards[playerID].Count; i++)
                // {
                //     cards.Add(RemoveSpecificCard_FromPlayer(playerID: playerID, cardPos: i));
                // }
                var numCardsToGrab = playedCards[playerID].Count;
                for (var i = 0; i < numCardsToGrab; i++)
                {
                    cards.Add(RemoveSpecificCard_FromPlayer(playerID: playerID, cardPos: 0));

                }

                return cards;
            }
            catch
            {
                throw;
            }
        }
    }
}
