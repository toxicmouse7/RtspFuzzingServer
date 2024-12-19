using System.Net;
using MediatR;
using RtspServer.Application.Handlers.Session;
using RtspServer.Application.Notifications;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Infrastructure;

public class RtspSessionsRepo : IRtspSessionsRepo
{
    private readonly Dictionary<long, RtspSession> _sessions = new();
    private readonly IPublisher _publisher;

    public RtspSessionsRepo(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task<RtspSession> CreateSessionAsync(IPEndPoint rtpEndPoint, IPEndPoint rtcpEndPoint)
    {
        var session = new RtspSession(rtpEndPoint, rtcpEndPoint, _publisher);
        _sessions.Add(session.Id, session);

        await _publisher.Publish(new SessionCreatedNotification(session));

        return session;
    }

    public Task<RtspSession?> GetSessionAsync(long sessionId)
    {
        _sessions.TryGetValue(sessionId, out var session);

        return Task.FromResult(session);
    }

    public Task RemoveSessionAsync(long sessionId)
    {
        _sessions.Remove(sessionId);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<RtspSession>> GetSessionsAsync()
    {
        return Task.FromResult<IReadOnlyCollection<RtspSession>>(_sessions.Values);
    }
}