using System.Collections;
using System.Linq;
using System.Reflection;
using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.PhaseActions;
using DeckForge.PlayerConstruction.PlayerEvents;

namespace DeckForge.PlayerConstruction
{
    /// <summary>
    /// Base class for all <see cref="IPlayer"/>s that must be inherited from.
    /// </summary>
    public class BasePlayer : IPlayer
    {
        private readonly int initHandSize;
        private bool isOutVal = false;
        private bool isActiveVal = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePlayer"/> class.
        /// </summary>
        /// <param name="gm"><see cref="IGameMediator"/> that the <see cref="BasePlayer"/> will use to communicate
        /// with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="BasePlayer"/>.</param>
        /// <param name="initHandSize">The size of the initial hand that the <see cref="BasePlayer"/> will start with.</param>
        public BasePlayer(IGameMediator gm, int playerID = 0, int initHandSize = 5)
        {
            PlayerHand = new();

            // PlayerHand is not considered part of PlayerResourceCollections despite being a collection
            PlayerResourceCollections = new List<IResourceCollection>();

            GM = gm;
            PlayerID = playerID;

            gm.RegisterPlayer(this);

            this.initHandSize = initHandSize;
        }

        /// <inheritdoc/>
        public event EventHandler<PlayerPlayedCardEventArgs>? PlayerPlayedCard;

        /// <inheritdoc/>
        public event EventHandler<SimplePlayerMessageEventArgs>? PlayerMessageEvent;

        /// <inheritdoc/>
        public int HandSize
        {
            get
            {
                return PlayerHand.CurrentHandSize;
            }
        }

        /// <inheritdoc/>
        public int PlayerID { get; }

        /// <inheritdoc/>
        public List<PlayingCard> PlayedCards
        {
            get
            {
                return GM.GetPlayedCardsOfPlayer(PlayerID);
            }
        }

        /// <inheritdoc/>
        public bool IsActive
        {
            get
            {
                return isActiveVal;
            }

            set
            {
                if (isOutVal == false)
                {
                    isActiveVal = value;
                }
            }
        }

