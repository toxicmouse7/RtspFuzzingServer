using System.Net;
using MediatR;
using RtspServer.Application.Notifications;

namespace RtspServer.Domain.Models.Sessions;

public sealed class RtspSession : IDisposable
{
    private readonly CancellationTokenSource _cts = new();
    private readonly IPublisher _publisher;

    public RtspSession(IPEndPoint rtpEndPoint, IPEndPoint rtcpEndPoint, IPublisher publisher)
    {
        RTPEndPoint = rtpEndPoint;
        RTCPEndPoint = rtcpEndPoint;
        _publisher = publisher;
        Id = Random.Shared.NextInt64();
    }
    
    public long Id { get; }
    public IPEndPoint RTPEndPoint { get; }
    public IPEndPoint RTCPEndPoint { get; }
    public CancellationToken Token => _cts.Token;

    public async Task CloseAsync()
    {
        await _cts.CancelAsync();
        await _publisher.Publish(new SessionClosedNotification(this), CancellationToken.None);
    }

    public void Dispose()
    {
        _cts.Dispose();
    }
}