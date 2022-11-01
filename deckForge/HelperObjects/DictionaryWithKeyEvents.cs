using System.Collections;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;

namespace DeckForge.HelperObjects
{


    /// <summary>
    /// Interface for an object that has an <see cref="DictionaryValueChangedEventArgs{TKey, TValue}"/> <see cref="EventHandler"/>.
    /// </summary>
    /// <typeparam name="TKey">Key value for <see cref="DictionaryValueChangedEventArgs{TKey, TValue}"/>.</typeparam>
    /// <typeparam name="TValue">Value for a key in <see cref="DictionaryValueChangedEventArgs{TKey, TValue}"/>.</typeparam>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "File primarily used for Dictionary")]
    public interface IKeyValueNotifier<TKey, TValue>
    {
        /// <summary>
        /// Event associated with the given key in the dictionary.
        /// </summary>
        public event EventHandler<DictionaryValueChangedEventArgs<TKey, TValue>>? KeyEvent;
    }

    /// <summary>
    /// EventArgs for whenever <see cref="DictionaryWithKeyEvents{TKey, TValue}"/> has a value changed in the dictionary.
    /// </summary>
    /// <typeparam name="TKey">Key associated with  the <see cref="DictionaryWithKeyEvents{TKey, TVal}"/>.</typeparam>
    /// <typeparam name="TValue">Value associated with the  the <see cref="DictionaryWithKeyEvents{TKey, TVal}"/>.</typeparam>
    public class DictionaryValueChangedEventArgs<TKey, TValue> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryValueChangedEventArgs{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="key">Key associated with  the <see cref="DictionaryWithKeyEvents{TKey, TVal}"/>.</param>
        /// <param name="value">Value associated with the  the <see cref="DictionaryWithKeyEvents{TKey, TVal}"/>.</param>
        public DictionaryValueChangedEventArgs(TKey key, TValue? value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets the key of the <see cref="DictionaryWithKeyEvents{TKey, TVal}"/> that was changed.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// Gets the value of the new <see cref="DictionaryWithKeyEvents{TKey, TVal}"/> value.
        /// </summary>
        public TValue? Value { get; }
    }

