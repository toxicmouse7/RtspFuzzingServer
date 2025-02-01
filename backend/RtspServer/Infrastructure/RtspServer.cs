using System.Net;
using System.Net.Sockets;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RtspServer.Application.Notifications;

namespace RtspServer.Infrastructure;

public class RtspServer : IHostedService
{
    private readonly TcpListener _server;
    private readonly IPublisher _publisher;
    private readonly ILogger<RtspServer> _logger;

    public RtspServer(IPEndPoint endPoint, IPublisher publisher, ILogger<RtspServer> logger)
    {
        _publisher = publisher;
        _logger = logger;
        _server = new TcpListener(endPoint);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.Factory.StartNew(
            async () => await StartListeningAsync(cancellationToken),
            TaskCreationOptions.LongRunning);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
    private async Task StartListeningAsync(CancellationToken token)
    {
        _server.Start();
        
        _logger.LogInformation("Rtsp server listening on: rtsp://{endpoint}", _server.LocalEndpoint);

        while (!token.IsCancellationRequested)
        {
            var client = await _server.AcceptTcpClientAsync(token);
            
            _logger.LogInformation("Client connected. Client EP: {ep}", client.Client.RemoteEndPoint);

            await _publisher.Publish(new ClientConnectedNotification(client), token);
        }
    }
}