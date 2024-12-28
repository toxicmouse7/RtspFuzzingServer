using ManagementServer.Settings;
using Microsoft.Extensions.Options;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Models.Sessions;

namespace ManagementServer.Infrastructure;

public class RtpPacketSourceManager : IRtpPacketSourceFactory
{
    private readonly IOptions<DataSourceSettings> _settings;
    private readonly Dictionary<RtspSession, RtpPacketSource> _sources = new();

    public RtpPacketSourceManager(IOptions<DataSourceSettings> settings)
    {
        _settings = settings;
    }

    public Task<IRtpPacketSource> CreatePacketSourceAsync(RtspSession session)
    {
        var source = new RtpPacketSource(session, _settings);
        _sources.Add(session, source);
        return Task.FromResult<IRtpPacketSource>(source);
    }

    public Task<IRtpPacketSource> GetPacketSourceAsync(RtspSession session)
    {
        var source = _sources[session];

        return Task.FromResult<IRtpPacketSource>(source);
    }
}