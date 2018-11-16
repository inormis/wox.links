namespace Wox.Links.Services
{
    public interface ILinkProcess
    {
        void Process(string url, params string[] args);
        void Open(string httpsSomeComDo);
    }
}