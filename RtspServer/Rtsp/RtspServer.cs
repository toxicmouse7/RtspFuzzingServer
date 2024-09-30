using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RtspServer.Rtsp;

public class RtspServer : BackgroundService
{
    private readonly TcpListener _tcpListener = new(IPAddress.Parse("127.0.0.1"), 9000);
    private readonly RtspRequestParser _requestParser;
    private readonly RtspRequestHandler _requestHandler;
    private readonly ILogger<RtspServer> _logger;
    private readonly RtspClientContext.Factory _rtspClientContextFactory;

    public RtspServer(ILogger<RtspServer> logger,
        RtspRequestParser requestParser,
        RtspRequestHandler requestHandler,
        RtspClientContext.Factory rtspClientContextFactory)
    {
        _logger = logger;
        _requestParser = requestParser;
        _requestHandler = requestHandler;
        _rtspClientContextFactory = rtspClientContextFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _tcpListener.Start();
        
        _logger.LogInformation("RtspServer is running on {address}", _tcpListener.LocalEndpoint);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await _tcpListener.AcceptTcpClientAsync(stoppingToken);

            _ = Task.Factory.StartNew(async () =>
            {
                var rtspClientContext = _rtspClientContextFactory(client, stoppingToken);
                await rtspClientContext.ServeAsync();
            }, stoppingToken);
        }
    }
}