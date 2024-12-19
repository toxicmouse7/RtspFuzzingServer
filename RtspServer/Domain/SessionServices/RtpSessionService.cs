using MediatR;
using RtspServer.Application.Notifications;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Abstract.SessionServices;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Domain.SessionServices;

public class RtpSessionService : IRtpSessionService
{
    private readonly Dictionary<RtspSession, RtpSession> _sessions = new();
    private readonly ISender _sender;
    private readonly IRtpPacketSourceFactory _sourceFactory;

    public RtpSessionService(ISender sender, IRtpPacketSourceFactory sourceFactory)
    {
        _sender = sender;
        _sourceFactory = sourceFactory;
    }

    public async Task<RtpSession> CreateSessionAsync(RtspSession rtspSession)
    {
        var source = await _sourceFactory.CreatePacketSourceAsync(rtspSession);
        var session = new RtpSession(rtspSession, _sender, source);
        
        _sessions.Add(rtspSession, session);

        return session;
    }

    public RtpSession GetSession(RtspSession rtspSession)
    {
        return _sessions[rtspSession];
    }

    public void RemoveSession(RtspSession rtspSession)
    {
        _sessions.Remove(rtspSession);
    }
}