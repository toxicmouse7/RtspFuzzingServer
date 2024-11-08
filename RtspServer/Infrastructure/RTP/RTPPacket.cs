namespace RtspServer.Infrastructure.RTP;

public class RTPPacket
{
    public RTPPacket(RTPHeader header, byte[] content)
    {
        Header = header;
        Content = content;
    }
    
    public RTPHeader Header { get; }
    public byte[] Content { get; }

    public byte[] ToByteArray()
    {
        return Header.ToByteArray().Concat(Content).ToArray();
    }
}