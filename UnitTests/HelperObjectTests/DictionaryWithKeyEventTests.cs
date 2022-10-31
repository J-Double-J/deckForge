using DeckForge.GameConstruction;
using DeckForge.HelperObjects;
using FluentAssertions;

namespace UnitTests.HelperObjectTests
{
    [TestClass]
    public class DictionaryWithKeyEventTests
    {
        [TestMethod]
        public void ElementsCanBeAddedToDictionary_WithBrackets()
        {
            DictionaryWithKeyEvents<int, int> dict = new();
            dict[1] = 1;

            dict[1].Should().Be(1, "that is the assigned value for the key");
        }

        [TestMethod]
        public void ElementsCanBeAddedToDictionary_WithAddMethod()
        {
            DictionaryWithKeyEvents<int, int> dict = new();
            dict.Add(1, 1);

            dict[1].Should().Be(1, "that is the assigned value for the key");
        }

        [TestMethod]
        public void ElementsWithSameKeysCannotBeAddedToDictionary_WithAddMethod()
        {
            DictionaryWithKeyEvents<int, int> dict = new();
            dict.Add(1, 1);

            Action a = () => dict.Add(1, 2);

            a.Should().Throw<ArgumentException>("a dictionary cannot use the Add() method for a key that already exists");
        }

        [TestMethod]
        public void ElementsWithSameKeysCanBeAddedToDictionary_WithBrackets()
        {
            DictionaryWithKeyEvents<int, int> dict = new();
            dict.Add(1, 1);
            dict[1] = 2;

            dict[1].Should().Be(2, "a dictionary can reassign a key value with [] accessors");
        }

        [TestMethod]
        public void GetDictionaryKeysAndValuesIndividually()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            List<int> keys = new() { 1, 3, 5 };
            List<int> values = new() { 2, 4, 6 };

            dict.Keys.Should().BeEquivalentTo(keys, "these are the key values given to the dictionary");
            dict.Values.Should().BeEquivalentTo(values, "these are the values associated to the keys");
        }

        [TestMethod]
        public void GetDictionaryCount()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            dict.Count.Should().Be(3, "there are 3 elements in the dictionary");
        }

        [TestMethod]
        public void AddKeyValuePairToDictionary()
        {
            DictionaryWithKeyEvents<int, int> dict = new();
            dict.Add(new KeyValuePair<int, int>(1, 1));

            dict[1].Should().Be(1, "a dictionary can add pairs together with KeyValuePairs<>");
        }

        public void DictionaryClearRemovesAllItems()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            dict.Clear();

            dict.Count.Should().Be(0, "there are no more objects left in the dictionary");
        }

        [TestMethod]
        public void DictionaryRemovesKeySuccessfully()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            dict.Remove(1).Should().BeTrue();
        }

        [TestMethod]
        public void DictionaryRemovesKeyValuePairSuccessfully()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            dict.Remove(new KeyValuePair<int, int>(1, 2)).Should().BeTrue();
        }

        [TestMethod]
        public void DictionaryCannotRemoveNonexistantKey()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            dict.Remove(20).Should().BeFalse();
        }

        [TestMethod]
        public void DictionaryCannotRemoveNonexistantKeyValuePair()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            dict.Remove(new KeyValuePair<int, int>(1, 3)).Should().BeFalse();
        }

        [TestMethod]
        public void EventRaisedWhenKeyCreatedForFirstTime()
        {
            DictionaryWithKeyEvents<int, int> dict = new();

            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(1).Monitor();
            dict.Add(1, 1);

            monitoredKey.Should().Raise("KeyEvent", "the `1` key was assigned a new value");
        }

        [TestMethod]
        public void EventNotRaisedWhenKeyValueIsUnchanged()
        {
            DictionaryWithKeyEvents<int, int> dict = new();
            dict.Add(1, 1);
            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(1).Monitor();
            dict[1] = 1;

            monitoredKey.Should().NotRaise("KeyEvent", "there was no change in value for the `1` key in the dictionary");
        }

        [TestMethod]
        public void EventRaisedWhenKeyValueIsChanged()
        {
            DictionaryWithKeyEvents<int, int> dict = new();
            dict.Add(1, 1);
            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(1).Monitor();
            dict[1] = 2;

            monitoredKey.Should().Raise("KeyEvent", "there was a change in value for the `1` key in the dictionary");
        }

        [TestMethod]
        public void EventRaisedOnlyForCorrectKeyChanged()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(3).Monitor();
            dict[1] = 3;
            monitoredKey.Should().NotRaise("KeyEvent");

            dict[3] = 10;
            monitoredKey.Should().Raise("KeyEvent");
        }

        [TestMethod]
        public void EventRaisedContainsKeyValueData()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(1).Monitor();
            dict[1] = 3;
            monitoredKey.Should().Raise("KeyEvent")
                .WithArgs<DictionaryValueChangedEventArgs<int, int>>(args => args.Key == 1 && args.Value == 3);
        }

        [TestMethod]
        public void EventRaisedWhenDictionaryCleared()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(5).Monitor();
            dict.Clear();

            monitoredKey.Should().Raise("KeyEvent", "the key value was wiped");
        }

        [TestMethod]
        public void EventRaisedWhenDictionaryClearedHasDefaultValues_int()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(5).Monitor();
            dict.Clear();

            monitoredKey.Should().Raise("KeyEvent", "the key value was wiped")
                .WithArgs<DictionaryValueChangedEventArgs<int, int>>(args => args.Value == 0);
        }

        [TestMethod]
        public void EventRaisedWhenDictionaryClearedHasDefaultValues_object()
        {
            DictionaryWithKeyEvents<int, DummyEquatableClass> dict = new()
            {
                { 1, new DummyEquatableClass() },
                { 3, new DummyEquatableClass() },
                { 5, new DummyEquatableClass() },
            };

            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(5).Monitor();
            dict.Clear();

            monitoredKey.Should().Raise("KeyEvent", "the key value was wiped")
                .WithArgs<DictionaryValueChangedEventArgs<int, DummyEquatableClass>>(args => args.Value == null);
        }

        [TestMethod]
        public void EventRaisedWhenInterestedKeyIsRemoved()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(5).Monitor();
            dict.Remove(5);

            monitoredKey.Should().Raise("KeyEvent")
                .WithArgs<DictionaryValueChangedEventArgs<int, int>>(args => args.Key == 5 && args.Value == 0);
        }

        [TestMethod]
        public void EventRaisedWhenInterestedKeyValuePairIsRemoved()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(5).Monitor();
            dict.Remove(new KeyValuePair<int, int>(5, 6));

            monitoredKey.Should().Raise("KeyEvent")
                .WithArgs<DictionaryValueChangedEventArgs<int, int>>(args => args.Key == 5 && args.Value == 0);
        }

        [TestMethod]
        public void EventIsNotRaisedWhenNonExistantKey_ThatIsListenedTo_IsTriedToBeRemoved()
        {
            DictionaryWithKeyEvents<int, int> dict = new()
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 },
            };

            using var monitoredKey = dict.CreateOrGetKeyEventDictionaryEntry(10).Monitor();
            dict.Remove(10);

            monitoredKey.Should().NotRaise("KeyEvent", "the `10` key was not changed");
        }

        // Dumb object to represent an object that is IEquatable.
        private class DummyEquatableClass : IEquatable<DummyEquatableClass>
        {
            public DummyEquatableClass()
            {
            }

            public bool Equals(DummyEquatableClass? other)
            {
                return true;
            }
        }
    }
}
