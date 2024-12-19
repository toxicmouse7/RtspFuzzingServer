namespace ManagementServer.Dto.Rest;

public class RTPHeader
{
    public bool HasExtensionHeader { get; init; }
    public bool HasPadding { get; init; }
    public ushort ExtensionHeaderLength { get; init; }
    public bool Marker { get; init; }
    public int PayloadType { get; init; }
    public int CSRCCount { get; init; }
    public int? Timestamp { get; init; }
    public ushort? SequenceNumber { get; init; }
}