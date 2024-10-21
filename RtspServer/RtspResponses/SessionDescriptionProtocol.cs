using System.Text;
using RtspServer.Rtsp;

namespace RtspServer.RtspResponses;

public class SessionDescriptionProtocol(SDP.SessionDescriptionProtocol protocol) : IRtspResponse
{
    public byte[] Compile(RtspRequest request)
    {
        var sdpSize = Encoding.UTF8.GetByteCount(protocol.ToString());

        return Encoding.UTF8.GetBytes(
            "RTSP/1.0 200 OK\r\n" +
            $"CSeq: {request.Headers.First(h => h.Key == "CSeq").Value}\r\n" +
            "Content-Type: application/sdp\r\n" +
            $"Content-Length: {sdpSize}\r\n" +
            "\r\n" +
            $"{protocol}");
    }
}