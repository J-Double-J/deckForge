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
        public virtual void DrawStartingHand()
        {
            DrawMultipleCards(initHandSize);
        }

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
            return DrawCard(TablePlacementZoneType.PlayerZone, PlayerID);
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

        // TODO: Refactor to be more useable as virtual. Change to use prompter.

        /// <inheritdoc/>
        public virtual ICard? PlayCard(bool facedown = false)
        {
            ICard? card = PromptPlayerToPickCardToPlay();

            if (card is null)
            {
                return null;
            }

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

        /// <summary>
        /// Prompts <see cref="IPlayer"/> to pick a <see cref="ICard"/> to play.
        /// </summary>
        /// <remarks>When overriding, consider using one of the GetCardInHand() methods.</remarks>
        /// <returns>A selected <see cref="ICard"/> or null if no valid cards, or empty hand.</returns>
        protected virtual ICard? PromptPlayerToPickCardToPlay()
        {
            return GetCardInHand("Which card would you like to play?", true);
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
        /// Prompt that is created whenever <see cref="IPlayer"/> has to pick a <see cref="ICard"/> from their <see cref="Hand"/>.
        /// </summary>
        /// <param name="header">String that will be in the header of the <see cref="IPrompter"/>. If <c>null</c> no header will be printed.</param>
        /// <returns>A dictionary for use in a <see cref="IPrompter"/>.</returns>
        protected virtual Dictionary<int, string> CreatePromptFromCardsInHand(string? header = "Cards in Hand:")
        {
            Dictionary<int, string> prompt = new();
            if (header is not null)
            {
                prompt.Add(0, header);
            }

            for (var i = 0; i < HandSize; i++)
            {
                prompt.Add(i + 1, $"{PlayerHand.GetCardAt(i).PrintCard()}");
            }

            return prompt;
        }

        /// <summary>
        /// Prompt that is created whenever <see cref="IPlayer"/> has to pick a <see cref="ICard"/> from their <see cref="Hand"/>. Can allow
        /// an option to cancel.
        /// </summary>
        /// <param name="allowCancel">If <c>true</c> allows the <see cref="IPlayer"/> to cancel or exit the prompt.</param>
        /// <param name="cancelText">String to present for cancel option.</param>
        /// <param name="header">String that will be in the header of the <see cref="IPrompter"/>. If <c>null</c> no header will be printed.</param>
        /// <returns>A dictionary for use in a <see cref="IPrompter"/>.</returns>
        protected virtual Dictionary<int, string> CreatePromptFromCardsInHand(bool allowCancel, string cancelText = "Cancel", string? header = "Cards in Hand:")
        {
            var prompt = CreatePromptFromCardsInHand(header);

            if (allowCancel)
            {
                prompt.Add(-1, cancelText);
            }

            return prompt;
        }

        // TODO: Refactor? Should GetCardInHand be a Hand or a BasePlayer method? Should these methods be public?

        /// <summary>
        /// Prompts the <see cref="IPlayer"/> to choose a <see cref="ICard"/>.
        /// </summary>
        /// <param name="promptHeader">Prompt header displayed to help instruct user choice.</param>
        /// <returns><see cref="ICard"/> they picked from their hand. If no <see cref="ICard"/>s are in
        /// the <see cref="IPlayer"/>'s hand, returns null.</returns>
        protected ICard? GetCardInHand(string promptHeader)
        {
            return GetCardInHand(promptHeader, false);
        }

        /// <summary>
        /// Prompts the <see cref="IPlayer"/> to choose a <see cref="ICard"/>.
        /// </summary>
        /// <param name="promptHeader">Prompt header displayed to help instruct user choice.</param>
        /// <param name="promptHasCancel">If <c>true</c> allows the <see cref="IPlayer"/> to not pick a <see cref="ICard"/>.</param>
        /// <param name="cancelPromptString">If prompt allows the choice to not pick a <see cref="ICard"/> this is the text for that choice.</param>
        /// <returns><see cref="ICard"/> they picked from their hand. If no <see cref="ICard"/>s are in
        /// the <see cref="IPlayer"/>'s hand or the <see cref="IPlayer"/> does not make a choice, returns null.</returns>
        protected ICard? GetCardInHand(string promptHeader, bool promptHasCancel, string cancelPromptString = "Cancel")
        {
            var handPrompt = CreatePromptFromCardsInHand(true, cancelPromptString, promptHeader);
            var handPrompter = new PlayerPrompter(InputReader, OutputDisplay, handPrompt, promptHasCancel);
            int choice = handPrompter.Prompt();

            if (choice != -1)
            {
                return PlayerHand.GetCardAt(choice - 1);
            }

            return null;
        }

        /// <summary>
        /// Prompts the <see cref="IPlayer"/> to choose a <see cref="ICard"/>.
        /// </summary>
        /// <param name="conditionForValidCards">Only allows <see cref="ICard"/>s that meet this condition to be chosen from.</param>
        /// <param name="promptHeader">Prompt header displayed to help instruct user choice.</param>
        /// <returns><see cref="ICard"/> they picked from their hand. If no <see cref="ICard"/>s are in
        /// the <see cref="IPlayer"/>'s hand or no <see cref="ICard"/>s meet
        /// the condition specified, returns null.</returns>
        protected ICard? GetCardInHand(Predicate<ICard> conditionForValidCards, string promptHeader)
        {
            return GetCardInHand(conditionForValidCards, promptHeader, false);
        }

        /// <summary>
        /// Prompts the <see cref="IPlayer"/> to choose a <see cref="ICard"/>.
        /// </summary>
        /// <param name="conditionForValidCards">Only allows <see cref="ICard"/>s that meet this condition to be chosen from.</param>
        /// <param name="promptHeader">Prompt header displayed to help instruct user choice.</param>
        /// <param name="promptHasCancel">If <c>true</c> allows the <see cref="IPlayer"/> to not pick a <see cref="ICard"/>.</param>
        /// <param name="cancelPromptString">If prompt allows the choice to not pick a <see cref="ICard"/> this is the text for that choice.</param>
        /// <returns><see cref="ICard"/> they picked from their hand. If no <see cref="ICard"/>s are in
        /// the <see cref="IPlayer"/>'s hand, no <see cref="ICard"/>s meet the conditions, or the <see cref="IPlayer"/> does not make a choice, returns null.</returns>
        protected ICard? GetCardInHand(Predicate<ICard> conditionForValidCards, string promptHeader, bool promptHasCancel, string cancelPromptString = "Cancel")
        {
            if (!CardFoundThatMeetsCondition(conditionForValidCards))
            {
                return null;
            }

            var handPrompt = CreatePromptFromCardsInHand(promptHasCancel, cancelPromptString, promptHeader);
            var prompter = new PlayerPrompter(InputReader, OutputDisplay, handPrompt, promptHasCancel);

            int choice = 0;
            while (choice != -1)
            {
                choice = prompter.Prompt();
                if (choice != -1)
                {
                    ICard card = PlayerHand.GetCardAt(choice - 1);
                    if (conditionForValidCards(card))
                    {
                        return card;
                    }
                    else
                    {
                        OutputDisplay.Display("Invalid Choice");
                    }
                }
            }

            return null;
        }

        private bool CardFoundThatMeetsCondition(Predicate<ICard> condition)
        {
            bool aCardMeetsCondition = false;
            foreach (ICard card in PlayerHand.Cards)
            {
                if (condition(card))
                {
                    aCardMeetsCondition = true;
                    break;
                }
            }

            return aCardMeetsCondition;
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
