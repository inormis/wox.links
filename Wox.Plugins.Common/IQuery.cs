namespace Wox.Plugins.Common {
    public interface IQuery {
        string Search { get; }
        string[] Arguments { get; }
        
        string RawArgument { get; }
    }
}