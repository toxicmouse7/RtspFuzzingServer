using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RtspServer.RtspController;

namespace RtspServer.Rtsp;

public class RtspServer : BackgroundService
{
    private readonly TcpListener _tcpListener = new(IPAddress.Any, 9000);
    private readonly ILogger<RtspServer> _logger;
    private readonly RtspConnectionContext.Factory _rtspConnectionContextFactory;

    public RtspServer(
        ILogger<RtspServer> logger,
        RtspConnectionContext.Factory rtspConnectionContextFactory)
    {
        _logger = logger;
        _rtspConnectionContextFactory = rtspConnectionContextFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _tcpListener.Start();
        
        _logger.LogInformation("RtspServer is running on {address}", _tcpListener.LocalEndpoint);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await _tcpListener.AcceptTcpClientAsync(stoppingToken);

            _logger.LogTrace("New client accepted");
            _ = Task.Factory.StartNew(async () =>
            {
                var rtspClientContext = _rtspConnectionContextFactory(client, stoppingToken);
                try
                {
                    await rtspClientContext.ServeAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error happened while serving client: {error}", e.Message);
                }
            }, stoppingToken);
        }
    }
}