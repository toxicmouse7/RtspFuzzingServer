using System.Text;
using RtspServer.Rtsp;

namespace RtspServer.RtspResponses;

public class Ok : IRtspResponse
{
    private readonly Dictionary<string, string> _headers;

    public Ok(Dictionary<string, string>? headers = null)
    {
        _headers = headers ?? new Dictionary<string, string>();
    }
    
    public byte[] Compile(RtspRequest request)
    {
        return Encoding.UTF8.GetBytes(
            "RTSP/1.0 200 OK\r\n" +
            $"CSeq: {request.CSeq}\r\n" +
            $"Date: {DateTimeOffset.UtcNow:o}\r\n" +
            string.Join("\r\n", _headers.Select(h => $"{h.Key}: {h.Value}"))
            + "\r\n\r\n");
    }
}