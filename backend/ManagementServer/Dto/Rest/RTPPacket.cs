namespace ManagementServer.Dto.Rest;

public class RTPPacket
{
    public RTPHeader RTPHeader { get; set; } = null!;
    public RTPJpegHeader RTPJpegHeader { get; set; } = null!;
    public string? Base64Content { get; set; } 
}