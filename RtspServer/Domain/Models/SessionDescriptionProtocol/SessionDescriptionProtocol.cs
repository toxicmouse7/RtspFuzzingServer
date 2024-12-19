namespace RtspServer.Domain.Models.SessionDescriptionProtocol;

public class SessionDescriptionProtocol
{
    public SessionDescriptionProtocol(Origin origin, Media media)
    {
        Origin = origin;
        Media = media;
    }
    
    public const int ProtocolVersion = 0;
    public const string SessionName = "My Session";
    public const string Time = "0 0";
    public Origin Origin { get; }
    public Media Media { get; }

    public override string ToString()
    {
        return $"v={ProtocolVersion}\r\n" +
               $"o={Origin}\r\n" +
               $"s={SessionName}\r\n" +
               $"t={Time}\r\n" +
               $"a=recvonly\r\n" +
               $"m={Media}\r\n" +
               $"a=rtpmap:26 JPEG/90000\r\n";
    }
}