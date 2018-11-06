namespace Wox.Links
{
    public interface IStorage
    {
        void Set(string shortcut, string url);
        
        Link[] GetShortcuts();
        
        void Delete(string shortcut);
    }
}