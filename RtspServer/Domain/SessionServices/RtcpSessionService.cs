using MediatR;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Abstract.SessionServices;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Domain.SessionServices;

public class RtcpSessionService : IRtcpSessionService
{
    private readonly Dictionary<RtspSession, RtcpSession> _sessions = new();
    private readonly ISender _sender;
    private readonly IRtcpPacketSourceFactory _sourceFactory;

    public RtcpSessionService(ISender sender, IRtcpPacketSourceFactory sourceFactory)
    {
        _sender = sender;
        _sourceFactory = sourceFactory;
    }

    public async Task<RtcpSession> CreateSessionAsync(RtspSession rtspSession)
    {
        var source = await _sourceFactory.CreatePacketSourceAsync(rtspSession);
        var session = new RtcpSession(rtspSession, source, _sender);
        
        _sessions.Add(rtspSession, session);

        return session;
    }

    public RtcpSession GetSession(RtspSession rtspSession)
    {
        return _sessions[rtspSession];
    }

    public void RemoveSession(RtspSession rtspSession)
    {
        _sessions.Remove(rtspSession);
    }
}