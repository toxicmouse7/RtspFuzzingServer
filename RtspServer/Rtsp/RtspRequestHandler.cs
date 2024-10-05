using RtspServer.SDP;

namespace RtspServer.Rtsp;

public class RtspRequestHandler
{
    public Task<string> HandleAsync(RtspRequest rtspRequest)
    {
        return rtspRequest.Method switch
        {
            RtspRequestMethod.Options => HandleOptionsAsync(rtspRequest),
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
            new Origin("255.255.123.123"),
            new Media(5050));
        
        return Task.FromResult(
            "RTSP/1.0 200 OK\r\n" +
            $"CSeq: {rtspRequest.Headers.First(h => h.Key == "CSeq").Value}\r\n" +
            "Content-Type: application/sdp\r\n");
    }
}