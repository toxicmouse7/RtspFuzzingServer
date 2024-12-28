using System.Net.Sockets;
using MediatR;
using RtspServer.Application.Notifications;
using RtspServer.Domain.Abstract.SessionServices;

namespace RtspServer.Application.Handlers.Rtcp;

public class SessionCreatedHandler : INotificationHandler<SessionCreatedNotification>
{
    private readonly IRtcpSessionService _sessionService;

    public SessionCreatedHandler(IRtcpSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task Handle(SessionCreatedNotification notification, CancellationToken cancellationToken)
    {
        var session = notification.Session;
        
        _ = Task.Run(async () =>
        {
            using var udpClient = new UdpClient(12346);

            while (!session.Token.IsCancellationRequested)
            {
                await udpClient.ReceiveAsync(session.Token);
            }
        }, CancellationToken.None);

        await _sessionService.CreateSessionAsync(notification.Session);
    }
}