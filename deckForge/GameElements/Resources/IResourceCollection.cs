using System.Collections;

namespace deckForge.GameElements.Resources
{

    public interface IResourceCollection {
        Type ResourceType { get; }
    }
    public interface IResourceCollection<T> : IResourceCollection
    {
        new Type ResourceType { get; }
        public void AddResource(T resource);
        public void AddMultipleResources(IList resource);
        public void RemoveResource(T resource);
        public void IncrementResourceCollection();
        public void DecrementResourceCollection();
        public T? GainResource();
        public List<T>? ClearCollection();
    }
}
