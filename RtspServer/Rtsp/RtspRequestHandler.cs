using System.Text;
using RtspServer.SDP;

namespace RtspServer.Rtsp;

public class RtspRequestHandler
{
    public Task<string> HandleAsync(RtspRequest rtspRequest)
    {
        return rtspRequest.Method switch
        {
            RtspRequestMethod.Options => HandleOptionsAsync(rtspRequest),
            RtspRequestMethod.Describe => HandleDescribeAsync(rtspRequest),
            RtspRequestMethod.Setup => HandleSetupAsync(rtspRequest),
            _ => HandleUnsupportedAsync()
        };
    }

    private static Task<string> HandleOptionsAsync(RtspRequest rtspRequest)
    {
        return Task.FromResult(
            "RTSP/1.0 200 OK\r\n" +
            $"CSeq: {rtspRequest.Headers.First(h => h.Key == "CSeq").Value}\r\n" +
            "Public: DESCRIBE, SETUP, TEARDOWN, PLAY, PAUSE\r\n\r\n");
    }

    private static Task<string> HandleUnsupportedAsync()
    {
        return Task.FromResult(
            "RTSP/1.0 501 Not Implemented\r\n\r\n");
    }

    private Task<string> HandleDescribeAsync(RtspRequest rtspRequest)
    {
        var sdp = new SessionDescriptionProtocol(
            new Origin("192.168.50.17"),
            new Media(5050));

        var sdpSize = Encoding.UTF8.GetByteCount(sdp.ToString());
        
        return Task.FromResult(
            "RTSP/1.0 200 OK\r\n" +
            $"CSeq: {rtspRequest.Headers.First(h => h.Key == "CSeq").Value}\r\n" +
            "Content-Type: application/sdp\r\n" +
            $"Content-Length: {sdpSize}\r\n" +
            "\r\n" +
            $"{sdp}");
    }

    private async Task<string> HandleSetupAsync(RtspRequest rtspRequest)
    {
        var availableTransport = rtspRequest.Headers.First(h => h.Key == "Transport").Value.Split(';');
        if (!availableTransport.Any(t => t.Contains("RTP/AVP")))
        {
            return await HandleUnsupportedAsync();
        }
        
        var clientPort = rtspRequest.Headers.First(h => h.Key == "Transport").Value
            .Split(';')
            .First(t => t.StartsWith("client_port"));

        return "RTSP/1.0 200 OK\r\n" +
               $"CSeq: {rtspRequest.Headers.First(h => h.Key == "CSeq").Value}\r\n" +
               $"Date: {DateTimeOffset.UtcNow:o}\r\n" +
               $"Session: {Random.Shared.Next()}\r\n" +
               $"Transport: RTP/AVP;unicast;{clientPort};server_port=12345-12346\r\n" +
               "\r\n";
    }
}