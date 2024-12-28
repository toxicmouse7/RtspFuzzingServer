using ManagementServer.Application.Commands;
using ManagementServer.Domain.Abstract;
using MediatR;
using RtspServer.Application.Queries;

namespace ManagementServer.Application.Handlers;

public class StopFuzzingHandler : IRequestHandler<StopFuzzingCommand>
{
    private readonly IFuzzingService _fuzzingService;
    private readonly ISender _sender;

    public StopFuzzingHandler(IFuzzingService fuzzingService, ISender sender)
    {
        _fuzzingService = fuzzingService;
        _sender = sender;
    }

    public async Task Handle(StopFuzzingCommand request, CancellationToken cancellationToken)
    {
        var session = await _sender.Send(new GetSessionQuery(request.SessionId), cancellationToken);
        if (session is null)
        {
            return;
        }
        
        _fuzzingService.StopFuzzing(session);
    }
}