using System.Net;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Domain.Abstract;

public interface IRtspSessionsRepo
{
    Task<RtspSession> CreateSessionAsync(IPEndPoint rtpEndPoint, IPEndPoint rtcpEndPoint);
    Task<RtspSession?> GetSessionAsync(long sessionId);
    Task RemoveSessionAsync(long sessionId);
    Task<IReadOnlyCollection<RtspSession>> GetSessionsAsync();
}