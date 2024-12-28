using MediatR;
using RtspServer.Application.Notifications;
using RtspServer.Domain.Abstract.SessionServices;

namespace RtspServer.Application.Handlers.Rtp;

public class SessionClosedHandler : INotificationHandler<SessionClosedNotification>
{
    private readonly IRtpSessionService _sessionService;

    public SessionClosedHandler(IRtpSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public Task Handle(SessionClosedNotification notification, CancellationToken cancellationToken)
    {
        _sessionService.RemoveSession(notification.Session);
        return Task.CompletedTask;
    }
}