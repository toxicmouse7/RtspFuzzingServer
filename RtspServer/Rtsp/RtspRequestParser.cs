using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;

namespace RtspServer.Rtsp;

public sealed class RtspRequestParser
{
    private readonly ILogger<RtspRequestParser> _logger;

    public RtspRequestParser(ILogger<RtspRequestParser> logger)
    {
        _logger = logger;
    }

    public async Task<RtspRequest> ParseStreamAsync(NetworkStream stream, CancellationToken cancellationToken)
    {
        var buffer = new byte[8196];
        
        _ = await stream.ReadAsync(buffer, cancellationToken);
        
        var rawRequest = Encoding.UTF8.GetString(buffer);
        var lines = rawRequest.Split("\r\n").ToArray();
        if (lines.Length == 0)
        {
            _logger.LogError("Empty RTSP request");
            throw new FormatException("Invalid RTSP request");
        }
        
        var requestLineParts = lines[0].Split(' ');
        if (requestLineParts.Length != 3)
        {
            _logger.LogError("Invalid RTSP request");
            throw new FormatException("Invalid RTSP request");
        }
        
        var method = requestLineParts[0];
        
        var uri = requestLineParts[1];
        var version = requestLineParts[2];

        var headers = new Dictionary<string, string>();
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }
            
            var colonIndex = line.IndexOf(':');
            if (colonIndex <= 0) continue;
            
            var headerName = line[..colonIndex].Trim();
            var headerValue = line[(colonIndex + 1)..].Trim();
            headers[headerName] = headerValue;
        }
        
        return new RtspRequest(method, uri, version, headers);
    }
}