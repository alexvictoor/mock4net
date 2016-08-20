using System.Net;
using System.Net.Sockets;

[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1633:FileMustHaveHeader",
        Justification = "Reviewed. Suppression is OK here, as unknown copyright and company.")]

namespace Mock4Net.Core.Http
{
    /// <summary>
    /// The ports.
    /// </summary>
    public static class Ports
    {
        /// <summary>
        /// The find free TCP port.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <remarks>see http://stackoverflow.com/questions/138043/find-the-next-tcp-port-in-net. </remarks>
        public static int FindFreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}
