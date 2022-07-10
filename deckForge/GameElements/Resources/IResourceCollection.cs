namespace deckForge.GameElements.Resources
{
    public interface IResourceCollection<T>
    {
        public void AddResource(T resource);
        public void RemoveResource(T resource);
        public void IncrementResourceCollection();
        public void DecrementResourceCollection();
        public T? GainResource();
    }
}
