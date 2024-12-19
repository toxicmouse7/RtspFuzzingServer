namespace RtspServer.Domain.Models.Rtp;

public record RtpHeader(
    bool Padding,
    bool Extension,
    uint CSRCCount,
    bool Marker,
    uint PayloadType,
    ushort SequenceNumber,
    int Timestamp,
    uint SSRCIdentifier,
    uint[] CSRC,
    ushort HeaderExtensionLength)
{
    public const int Version = 2;
    public const short ExtensionsHeaderId = 0;
}