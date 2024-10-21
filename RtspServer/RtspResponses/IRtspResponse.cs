using RtspServer.Rtsp;

namespace RtspServer.RtspResponses;

public interface IRtspResponse
{
    public byte[] Compile(RtspRequest request);
}