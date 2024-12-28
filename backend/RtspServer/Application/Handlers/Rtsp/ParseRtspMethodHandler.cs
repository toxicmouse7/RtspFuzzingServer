using MediatR;
using RtspServer.Application.Commands;
using RtspServer.Enums;

namespace RtspServer.Application.Handlers.Rtsp;

public class ParseRtspMethodHandler : IRequestHandler<ParseRtspMethodCommand, RtspMethod>
{
    public Task<RtspMethod> Handle(ParseRtspMethodCommand request, CancellationToken cancellationToken)
    {
        var method = request.Method switch
        {
            "OPTIONS" => RtspMethod.Options,
            "DESCRIBE" => RtspMethod.Describe,
            "SETUP" => RtspMethod.Setup,
            "PLAY" => RtspMethod.Play,
            "TEARDOWN" => RtspMethod.Teardown,
            _ => throw new ArgumentOutOfRangeException(nameof(request), request.Method, "Unknown method")
        };

        return Task.FromResult(method);
    }
}