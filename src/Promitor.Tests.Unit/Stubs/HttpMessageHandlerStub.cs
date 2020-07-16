using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Promitor.Tests.Unit.Stubs
{
    public class HttpMessageHandlerStub : HttpMessageHandler
    {
        public HttpRequestMessage LastRequest { get; set; }

        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            LastRequest = request;
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}
