using System.Net;
using System.Security.Policy;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExtension
    {
        public static bool IsRequestFromLocalHost(this HttpRequest httpReq, Microsoft.AspNetCore.Mvc.IUrlHelper urlHelper)
        {
            var callingUrl = httpReq.Headers["Referer"].ToString();
            var isLocal = urlHelper.IsLocalUrl(callingUrl);
            return isLocal;
        }

        public static bool IsRequestFromLocalHost(this HttpRequest httpReq)
        {
            var connection = httpReq.HttpContext.Connection;
            if (connection.RemoteIpAddress.IsSet())
            {
                //We have a remote address set up
                return connection.LocalIpAddress.IsSet()
                    //Is local is same as remote, then we are local
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    //else we are remote if the remote IP address is not a loopback address
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            return true;
        }

        private const string LoopbackIPV6Address = "::1";
        private const string LoopbackIPV4Address = "127.0.0.1";

        private static bool IsSetAndNotLoopback(this IPAddress address)
        {
            return address != null && address.ToString() != LoopbackIPV6Address && address.ToString() != LoopbackIPV4Address;
        }

        private static bool IsSet(this IPAddress address)
        {
            return address != null;
        }
    }
}
