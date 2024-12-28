using ManagementServer.Domain.Models;
using RtspServer.Domain.Models;
using RtspServer.Domain.Models.Sessions;

namespace ManagementServer.Domain.Abstract;

public interface IFuzzingService
{
    Task<RtpFuzzingPreset> AddRtpPresetAsync(RtpFuzzingPreset preset);
    Task<IEnumerable<RtpFuzzingPreset>> GetAllPresetsAsync();
    Task RemovePresetAsync(Guid presetId);
    Task StartFuzzingAsync(long sessionId);
    void StopFuzzing(RtspSession session);
}