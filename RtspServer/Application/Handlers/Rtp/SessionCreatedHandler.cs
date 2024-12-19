using MediatR;
using RtspServer.Application.Notifications;
using RtspServer.Domain.Abstract.SessionServices;

namespace RtspServer.Application.Handlers.Rtp;

public class SessionCreatedHandler : INotificationHandler<SessionCreatedNotification>
{
    private readonly IRtpSessionService _sessionService;

    public SessionCreatedHandler(IRtpSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task Handle(SessionCreatedNotification notification, CancellationToken cancellationToken)
    {
        var session = notification.Session;
        
        await _sessionService.CreateSessionAsync(session);
    }
}