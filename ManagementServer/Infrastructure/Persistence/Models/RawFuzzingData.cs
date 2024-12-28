namespace ManagementServer.Infrastructure.Persistence.Models;

public class RawFuzzingData
{
    public Guid Id { get; protected init; }
    public RtpFuzzingPreset Preset { get; protected init; } = null!;
    public byte[] RawData { get; protected init; } = null!;
}