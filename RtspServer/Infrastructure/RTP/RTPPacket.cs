using System.Net;

namespace RtspServer.Infrastructure.RTP;

public class RTPPacket
{
    public const int Version = 2;

    public RTPPacket(int unixtime, short sequenceNumber, byte[] content)
    {
        SequenceNumber = sequenceNumber;
        Content = content;
    }

    public bool Padding { get; } = false;
    public bool Extension { get; } = false;
    public int CSRCCount { get; } = 0;
    public bool Marker { get; } = true;
    public int PayloadType { get; } = 26;
    public short SequenceNumber { get; }
    public int Timestamp { get; }
    public int SSRC { get; } = 0x12121212;
    public int[] CSRC { get; } = null!;
    public int HeaderExtension { get; } = 1;
    public byte[] Content { get; }

    public byte[] ToByteArray()
    {
        var firstByte = (byte)(Version << 6
                               | ((Padding ? 1 : 0) << 5)
                               | ((Extension ? 1 : 0) << 4)
                               | CSRCCount);
        
        var secondByte = (byte)((Marker ? 1 : 0) << 7
                                | (PayloadType));
        
        return [
            firstByte,
            secondByte,
            ..BitConverter.GetBytes(SequenceNumber).Reverse(),
            ..BitConverter.GetBytes(Timestamp).Reverse(),
            ..BitConverter.GetBytes(SSRC).Reverse(),
            // ext header length
            // 12, 12,
            // 0xff, 0xff,
            // jpeg header
            0,
            0, 0, 0,
            1,
            255,
            138,
            179,
            //precision
            0, 0,
            // qtable_len
            1,
            ..Content];
    }
}