    /// <summary>
    /// This dictionary raises <see cref="DictionaryValueChangedEventArgs{TKey, TValue}"/> whenever a key is updated to a new value.
    /// Listeners can listen to specific key values before the keys exist in the dictionary, and
    /// will be notified on their creation as well.
    /// </summary>
    /// <typeparam name="TKey">Non-nullable key for dictionary.</typeparam>
    /// <typeparam name="TValue">Value associated with dictionary.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Custom Dictionary only one that raises DictionaryValueChangedEventArgs")]
    public class DictionaryWithKeyEvents<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : notnull
        where TValue : IEquatable<TValue>
    {
        private readonly IDictionary<TKey, TValue> innerDict;
        private readonly IDictionary<TKey, KeyValueNotifier> eventDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryWithKeyEvents{TKey, TValue}"/> class.
        /// </summary>
        public DictionaryWithKeyEvents()
        {
            innerDict = new Dictionary<TKey, TValue>();
            eventDictionary = new Dictionary<TKey, KeyValueNotifier>();
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys
        {
            get { return innerDict.Keys; }
        }

        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get { return innerDict.Values; }
        }

        /// <inheritdoc/>
        public int Count
        {
            get { return innerDict.Count; }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
        {
            get
            {
                return innerDict.IsReadOnly;
            }
        }

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get
            {
                return innerDict[key];
            }

            set
            {
                if (innerDict.ContainsKey(key))
                {
                    if (!value.Equals(innerDict[key]))
                    {
                        innerDict[key] = value;
                        var dictEvent = new DictionaryValueChangedEventArgs<TKey, TValue>(key, value);
                        NotifyListenersForKey(key, value);
                    }
                }
                else
                {
                    innerDict[key] = value;
                    NotifyListenersForKey(key, value);
                }
            }
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            innerDict.Add(key, value);
            NotifyListenersForKey(key, value);
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            innerDict.Add(item);
            NotifyListenersForKey(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all the items from the <see cref="ICollection{T}"/>. Listeners interested
        /// in certain keys are notified of the change. <see cref="Dictionary{TKey, TValue}"/> raised will have a
        /// default <see cref="TValue"/>.
        /// </summary>
        public void Clear()
        {
            innerDict.Clear();

            foreach (var item in eventDictionary)
            {
                item.Value.OnKeyValueChanged(new DictionaryValueChangedEventArgs<TKey, TValue>(item.Key, default));
            }
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return innerDict.Contains(item);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            return innerDict.ContainsKey(key);
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            innerDict.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return innerDict.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            bool removed = innerDict.Remove(key);

            if (removed && eventDictionary.ContainsKey(key))
            {
                eventDictionary[key].OnKeyValueChanged(new DictionaryValueChangedEventArgs<TKey, TValue>(key, default));
            }

            return removed;
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool removed = innerDict.Remove(item);

            if (removed && eventDictionary.ContainsKey(item.Key))
            {
                eventDictionary[item.Key].OnKeyValueChanged(new DictionaryValueChangedEventArgs<TKey, TValue>(item.Key, default));
            }

            return removed;
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return innerDict.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the key's <see cref="EventHandler"/> if it exists.
        /// </summary>
        /// <param name="key">Key value to search for.</param>
        /// <returns>The key's associated <see cref="IKeyValueNotifier"/> if it exists, otherwise null.</returns>
        public IKeyValueNotifier<TKey, TValue>? GetKeyEventHandler(TKey key)
        {
            return eventDictionary.ContainsKey(key) ? eventDictionary[key] : null;
        }

        /// <summary>
        /// Removes the event linked with a key. If there are any listeners, the event will not be removed.
        /// </summary>
        /// <remarks>This function is mostly for performance benefits and removing events that are not
        /// needed anymore. A good rule of thumb is to call this method whenever an object that used
        /// to be interested in a key no longer needs to listen as it may have been the only object interested.</remarks>
        /// <param name="key">Key value to search for.</param>
        /// <returns><c>true</c> if the event with the key was removed successfully. Otherwise returns <c>false</c>.</returns>
        public bool RemoveKeyEventIfNoListeners(TKey key)
        {
            bool success = false;
            if (eventDictionary.ContainsKey(key) && eventDictionary[key].Listeners == 0)
            {
                eventDictionary.Remove(key);
                success = true;
            }

            return success;
        }

        /// <summary>
        /// Gets the key's <see cref="EventHandler"/> if it exists. If it does not exist it creates an EventHandler for the key,
        /// however, a key-value pair will not exist in the dictionary.
        /// </summary>
        /// <param name="key">Key value to search for.</param>
        /// <returns>The key's associated <see cref="KeyValueNotifier"/> which can be used to subscribe to it's
        /// <see cref="KeyValueNotifier.KeyEvent"/>.</returns>
        public IKeyValueNotifier<TKey, TValue> CreateOrGetKeyEventDictionaryEntry(TKey key)
        {
            if (!eventDictionary.ContainsKey(key))
            {
                eventDictionary.Add(key, new KeyValueNotifier());
            }

            return eventDictionary[key];
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerDict.GetEnumerator();
        }

        // It's possible for keys to have listeners before a key is actually created.
        // If a key is being listened to beforehand, this raises an event.
        private void NotifyListenersForKey(TKey key, TValue value)
        {
            if (eventDictionary.ContainsKey(key))
            {
                eventDictionary[key].OnKeyValueChanged(new DictionaryValueChangedEventArgs<TKey, TValue>(key, value));
            }
        }

        /// <summary>
        /// Helper object that raises an event when prompted to.
        /// </summary>
        private class KeyValueNotifier : IKeyValueNotifier<TKey, TValue>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="KeyValueNotifier"/> class.
            /// </summary>
            public KeyValueNotifier()
            {
            }

            /// <inheritdoc/>
            public event EventHandler<DictionaryValueChangedEventArgs<TKey, TValue>>? KeyEvent;

            public int Listeners
            {
                get
                {
                    return KeyEvent is not null ? KeyEvent.GetInvocationList().Length : 0;
                }
            }

            /// <summary>
            /// Allows the owning dictionary to invoke <see cref="KeyValueNotifier"/>'s event for the given key.
            /// </summary>
            /// <param name="e">The event args for <see cref="DictionaryValueChangedEventArgs{TKey, TValue}"/>.</param>
            public void OnKeyValueChanged(DictionaryValueChangedEventArgs<TKey, TValue> e)
            {
                KeyEvent?.Invoke(this, e);
            }
        }
    }
}
