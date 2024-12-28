using RtspServer.Domain.Abstract;
using RtspServer.Domain.Models.Sessions;

namespace ManagementServer.Infrastructure;

public class RtcpPacketSourceManager : IRtcpPacketSourceFactory
{
    private readonly Dictionary<RtspSession, RtcpPacketSource> _sources = new();
    public Task<IRtcpPacketSource> CreatePacketSourceAsync(RtspSession session)
    {
        var source = new RtcpPacketSource(session);
        _sources.Add(session, source);
        return Task.FromResult<IRtcpPacketSource>(source);
    }

    public Task<IRtcpPacketSource> GetPacketSourceAsync(RtspSession session)
    {
        var source = _sources[session];

        return Task.FromResult<IRtcpPacketSource>(source);
    }
}