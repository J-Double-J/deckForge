using deckForge.GameConstruction;
using deckForge.PlayerConstruction.PlayerEvents;
using deckForge.PhaseActions;
using deckForge.GameElements.Resources;
using System.Reflection;
using System.Collections;

namespace deckForge.PlayerConstruction
{
    public class BasePlayer : IPlayer
    {
        protected readonly IGameMediator GM;
        protected int CardPlays;
        protected int CardDraws;
        protected int InitHandSize;
        protected Hand PlayerHand; 
        protected List<IResourceCollection> PlayerResourceCollections;

        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;
        public event EventHandler<SimplePlayerMessageEventArgs>? PlayerMessageEvent;

        public BasePlayer(IGameMediator gm, int playerID = 0, int initHandSize = 5)
        {
            PlayerHand = new();

            //PlayerHand is not considered part of PlayerResourceCollections despite being a collection
            PlayerResourceCollections = new List<IResourceCollection>();

            GM = gm;
            PlayerID = playerID;

            //TODO: remove this is for sake of testing.
            CardPlays = 1;
            CardDraws = 1;

            InitHandSize = initHandSize;
        }

        public int HandSize
        {
            get { return PlayerHand.CurrentHandSize; }
        }

        public int PlayerID
        {
            get;
        }

        public List<Card> PlayedCards
        {
            get
            {
                return GM.GetPlayedCardsOfPlayer(PlayerID);
            }
        }

        virtual public void DrawStartingHand() {
            for (var i = 0; i < InitHandSize; i++)
            {
                DrawCard();
            }
        }

        virtual public void StartTurn()
        {
            for (var i = 0; i < CardDraws; i++)
            {
                DrawCard();
            }
            for (var j = 0; j < CardPlays; j++)
            {
                PlayCard();
            }
            GM.EndPlayerTurn();
        }

        //Returns which card was drawn
        virtual public Card? DrawCard()
        {
            Card? c = GM.DrawCardFromDeck();
            if (c != null)
            {
                PlayerHand.AddResource(c);
            }
            else
            {
                Console.WriteLine("Deck is Empty.");
            }

            return c;
        }

        //Returns PlayedCard
        virtual public Card? PlayCard(bool facedown = false)
        {
            //TODO: Remove
            if (HandSize == 0)
            {
                GM.EndGame();
                return null;
            }
            else
            {
                Console.WriteLine("Which card would you like to play?");
                for (var i = 0; i < HandSize; i++)
                {
                    Console.WriteLine($"{i}) {PlayerHand.GetCardAt(i).PrintCard()}");
                }
                string? input;
                int selectedVal;
                do
                {
                    input = Console.ReadLine();
                } while (int.TryParse(input, out selectedVal) && (selectedVal > HandSize || selectedVal < 0));

                Card c = PlayerHand.GetCardAt(selectedVal);
                PlayerHand.RemoveResource(c);

                if (facedown)
                    c.Flip();

                //TODO: Possible conflict of ordering. Does another player/card do their events before or after a card is played?
                GM.PlayerPlayedCard(PlayerID, c);
                OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(c));

                return c;
            }
        }

        protected void OnPlayerPlayedCard(PlayerPlayedCardEventArgs e)
        {
            var handler = PlayerPlayedCard;

            if (handler != null)
                handler(this, e);
        }

        protected void RaiseSimplePlayerMessageEvent(SimplePlayerMessageEventArgs e)
        {
            var handler = PlayerMessageEvent;
            if (handler != null)
                handler(this, e);
        }

        //Gets Flipped Card
        public Card FlipSingleCard(int cardNum, bool? facedown = null)
        {
            return GM.FlipSingleCard(PlayerID, cardNum, facedown);
        }

        public List<Card> TakeAllCardsFromTable()
        {
            return GM.PickUpAllCards_FromTable_FromPlayer(PlayerID);
        }

        public void AddResourceCollection(IResourceCollection collection)
        {
            PlayerResourceCollections.Add(collection);
        }

        //Finds first resource match or returns -1 if not found
        public virtual int FindCorrectPoolID(Type resourceType)
        {
            int resourcePoolID = -1;

            try {
                Type? collectionType = null;

                for (var i = 0; i < PlayerResourceCollections.Count; i++)
                {

                    ThrowIf_InvalidResourceID(i);

                    IResourceCollection resourceCollection = PlayerResourceCollections[i];
                    var instance = Activator.CreateInstance(resourceCollection.GetType());

                    PropertyInfo[] propertyInfos = resourceCollection.GetType().GetProperties();

                    foreach (PropertyInfo property in propertyInfos)
                    {
                        if (property.Name == "ResourceType")
                        {
                            collectionType = (Type)property.GetValue(instance, null)!;
                        }
                    }

                    if (collectionType != null && collectionType == resourceType)
                    {
                        resourcePoolID = i;
                        break;
                    }
                }
            } 
            catch { 
                throw; 
            }
            
            return resourcePoolID;
        }

