namespace ManagementServer.Dto.Rest.Out;

public class RTPPacket
{
    public Guid Id { get; set; }
    public RTPHeader RTPHeader { get; set; } = null!;
    public RTPJpegHeader RTPJpegHeader { get; set; } = null!;
    public string? Base64Content { get; set; } 
}