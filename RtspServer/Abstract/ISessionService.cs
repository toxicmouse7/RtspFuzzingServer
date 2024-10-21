using RtspServer.Domain.Models;

namespace RtspServer.Abstract;

public interface ISessionService
{
    Session CreateSession(long clientPort, string ip);
    Session GetSession(long sessionId);
}