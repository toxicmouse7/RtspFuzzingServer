using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Domain.Abstract;

public interface IRtcpPacketSource
{
    Task<RtcpPacket> GetPacketAsync();
    Task AppendPacketAsync(RtcpPacket rtpPacket);
}