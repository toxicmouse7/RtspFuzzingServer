using System.Net;
using System.Net.Sockets;

namespace RtspServer.Infrastructure;

public class NetworkService : INetworkService
{
    public string GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ipAddress in host.AddressList)
        {
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                return ipAddress.ToString();
            }
        }
        
        throw new Exception("Unable to get local ip address.");
    }
}