        public int CountOfResourceCollection(int resourceCollectionID) {
            try {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                PropertyInfo[] propertyInfos = resourceCollection.GetType().GetProperties();

                instance = resourceCollection;
                foreach (PropertyInfo property in propertyInfos)
                {
                    if (property.Name == "Count")
                    {
                        return (int)property.GetValue(instance, null)!;
                    }
                }
                return -1;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
            catch
            {
                throw;
            }
        }

        public object? TakeResourceFromCollection(int resourceCollectionID)
        {
            try
            {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                object? gainedResource = null;
                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                instance = resourceCollection;
                MethodInfo[] methodInfos = resourceCollection.GetType().GetMethods();

                foreach (MethodInfo method in methodInfos) {
                    if (method.Name == "GainResource") {
                        gainedResource = method.Invoke(instance, null);
                        break;
                    }
                }

                return gainedResource;
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
            catch
            {
                throw;
            }
        }

        public void AddResourceToCollection(int resourceCollectionID, object resource) {
            try
            {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                if (resource.GetType() != resourceCollection.ResourceType)
                    throw new ArgumentException($"Resource of type {resource.GetType()} cannot be added to a Resource Collection of {resourceCollection.ResourceType}");

                instance = resourceCollection;
                MethodInfo[] methodInfos = resourceCollection.GetType().GetMethods();

                foreach (MethodInfo method in methodInfos)
                {
                    if (method.Name == "AddResource")
                    {
                        object[] args = { resource };
                        method.Invoke(instance, args);
                    }
                }
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
            catch
            {
                throw;
            }
        }

        public void RemoveResourceFromCollection(int resourceCollectionID, object resource) {
            try
            {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                ThrowIf_InvalidResourceForCollection(resourceCollection, resource);

                instance = resourceCollection;
                MethodInfo[] methodInfos = resourceCollection.GetType().GetMethods();
                foreach (MethodInfo method in methodInfos) {
                    if (method.Name == "RemoveResource") {
                        object[] args = { resource };
                        method.Invoke(instance, args);
                    }
                }
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
            catch
            {
                throw;
            }
        }

        //Individual resources must have a 
        public void AddMultipleResourcesToCollection(int resourceCollectionID, List<object> resources) {
            try
            {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                ThrowIf_InvalidResourceForCollection(resourceCollection, resources[0]);

                instance = resourceCollection;
                MethodInfo[] methodInfos = resourceCollection.GetType().GetMethods();

                Type listType = typeof(List<>).MakeGenericType(resources[0].GetType());
                IList listOfResources = (Activator.CreateInstance(listType) as IList)!;

                listOfResources = resources;

                foreach (MethodInfo method in methodInfos)
                {
                    if (method.Name == "AddMultipleResources")
                    {
                        object[] args = { listOfResources };
                        method.Invoke(instance, args);
                    }
                }
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
            catch
            {
                throw;
            }
        }

        public void IncrementResourceCollection(int resourceCollectionID) {
            try
            {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                instance = resourceCollection;
                MethodInfo[] methodInfos = resourceCollection.GetType().GetMethods();

                foreach (MethodInfo method in methodInfos)
                {
                    if (method.Name == "IncrementResourceCollection")
                    {
                        method.Invoke(instance, null);
                    }
                }
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
            catch {
                throw;
            }
        }

        public void DecrementResourceCollection(int resourceCollectionID)
        {
            try {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                instance = resourceCollection;
                MethodInfo[] methodInfos = resourceCollection.GetType().GetMethods();

                foreach (MethodInfo method in methodInfos)
                {
                    if (method.Name == "DecrementResourceCollection")
                    {
                        method.Invoke(instance, null);
                    }
                }
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
            catch
            {
                throw;
            }

        }

        public void ClearResourceCollection(int resourceCollectionID) {
            try {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                instance = resourceCollection;
                MethodInfo[] methodInfos = resourceCollection.GetType().GetMethods();

                foreach (MethodInfo method in methodInfos)
                {
                    if (method.Name == "ClearCollection")
                    {
                        method.Invoke(instance, null);
                    }
                }
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
            catch
            {
                throw;
            }
        }

        private void ThrowIf_InvalidResourceID(int resourceCollectionID) {
            if (resourceCollectionID < 0 || resourceCollectionID > PlayerResourceCollections.Count)
                throw new ArgumentOutOfRangeException($"PlayerResouceCollections[{resourceCollectionID}] does not exist. Did you forget to add a collection to Player?");
        }

        private void ThrowIf_InvalidResourceForCollection(IResourceCollection resourceCollection, object resource)
        {
            if (resource.GetType() != resourceCollection.ResourceType)
                throw new ArgumentException($"Resource of type {resource.GetType()} cannot be added to a Resource Collection of type {resourceCollection.ResourceType}");
        }

        public object? ExecuteGameAction(PlayerGameAction action) {
            return action.execute(this);  
        }
        public object? ExecuteGameActionAgainstPlayer(PlayerGameAction action, IPlayer target) {
            return action.execute(this, target);
        }
        public object? ExecuteGameActionAgainstMultiplePlayers(PlayerGameAction action, List<IPlayer> targets, bool includeSelf = false) {

            List<IPlayer> targetList = targets;

            //If action can be targetted against everyone but self, remove self from list
            if (includeSelf == false) {
                targetList = new();
                for (int i = 0; i < targets.Count; i++) {
                    if (i != PlayerID) {
                        targetList.Add(targets[i]);
                    }
                }
            }
            
            return action.execute(this, targets);
        }
    }
}
