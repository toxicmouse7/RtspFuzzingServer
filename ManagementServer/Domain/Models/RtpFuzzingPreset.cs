using ManagementServer.Settings;
using RtspServer.Domain.Models.Abstract;
using RtspServer.Domain.Models.Rtp;

namespace ManagementServer.Domain.Models;

public class RtpFuzzingPreset
{
    public RtpFuzzingPreset(
        RtpHeader header, RtpContentHeader contentHeader, byte[] payload, AppendSettings appendSettings)
    {
        Id = Guid.NewGuid();
        Header = header;
        ContentHeader = contentHeader;
        Payload = payload;
        AppendSettings = appendSettings;
    }
    
    public Guid Id { get; protected init; }
    public RtpHeader Header { get; protected init; }
    public RtpContentHeader ContentHeader { get; protected init; }
    public byte[] Payload { get; protected init; }
    public AppendSettings AppendSettings { get; protected init; }
    public IReadOnlyCollection<RawFuzzingData> RawFuzzingData { get; protected init; } = new List<RawFuzzingData>();

    public void AddRawFuzzingData(IEnumerable<RawFuzzingData> data)
    {
        (RawFuzzingData as List<RawFuzzingData>)!.AddRange(data);
    }
}