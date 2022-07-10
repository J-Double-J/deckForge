namespace deckForge.GameElements.Resources
{
    public interface IResourceCollection
    {
        public IResource collection { get; }
        public void AddResource(IResource resource);
        public void RemoveResource(IResource resource);
        public void IncrementResource();
        public void DecrementResource();
    }
}
