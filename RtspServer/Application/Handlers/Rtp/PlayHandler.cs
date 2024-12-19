using MediatR;
using RtspServer.Application.Commands;
using RtspServer.Domain.Abstract.SessionServices;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Application.Handlers.Rtp;

public class PlayHandler : IRequestHandler<PlayCommand>
{
    private readonly IRtpSessionService _rtpSessionService;
    private readonly IRtcpSessionService _rtcpSessionService;

    public PlayHandler(IRtpSessionService rtpSessionService, IRtcpSessionService rtcpSessionService)
    {
        _rtpSessionService = rtpSessionService;
        _rtcpSessionService = rtcpSessionService;
    }

    public Task Handle(PlayCommand request, CancellationToken cancellationToken)
    {
        var rtspSession = request.Session;
        var rtpSession = _rtpSessionService.GetSession(rtspSession);
        var rtcpSession = _rtcpSessionService.GetSession(rtspSession);
        
        rtpSession.Play();
        rtcpSession.Play();
        
        return Task.CompletedTask;
    }
}