namespace RtspServer.Infrastructure.RTP;

public class RTPJpegHeader
{
    public byte TypeSpecific { get; init; }
    public byte[] FragmentOffset { get; init; } = null!;
    public byte Type { get; init; }
    public byte Q { get; init; }
    public byte Width { get; init; }
    public byte Height { get; init; }

    public byte[] ToByteArray()
    {
        return [TypeSpecific, ..FragmentOffset, Type, Q, Width, Height];
    }
}