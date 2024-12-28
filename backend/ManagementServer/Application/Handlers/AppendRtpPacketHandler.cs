using ManagementServer.Application.Commands;
using ManagementServer.Infrastructure;
using MediatR;
using RtspServer.Domain.Abstract;

namespace ManagementServer.Application.Handlers;

public class AppendRtpPacketHandler : IRequestHandler<AppendRtpPacketCommand>
{
    private readonly IRtspSessionsRepo _sessionsRepo;
    private readonly RtpPacketSourceManager _sourceManager;

    public AppendRtpPacketHandler(IRtspSessionsRepo sessionsRepo, RtpPacketSourceManager sourceManager)
    {
        _sessionsRepo = sessionsRepo;
        _sourceManager = sourceManager;
    }

    public async Task Handle(AppendRtpPacketCommand request, CancellationToken cancellationToken)
    {
        var (sessionId, rtpPacket, appendSettings) = request;

        var session = await _sessionsRepo.GetSessionAsync(sessionId);
        if (session is null)
        {
            return;
        }
        
        var source = await _sourceManager.GetPacketSourceAsync(session) as RtpPacketSource;
        await source!.AppendPacketAsync(rtpPacket, appendSettings);
    }
}