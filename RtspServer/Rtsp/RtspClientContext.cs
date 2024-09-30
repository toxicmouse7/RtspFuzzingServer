using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace RtspServer.Rtsp;

public sealed class RtspClientContext
{
    private readonly TcpClient _client;
    private readonly CancellationToken _stoppingToken;
    private readonly ILogger<RtspClientContext> _logger;
    private readonly RtspRequestParser _requestParser;
    private readonly RtspRequestHandler _requestHandler;

    public RtspClientContext(
        TcpClient client,
        CancellationToken stoppingToken,
        ILogger<RtspClientContext> logger,
        RtspRequestParser requestParser,
        RtspRequestHandler requestHandler)
    {
        _client = client;
        _logger = logger;
        _requestParser = requestParser;
        _requestHandler = requestHandler;
        _stoppingToken = stoppingToken;
    }
    
    public delegate RtspClientContext Factory(TcpClient client, CancellationToken stoppingToken);
    
    private async Task HandleRequestAsync(RtspRequest request)
    {
        var stream = _client.GetStream();
        var response = await _requestHandler.HandleAsync(request);

        await stream.WriteAsync(Encoding.UTF8.GetBytes(response), _stoppingToken);
        _logger.LogDebug("Sent response:\n{response}", response.Trim());
    }

    public async Task ServeAsync()
    {
        var stream = _client.GetStream();
        
        while (!_stoppingToken.IsCancellationRequested)
        {
            var request = await _requestParser.ParseStreamAsync(stream, _stoppingToken);
            _logger.LogDebug("Received request:\n{request}", request.ToString().Trim());
            
            await HandleRequestAsync(request);
        }
    }
}