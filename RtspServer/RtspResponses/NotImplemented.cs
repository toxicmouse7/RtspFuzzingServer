using RtspServer.Rtsp;

namespace RtspServer.RtspResponses;

public class NotImplemented : IRtspResponse
{
    public byte[] Compile(RtspRequest _)
    {
        return "RTSP/1.0 501 Not Implemented\r\n\r\n"u8.ToArray();
    }
}