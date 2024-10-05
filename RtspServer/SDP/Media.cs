namespace RtspServer.SDP;

public class Media
{
    public const string StreamType = "video";
    public const string Transport = "RTP/AVP";
    public const int Codec = 26; // MJPEG
    public int Port { get; }

    public Media(int port)
    {
        Port = port;
    }

    public override string ToString()
    {
        return $"{StreamType} {Port} {Transport} {Codec}";
    }
}