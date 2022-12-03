using System.Collections;
using System.Linq;
using System.Reflection;
using DeckForge.GameConstruction;
using DeckForge.GameElements.Resources;
using DeckForge.GameElements.Resources.Cards;
using DeckForge.GameElements.Table;
using DeckForge.HelperObjects;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePlayer"/> class. Specifies where input and output should be done.
        /// </summary>
        /// <param name="reader">Tells <see cref="IPlayer"/> where to read input.</param>
        /// <param name="output">Tells <see cref="IPlayer"/> where to display output.</param>
        /// <param name="gm"><see cref="IGameMediator"/> that the <see cref="BasePlayer"/> will use to communicate
        /// with other game elements.</param>
        /// <param name="playerID">ID of the <see cref="BasePlayer"/>.</param>
        /// <param name="initHandSize">The size of the initial hand that the <see cref="BasePlayer"/> will start with.</param>
        public BasePlayer(IInputReader reader, IOutputDisplay output, IGameMediator gm, int playerID = 0, int initHandSize = 5)
            : this(gm, playerID, initHandSize)
        {
            InputReader = reader;
            OutputDisplay = output;
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
        public IReadOnlyList<ICard> PlayedCards
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

        /// <summary>
        /// Gets the Input Reader that reads user input.
        /// </summary>
        protected IInputReader InputReader { get; } = new ConsoleReader();

        /// <summary>
        /// Gets the Output Display that displays strings.
        /// </summary>
        protected IOutputDisplay OutputDisplay { get; } = new ConsoleOutput();

        /// <inheritdoc/>
        public virtual void DrawStartingHand(TablePlacementZoneType zoneType, int area = 0)
        {
            for (var i = 0; i < initHandSize; i++)
            {
                DrawCard(zoneType, area);
            }
        }

        /// <inheritdoc/>
        public virtual void StartTurn()
        {
        }

        /// <inheritdoc/>
        public virtual void EndTurn()
        {
        }

        /// <inheritdoc/>
        public virtual void LoseGame()
        {
            RaiseSimplePlayerMessageEvent(new SimplePlayerMessageEventArgs("LOSE_GAME"));
        }

        /// <inheritdoc/>
        public virtual ICard? DrawCard()
        {
            ICard? card = GM.Table!.DrawCardFromDeckInArea(TablePlacementZoneType.PlayerZone, PlayerID);
            if (card != null)
            {
                card.OwnedBy = this;
                PlayerHand.AddResource(card);
            }
            else
            {
                OutputDisplay.Display("Deck is Empty.");
            }

            return card;
        }

        /// <inheritdoc/>
        public virtual ICard? DrawCard(TablePlacementZoneType zoneType, int area = 0)
        {
            ICard? card = GM.DrawCardFromDeck(zoneType, area);
            if (card != null)
            {
                card.OwnedBy = this;
                PlayerHand.AddResource(card);
            }
            else
            {
                OutputDisplay.Display("Deck is Empty.");
            }

            return card;
        }

        /// <inheritdoc/>
        public virtual List<ICard?> DrawMultipleCards(int numCards)
        {
            List<ICard?> cards = new();

            for (int i = 0; i < numCards; i++)
            {
                cards.Add(DrawCard());
            }

            return cards;
        }

        /// <inheritdoc/>
        public virtual List<ICard?> DrawMultipleCards(int numCards, TablePlacementZoneType zoneType, int area = 0)
        {
            List<ICard?> cards = new();

            for (int i = 0; i < numCards; i++)
            {
                cards.Add(DrawCard(zoneType, area));
            }

            return cards;
        }

        /// <inheritdoc/>
        public virtual void AddCardToHand(ICard card)
        {
            card.OwnedBy = this;
            PlayerHand.AddResource(card);
        }

        /// <inheritdoc/>
        public virtual void AddCardsToHand(List<ICard> cards)
        {
            foreach (ICard card in cards)
            {
                AddCardToHand(card);
            }
        }

        /// <inheritdoc/>
        public virtual ICard? PlayCard(bool facedown = false)
        {
            string? input;
            int selectedVal;

            OutputDisplay.Display("Which card would you like to play?");
            for (var i = 0; i < HandSize; i++)
            {
                OutputDisplay.Display($"{i}) {PlayerHand.GetCardAt(i).PrintCard()}");
            }

            do
            {
                input = InputReader.GetInput();
            }
            while (int.TryParse(input, out selectedVal) && (selectedVal > HandSize || selectedVal < 0));

            ICard card = PlayerHand.GetCardAt(selectedVal);
            PlayerHand.RemoveResource(card);

            if (facedown)
            {
                card.Flip();
            }

            // TODO: Possible conflict of ordering. Does another player/card do their events before or after a card is played?
            GM.PlayerPlayedCard(PlayerID, card);
            OnPlayerPlayedCard(new PlayerPlayedCardEventArgs(card));

            return card;
        }

        /// <inheritdoc/>
        public void FlipSingleCard(int cardNum, bool? facedown = null)
        {
            GM.FlipSingleCard(TablePlacementZoneType.PlayerZone, PlayerID, cardNum, facedown);
        }

        /// <inheritdoc/>
        public List<ICard> TakeAllCardsFromTable()
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

                    PropertyInfo property = resourceCollection.GetType().GetProperty("ResourceType")!;
                    collectionType = (Type)property.GetValue(instance, null)!;

                    if (collectionType != null && (collectionType == resourceType || collectionType.IsAssignableFrom(resourceType)))
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
                instance = resourceCollection;

                PropertyInfo property = resourceCollection.GetType().GetProperty("Count")!;
                return (int)property.GetValue(instance, null)!;
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

                MethodInfo method = resourceCollection.GetType().GetMethod("GainResource")!;
                gainedResource = method.Invoke(instance, null);

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

                if (resource.GetType() != resourceCollection.ResourceType && !resourceCollection.ResourceType.IsAssignableFrom(resource.GetType()))
                {
                    throw new ArgumentException($"Resource of type {resource.GetType()} cannot be added to a Resource Collection of {resourceCollection.ResourceType}");
                }

                instance = resourceCollection;

                MethodInfo method = resourceCollection.GetType().GetMethod("AddResource")!;
                object[] args = { resource };
                method.Invoke(instance, args);
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

                MethodInfo method = resourceCollection.GetType().GetMethod("RemoveResource")!;
                object[] args = { resource };
                method.Invoke(instance, args);
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
            if (resources.Count != 0)
            {
                try
                {
                    ThrowIf_InvalidResourceID(resourceCollectionID);

                    IResourceCollection resourceCollection = PlayerResourceCollections[resourceCollectionID];
                    var instance = Activator.CreateInstance(resourceCollection.GetType());

                    ThrowIf_InvalidResourceForCollection(resourceCollection, resources[0]);

                    instance = resourceCollection;

                    Type listType = typeof(List<>).MakeGenericType(resources[0].GetType());
                    IList listOfResources = (Activator.CreateInstance(listType) as IList)!;

                    listOfResources = resources;

                    MethodInfo method = resourceCollection.GetType().GetMethod("AddMultipleResources")!;
                    object[] args = { listOfResources };
                    method.Invoke(instance, args);
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

                MethodInfo method = resourceCollection.GetType().GetMethod("IncrementResourceCollection")!;
                method.Invoke(instance, null);
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

                MethodInfo method = resourceCollection.GetType().GetMethod("DecrementResourceCollection")!;
                method.Invoke(instance, null);
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

                MethodInfo method = resourceCollection.GetType().GetMethod("ClearCollection")!;
                method.Invoke(instance, null);
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
        /// Invokes <see cref="PlayerPlayedCard"/> whenever a <see cref="BasePlayer"/> plays a <see cref="ICard"/>.
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
            if (resource.GetType() != resourceCollection.ResourceType && !resourceCollection.ResourceType.IsAssignableFrom(resource.GetType()))
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
