using ManagementServer.Application.Commands;
using ManagementServer.Domain.Abstract;
using MediatR;

namespace ManagementServer.Application.Handlers;

public class StartFuzzingHandler : IRequestHandler<StartFuzzingCommand>
{
    private readonly IFuzzingService _fuzzingService;

    public StartFuzzingHandler(IFuzzingService fuzzingService)
    {
        _fuzzingService = fuzzingService;
    }

    public Task Handle(StartFuzzingCommand request, CancellationToken cancellationToken)
    {
        _ = Task.Run(async () => await _fuzzingService.StartFuzzingAsync(request.SessionId), cancellationToken);

        return Task.CompletedTask;
    }
}