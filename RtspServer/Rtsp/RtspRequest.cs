namespace RtspServer.Rtsp;

public sealed class RtspRequest(
    string method,
    string uri,
    string protocol,
    string ip,
    Dictionary<string, string> headers)
{
    public string Method { get; } = method;
    public string Uri { get; } = uri;
    public string Protocol { get; } = protocol;
    public string Ip { get; } = ip;
    public Dictionary<string, string> Headers { get; } = headers;

    public string CSeq => Headers["CSeq"];

    public override string ToString()
    {
        var requestLine = $"{Method} {Uri} {Protocol}\r\n";
        var headers = string.Join("\r\n", Headers.Select(h => $"{h.Key}: {h.Value}"));

        return requestLine + headers;
    }
}