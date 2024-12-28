using ManagementServer.Domain.Models;

namespace ManagementServer.Domain.Abstract;

public interface IRtpFuzzingPayloadGenerator
{
    Task<IReadOnlyCollection<RawFuzzingData>> GenerateRtpPayloadsAsync(RtpFuzzingPreset preset, TimeSpan generateFor);
}