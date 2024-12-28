namespace ManagementServer.Dto.Rest;

public class SenderReport
{
    public bool Padding { get; init; }
    public byte ReceptionReportCount { get; init; }
    public ushort Length { get; init; } 
    public ulong NtpTimestamp { get; init; }
    public uint RtpTimestamp { get; init; }
    public uint SenderPacketsCount { get; init; }
    public uint SenderOctetsCount { get; init; }
}