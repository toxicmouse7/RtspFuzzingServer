using ManagementServer.Application.Commands;
using ManagementServer.Infrastructure;
using MediatR;

namespace ManagementServer.Application.Handlers;

public class AppendRtcpPacketHandler : IRequestHandler<AppendRtcpPacketCommand>
{
    private readonly RtcpPacketSourceManager _manager;

    public AppendRtcpPacketHandler(RtcpPacketSourceManager manager)
    {
        _manager = manager;
    }

    public async Task Handle(AppendRtcpPacketCommand request, CancellationToken cancellationToken)
    {
        var (session, packet) = request;

        var source = await _manager.GetPacketSourceAsync(session);
        await source.AppendPacketAsync(packet);
    }
}