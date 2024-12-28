using RtspServer.Domain.Models.Rtp;

namespace RtspServer.Domain.Abstract;

public interface IRtpPacketSource
{
    Task<RtpPacket> GetPacketAsync();
}