using MediatR;
using RtspServer.Application.Notifications;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Domain.Abstract.SessionServices;

public interface IRtpSessionService
{
    Task<RtpSession> CreateSessionAsync(RtspSession rtspSession);
    RtpSession GetSession(RtspSession rtspSession);
    void RemoveSession(RtspSession rtspSession);
}