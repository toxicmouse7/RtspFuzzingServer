using MediatR;
using RtspServer.Application.Notifications;
using RtspServer.Infrastructure.Models;

namespace RtspServer.Application.Handlers.Network;

public class ClientConnectedHandler : INotificationHandler<ClientConnectedNotification>
{
    private readonly ISender _sender;

    public ClientConnectedHandler(ISender sender)
    {
        _sender = sender;
    }

    public Task Handle(ClientConnectedNotification notification, CancellationToken cancellationToken)
    {
        var client = notification.Client;

        var clientConnection = new ClientConnection(client, _sender);

        _ = Task.Factory.StartNew(clientConnection.ProcessAsync, TaskCreationOptions.LongRunning);
        
        return Task.CompletedTask;
    }
}