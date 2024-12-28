using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Domain.Abstract;

public interface IRtcpPacketSourceFactory
{
    Task<IRtcpPacketSource> CreatePacketSourceAsync(RtspSession session);
}