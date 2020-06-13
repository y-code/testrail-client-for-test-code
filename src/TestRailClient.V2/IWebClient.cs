using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Ycode.TestRailClient.V2
{
    public interface IWebClient : IDisposable
    {
        WebHeaderCollection Headers { get; }
        Task<Stream> OpenReadTaskAsync(string address);
        Task<string> UploadStringTaskAsync(string address, string data);
    }

    public interface IWebClientFactory
    {
        IWebClient Create(WebHeaderCollection headers);
    }

    public class WebClientFactory : IWebClientFactory
    {
        public IWebClient Create(WebHeaderCollection headers)
            => new WebClientAdapter(headers);
    }

    public class WebClientAdapter : WebClient, IWebClient
    {
        public WebClientAdapter(WebHeaderCollection headers) : base()
        {
            Headers = headers;
        }
    }
}
