using ManagementServer.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RtspServer.Application.Notifications;

namespace ManagementServer.Application.Handlers;

public class SessionClosedHandler : INotificationHandler<SessionClosedNotification>
{
    private readonly IHubContext<SessionsHub> _hub;

    public SessionClosedHandler(IHubContext<SessionsHub> hub)
    {
        _hub = hub;
    }

    public async Task Handle(SessionClosedNotification notification, CancellationToken cancellationToken)
    {
        var session = notification.Session;

        await _hub.Clients.All.SendAsync(
            "RemoveSession",
            session.Id.ToString(),
            cancellationToken: cancellationToken);
    }
}