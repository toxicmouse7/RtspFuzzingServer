namespace RtspServer.Rtsp;

public sealed class RtspRequest(
    RtspRequestMethod method, string uri, string protocol, Dictionary<string, string> headers)
{
    public RtspRequestMethod Method { get; } = method;
    public string Uri { get; } = uri;
    public string Protocol { get; } = protocol;
    public Dictionary<string, string> Headers { get; } = headers;

    public override string ToString()
    {
        var requestLine = $"{Method} {Uri} {Protocol}\r\n";
        var headers = string.Join("\r\n", Headers.Select(h => $"{h.Key}: {h.Value}"));
        
        return requestLine + headers;
    }
}