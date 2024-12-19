namespace RtspServer.Domain.Models.Abstract;

public abstract record RtcpPacket(bool Padding, byte PayloadType, int SSRC)
{
    public const int Version = 2;
}