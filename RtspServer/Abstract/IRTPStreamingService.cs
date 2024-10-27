using RtspServer.Domain.Models;

namespace RtspServer.Abstract;

public interface IRTPStreamingService
{
    void StartRTPStream(Session session);
    void StopRTPStream(Session session);
}