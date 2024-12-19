using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Domain.Abstract;

public interface IRtpPacketSourceFactory
{
    Task<IRtpPacketSource> CreatePacketSourceAsync(RtspSession session);
}