using deckForge.GameConstruction;
using deckForge.GameElements.Resources;

namespace deckForge.GameElements
{
    public class Table : ITable
    {
        IGameMediator _gm;
        public Table(IGameMediator mediator, int playerCount, List<Deck> initDecks)
        {
            _gm = mediator;
            _gm.RegisterTable(this);

            PlayedCards = new();
            for (var i = 0; i < playerCount; i++)
            {
                List<Card> cards = new();
                PlayedCards.Add(cards);
            }

            TableDecks = initDecks;
        }

        public List<Deck> TableDecks
        {
            get;
        }

        public List<List<Card>> PlayedCards
        {
            get;
        }

        public void PrintTableState()
        {
            foreach (List<Card> player in PlayedCards)
            {
                foreach (Card c in player)
                {
                    Console.WriteLine(c.PrintCard());
                }
            }
        }

        public List<Card> GetCardsForSpecificPlayer(int playerID)
        {
            return PlayedCards[playerID];
        }

        public void PlaceCardOnTable(int playerID, Card c)
        {
            try
            {
                PlayedCards[playerID].Add(c);
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
                foreach (Card c in PlayedCards[playerID])
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
            for (var i = 0; i < PlayedCards.Count; i++)
            {
                Flip_AllCardsOneWay_SpecificPlayer(i, facedown);
            }
        }

        public void Flip_AllCardsEitherWay_SpecificPlayer(int playerID)
        {
            try
            {
                foreach (Card c in PlayedCards[playerID])
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
            for (var i = 0; i < PlayedCards.Count; i++)
            {
                Flip_AllCardsEitherWay_SpecificPlayer(i);
            }
        }

        //Returns which card this command was on
        public Card Flip_SpecificCard_SpecificPlayer(int playerID, int cardPos)
        {
            try
            {
                PlayedCards[playerID][cardPos].Flip();
                return PlayedCards[playerID][cardPos];
            }
            catch { throw; }
        }

        //Returns which card this command was on
        public Card Flip_SpecificCard_SpecificPlayer_SpecificWay(int playerID, int cardPos, bool facedown = false)
        {
            if (PlayedCards[playerID][cardPos].Facedown != facedown)
            {
                PlayedCards[playerID][cardPos].Flip();
                return PlayedCards[playerID][cardPos];
            }
            else
            {
                return PlayedCards[playerID][cardPos];
            }
        }

        public Card RemoveSpecificCard_FromPlayer(int playerID, int cardPos)
        {
            try
            {
                Card c = PlayedCards[playerID][cardPos];
                PlayedCards[playerID].RemoveAt(cardPos);
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
                var numCardsToGrab = PlayedCards[playerID].Count;
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

        public Card? DrawCardFromDeck(int deckNum = 0) {
            try {
                return TableDecks[deckNum].DrawCard();
            } catch {
                throw;
            }
        }

        public List<Card?> DrawMultipleCardsFromDeck(int cardCount, int deckNum = 0) {
            List<Card?> cards = new();
            try {
                for (var i = 0; i < cardCount; i++) {
                    cards.Add(DrawCardFromDeck(deckNum));
                }
                return cards;
            } catch {
                throw;
            }
        }
    }
}