        /// <inheritdoc/>
        public bool IsOut
        {
            get
            {
                return isOutVal;
            }

            set
            {
                if (value is true)
                {
                    isOutVal = value;
                    isActiveVal = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of cards plays a turn a <see cref="BasePlayer"/> can do.
        /// </summary>
        protected int CardPlays { get; set; }

        /// <summary>
        /// Gets or sets the number of card draws they get at the start of their turn.
        /// </summary>
        protected int CardDraws { get; set; }

        /// <summary>
        /// Gets or sets the hand of the <see cref="BasePlayer"/>.
        /// </summary>
        protected Hand PlayerHand { get; set; }

        /// <summary>
        /// Gets or sets a list of the <see cref="IResourceCollection"/>s that the <see cref="BasePlayer"/> manages.
        /// </summary>
        protected List<IResourceCollection> PlayerResourceCollections { get; set; }

        /// <summary>
        /// Gets the <see cref="IGameMediator"/> that the <see cref="BasePlayer"/> will use to
        /// communicate to other game elements.
        /// </summary>
        protected IGameMediator GM { get; }

        /// <inheritdoc/>
        public virtual void DrawStartingHand()
        {
            for (var i = 0; i < initHandSize; i++)
            {
                DrawCard();
            }
        }

        /// <inheritdoc/>
        public virtual void StartTurn() // TODO: Remove outdated function.
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

        /// <inheritdoc/>
        public virtual void LoseGame()
        {
            RaiseSimplePlayerMessageEvent(new SimplePlayerMessageEventArgs("LOSE_GAME"));
        }

        /// <inheritdoc/>
        public virtual PlayingCard? DrawCard()
        {
            PlayingCard? c = GM.DrawCardFromDeck();
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

        /// <inheritdoc/>
        public virtual PlayingCard? PlayCard(bool facedown = false)
        {
            string? input;
            int selectedVal;

            Console.WriteLine("Which card would you like to play?");
            for (var i = 0; i < HandSize; i++)
            {
                Console.WriteLine($"{i}) {PlayerHand.GetCardAt(i).PrintCard()}");
            }

            do
            {
                input = Console.ReadLine();
            }
            while (int.TryParse(input, out selectedVal) && (selectedVal > HandSize || selectedVal < 0));

            PlayingCard c = PlayerHand.GetCardAt(selectedVal);
            PlayerHand.RemoveResource(c);

            if (facedown)
            {
                c.Flip();
            }

            // TODO: Possible conflict of ordering. Does another player/card do their events before or after a card is played?
            GM.PlayerPlayedCard(PlayerID, c);
            OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(c));

            return c;
        }

        /// <inheritdoc/>
        public PlayingCard FlipSingleCard(int cardNum, bool? facedown = null)
        {
            return GM.FlipSingleCard(PlayerID, cardNum, facedown);
        }

        /// <inheritdoc/>
        public List<PlayingCard> TakeAllCardsFromTable()
        {
            return GM.PickUpAllCards_FromTable_FromPlayer(PlayerID);
        }

        /// <inheritdoc/>
        public void AddResourceCollection(IResourceCollection collection)
        {
            PlayerResourceCollections.Add(collection);
        }

        /// <inheritdoc/>
        public virtual int FindCorrectResourceCollectionID(Type resourceType)
        {
            int resourcePoolID = -1;

            try
            {
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
            catch
            {
                throw;
            }

            return resourcePoolID;
        }

        /// <inheritdoc/>
        public int CountOfResourceCollection(int resourceCollectionID)
        {
            try
            {
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

        /// <inheritdoc/>
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

                foreach (MethodInfo method in methodInfos)
                {
                    if (method.Name == "GainResource")
                    {
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

        /// <inheritdoc/>
        public void AddResourceToCollection(int resourceCollectionID, object resource)
        {
            try
            {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                if (resource.GetType() != resourceCollection.ResourceType)
                {
                    throw new ArgumentException($"Resource of type {resource.GetType()} cannot be added to a Resource Collection of {resourceCollection.ResourceType}");
                }

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

        /// <inheritdoc/>
        public void RemoveResourceFromCollection(int resourceCollectionID, object resource)
        {
            try
            {
                ThrowIf_InvalidResourceID(resourceCollectionID);

                IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                var instance = Activator.CreateInstance(resourceCollection.GetType());

                ThrowIf_InvalidResourceForCollection(resourceCollection, resource);

                instance = resourceCollection;
                MethodInfo[] methodInfos = resourceCollection.GetType().GetMethods();
                foreach (MethodInfo method in methodInfos)
                {
                    if (method.Name == "RemoveResource")
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

        /// <inheritdoc/>
        public void AddMultipleResourcesToCollection(int resourceCollectionID, List<object> resources)
        {
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

        /// <inheritdoc/>
        public void IncrementResourceCollection(int resourceCollectionID)
        {
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
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public void DecrementResourceCollection(int resourceCollectionID)
        {
            try
            {
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

        /// <inheritdoc/>
        public void ClearResourceCollection(int resourceCollectionID)
        {
            try
            {
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

        /// <inheritdoc/>
        public object? ExecuteGameAction(IGameAction<IPlayer> action)
        {
            return action.Execute(this);
        }

        /// <inheritdoc/>
        public object? ExecuteGameActionAgainstPlayer(IGameAction<IPlayer> action, IPlayer target)
        {
            return action.Execute(this, target);
        }

        /// <inheritdoc/>
        public object? ExecuteGameActionAgainstMultiplePlayers(IGameAction<IPlayer> action, List<IPlayer> targets, bool includeSelf = false)
        {
            List<IPlayer> targetList = targets;

            // If action is targetted against everyone but self, remove self from list
            if (includeSelf == false)
            {
                targetList.RemoveAll(p => p.PlayerID == PlayerID);
            }

            return action.Execute(this, targetList);
        }

        /// <summary>
        /// Invokes <see cref="PlayerPlayedCard"/> whenever a <see cref="BasePlayer"/> plays a <see cref="PlayingCard"/>.
        /// </summary>
        /// <param name="e">The arguments of the <see cref="PlayerPlayedCardEventArgs"/>.</param>
        protected void OnPlayerPlayedCard(PlayerPlayedCardEventArgs e)
        {
            PlayerPlayedCard?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes <see cref="PlayerMessageEvent"/> with <see cref="SimplePlayerMessageEventArgs"/>.
        /// </summary>
        /// <param name="e">The arguments of the <see cref="SimplePlayerMessageEventArgs"/>.</param>
        protected void RaiseSimplePlayerMessageEvent(SimplePlayerMessageEventArgs e)
        {
            PlayerMessageEvent?.Invoke(this, e);
        }

        private static void ThrowIf_InvalidResourceForCollection(IResourceCollection resourceCollection, object resource)
        {
            if (resource.GetType() != resourceCollection.ResourceType)
            {
                throw new ArgumentException($"Resource of type {resource.GetType()} cannot be added to a Resource Collection of type {resourceCollection.ResourceType}");
            }
        }

        private void ThrowIf_InvalidResourceID(int resourceCollectionID)
        {
            if (resourceCollectionID < 0 || resourceCollectionID > PlayerResourceCollections.Count)
            {
                throw new ArgumentOutOfRangeException($"PlayerResouceCollections[{resourceCollectionID}] does not exist. Did you forget to add a collection to Player?");
            }
        }
    }
}
