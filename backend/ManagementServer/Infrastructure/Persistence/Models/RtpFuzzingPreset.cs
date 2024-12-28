using ManagementServer.Settings;
using RtspServer.Domain.Models.Rtp;

namespace ManagementServer.Infrastructure.Persistence.Models;

public enum HeaderType
{
    Empty,
    Jpeg
}

public class RtpFuzzingPreset
{
    public Guid Id { get; protected init; }
    public RtpHeader Header { get; protected init; } = null!;
    public string ContentHeader { get; protected init; } = null!;
    public HeaderType ContentHeaderType { get; protected init; }
    public byte[]? Payload { get; protected init; }
    public AppendSettings AppendSettings { get; protected init; } = null!;
    public List<RawFuzzingData> RawFuzzingData { get; protected init; } = null!;
}