namespace RtspServer.SDP;

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
}