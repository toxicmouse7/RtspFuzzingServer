namespace RtspServer.Infrastructure.RTP;

public class RTPHeader
{
    public const int Version = 2;
    public const short ExtensionsHeaderId = 0;

    public RTPHeader(int unixtime, short sequenceNumber)
    {
        Timestamp = unixtime;
        SequenceNumber = sequenceNumber;
    }

    public bool Padding { get; } = false;
    public bool Extension { get; } = true;
    public int CSRCCount { get; } = 0;
    public bool Marker { get; } = true;
    public int PayloadType { get; } = 26;
    public short SequenceNumber { get; }
    public int Timestamp { get; }
    public int SSRCIdentifier { get; } = 0x12121212;
    public int[] CSRC { get; } = null!;
    public short HeaderExtensionLength { get; } = 5000;

    public byte[] ToByteArray()
    {
        var firstByte = (byte)(Version << 6
                               | ((Padding ? 1 : 0) << 5)
                               | ((Extension ? 1 : 0) << 4)
                               | CSRCCount);
        
        var secondByte = (byte)((Marker ? 1 : 0) << 7
                                | (PayloadType));
        
        var header = new List<byte> {
            firstByte,
            secondByte,
        };

        header.AddRange(BitConverter.GetBytes(SequenceNumber).Reverse());
        header.AddRange(BitConverter.GetBytes(Timestamp).Reverse());
        header.AddRange(BitConverter.GetBytes(SSRCIdentifier).Reverse());

        if (Extension)
        {
            header.AddRange(BitConverter.GetBytes(ExtensionsHeaderId).Reverse());
            header.AddRange(BitConverter.GetBytes(HeaderExtensionLength).Reverse());
        }
        
        return header.ToArray();
    }
}