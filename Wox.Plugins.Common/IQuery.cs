namespace Wox.Plugins.Common {
    public interface IQuery {
        string Search { get; }
        string[] Terms { get; }
    }
}