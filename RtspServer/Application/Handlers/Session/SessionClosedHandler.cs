using MediatR;
using RtspServer.Application.Notifications;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Abstract.SessionServices;

namespace RtspServer.Application.Handlers.Session;

public class SessionClosedHandler : INotificationHandler<SessionClosedNotification>
{
    private readonly IRtspSessionsRepo _sessionsRepo;
    private readonly IRtpSessionService _rtpSessionService;

    public SessionClosedHandler(IRtspSessionsRepo sessionsRepo, IRtpSessionService rtpSessionService)
    {
        _sessionsRepo = sessionsRepo;
        _rtpSessionService = rtpSessionService;
    }

    public async Task Handle(SessionClosedNotification notification, CancellationToken cancellationToken)
    {
        await _sessionsRepo.RemoveSessionAsync(notification.Session.Id);
        _rtpSessionService.RemoveSession(notification.Session);
    }
}