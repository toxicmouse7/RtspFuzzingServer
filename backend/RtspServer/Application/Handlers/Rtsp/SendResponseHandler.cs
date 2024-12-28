using System.Net.Sockets;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using RtspServer.Application.Commands;
using RtspServer.Domain.Models.RtspResponses;

namespace RtspServer.Application.Handlers.Rtsp;

public class SendResponseHandler : IRequestHandler<SendResponseCommand>
{
    private readonly ILogger<SendResponseHandler> _logger;

    public SendResponseHandler(ILogger<SendResponseHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SendResponseCommand request, CancellationToken cancellationToken)
    {
        var (stream, response) = request;

        await (response switch
        {
            RtspOptionsResponse optionsResponse => SendOptionsResponse(stream, optionsResponse),
            RtspDescribeResponse describeResponse => SendDescribeResponse(stream, describeResponse),
            RtspSetupResponse setupResponse => SendSetupResponse(stream, setupResponse),
            RtspPlayResponse playResponse => SendPlayResponse(stream, playResponse),
            RtspTeardownResponse teardownResponse => SendTeardownResponse(stream, teardownResponse),
            RtspMethodNotValidResponse methodNotValidResponse => SendMethodNotValidResponse(stream, methodNotValidResponse),
            _ => SendNotImplementedResponse(stream)
        });
    }

    private async Task SendMethodNotValidResponse(NetworkStream stream, RtspMethodNotValidResponse methodNotValidResponse)
    {
        var response = FormatResponse(
            "RTSP/1.0 455 Method Not Valid in This State",
            $"CSeq: {methodNotValidResponse.Sequence}");
        
        _logger.LogTrace("Sent response: {response}", Encoding.UTF8.GetString(response));

        await stream.WriteAsync(response);
    }

    private async Task SendNotImplementedResponse(NetworkStream stream)
    {
        var response = "RTSP/1.0 501 Not Implemented\r\n\r\n"u8.ToArray();
        
        _logger.LogTrace("Sent response: {response}", Encoding.UTF8.GetString(response));

        await stream.WriteAsync(response);
    }

    private async Task SendOptionsResponse(NetworkStream stream, RtspOptionsResponse optionsResponse)
    {
        var response = FormatResponse(
            "RTSP/1.0 200 OK",
            $"CSeq: {optionsResponse.Sequence}",
            $"Date: {DateTimeOffset.UtcNow:o}",
            $"Public: {string.Join(',', optionsResponse.Methods)}");
        
        _logger.LogTrace("Sent response: {response}", Encoding.UTF8.GetString(response));

        await stream.WriteAsync(response);
    }

    private async Task SendDescribeResponse(NetworkStream stream, RtspDescribeResponse describeResponse)
    {
        var sdp = describeResponse.SessionDescriptionProtocol.ToString();
        var sdpSize = Encoding.UTF8.GetByteCount(sdp);

        var response = FormatSdpResponse(
            "RTSP/1.0 200 OK",
            $"CSeq: {describeResponse.Sequence}",
            "Content-Type: application/sdp",
            $"Content-Length: {sdpSize}",
            $"\r\n{sdp}");
        
        _logger.LogTrace("Sent response: {response}", Encoding.UTF8.GetString(response));

        await stream.WriteAsync(response);
    }

    private async Task SendSetupResponse(NetworkStream stream, RtspSetupResponse setupResponse)
    {
        var response = FormatResponse(
            "RTSP/1.0 200 OK",
            $"CSeq: {setupResponse.Sequence}",
            $"Date: {DateTimeOffset.UtcNow:o}",
            $"Session: {setupResponse.SessionId}",
            $"Transport: {setupResponse.Transport}");
        
        _logger.LogTrace("Sent response: {response}", Encoding.UTF8.GetString(response));

        await stream.WriteAsync(response);
    }
    
    private async Task SendPlayResponse(NetworkStream stream, RtspPlayResponse playResponse)
    {
        var response = FormatResponse(
            "RTSP/1.0 200 OK",
            $"CSeq: {playResponse.Sequence}",
            $"Date: {DateTimeOffset.UtcNow:o}",
            $"Session: {playResponse.SessionId}");
        
        _logger.LogTrace("Sent response: {response}", Encoding.UTF8.GetString(response));

        await stream.WriteAsync(response);
    }
    
    private async Task SendTeardownResponse(NetworkStream stream, RtspTeardownResponse teardownResponse)
    {
        var response = FormatResponse(
            "RTSP/1.0 200 OK",
            $"CSeq: {teardownResponse.Sequence}",
            $"Date: {DateTimeOffset.UtcNow:o}");
        
        _logger.LogTrace("Sent response: {response}", Encoding.UTF8.GetString(response));

        await stream.WriteAsync(response);
    }

    private static byte[] FormatResponse(params string[] lines)
    {
        var text = string.Join("\r\n", lines) + "\r\n\r\n";
        return Encoding.UTF8.GetBytes(text);
    }

    private static byte[] FormatSdpResponse(params string[] lines)
    {
        var text = string.Join("\r\n", lines);
        return Encoding.UTF8.GetBytes(text);
    }
}