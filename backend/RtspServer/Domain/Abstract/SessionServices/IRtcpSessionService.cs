using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Domain.Abstract.SessionServices;

public interface IRtcpSessionService
{
    Task<RtcpSession> CreateSessionAsync(RtspSession rtspSession);
    RtcpSession GetSession(RtspSession rtspSession);
    void RemoveSession(RtspSession rtspSession);
}