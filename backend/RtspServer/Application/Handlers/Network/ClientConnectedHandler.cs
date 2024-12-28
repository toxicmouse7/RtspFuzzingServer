using MediatR;
using Microsoft.Extensions.Logging;
using RtspServer.Application.Notifications;
using RtspServer.Infrastructure.Models;

namespace RtspServer.Application.Handlers.Network;

public class ClientConnectedHandler : INotificationHandler<ClientConnectedNotification>
{
    private readonly ISender _sender;
    private readonly ILoggerFactory _loggerFactory;

    public ClientConnectedHandler(ISender sender, ILoggerFactory loggerFactory)
    {
        _sender = sender;
        _loggerFactory = loggerFactory;
    }

    public Task Handle(ClientConnectedNotification notification, CancellationToken cancellationToken)
    {
        var client = notification.Client;
        var logger = _loggerFactory.CreateLogger<ClientConnection>();

        var clientConnection = new ClientConnection(client, _sender, logger);

        _ = Task.Factory.StartNew(async () =>
        {
            try
            {
                await clientConnection.ProcessAsync();
            }
            catch
            {
                await clientConnection.CloseAsync();
            }
        }, TaskCreationOptions.LongRunning);
        
        return Task.CompletedTask;
    }
}