namespace ManagementServer.Dto.Rest;

public class RTPJpegHeader
{
    public byte TypeSpecific { get; init; }
    public int FragmentOffset { get; init; }
    public byte Type { get; init; }
    public byte Quantization { get; init; }
    public byte Width { get; init; }
    public byte Height { get; init; }
}