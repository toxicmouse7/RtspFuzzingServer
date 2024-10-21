namespace RtspServer.Domain.Models;

public class Session
{
    public Session(long rtpPort, string ip)
    {
        Id = Random.Shared.NextInt64();
        RtpPort = rtpPort;
        Ip = ip;
    }
    
    public long Id { get; }
    public long RtpPort { get; }
    public string Ip { get; }
}