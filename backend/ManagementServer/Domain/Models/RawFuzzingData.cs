namespace ManagementServer.Domain.Models;

public class RawFuzzingData
{
    public RawFuzzingData(RtpFuzzingPreset preset, byte[] rawData)
    {
        Preset = preset;
        RawData = rawData;
    }
    
    public RtpFuzzingPreset Preset { get; protected init; }
    public byte[] RawData { get; protected init; }
}