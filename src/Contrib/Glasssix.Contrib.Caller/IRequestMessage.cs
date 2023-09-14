using System.Net.Http;

namespace Glasssix.Contrib.Caller
{
    public interface IRequestMessage
    {
        void ProcessHttpRequestMessage(HttpRequestMessage requestMessage);

        void ProcessHttpRequestMessage<TRequest>(HttpRequestMessage requestMessage, TRequest data);
    }
}