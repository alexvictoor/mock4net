using System.Net;
using System.Net.Sockets;

namespace Mock4Net.Core.Http
{
    public static class Ports
    {

        // see http://stackoverflow.com/questions/138043/find-the-next-tcp-port-in-net
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
