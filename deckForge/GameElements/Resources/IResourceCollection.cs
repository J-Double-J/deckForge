using System.Collections;

namespace DeckForge.GameElements.Resources
{

    public interface IResourceCollection {
        Type ResourceType { get; }
    }
    public interface IResourceCollection<T> : IResourceCollection
    {
        new public Type ResourceType { get; }
        public int Count { get; } 
        public void AddResource(T resource);
        public void AddMultipleResources(IList resource);
        public void RemoveResource(T resource);
        public void IncrementResourceCollection();
        public void DecrementResourceCollection();
        public T? GainResource();
        public List<T>? ClearCollection();
    }
}
