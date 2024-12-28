using MediatR;
using RtspServer.Application.Queries;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Application.Handlers.Session;

public class GetSessionHandler : IRequestHandler<GetSessionQuery, RtspSession?>
{
    private readonly IRtspSessionsRepo _sessionsRepo;

    public GetSessionHandler(IRtspSessionsRepo sessionsRepo)
    {
        _sessionsRepo = sessionsRepo;
    }

    public async Task<RtspSession?> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        return await _sessionsRepo.GetSessionAsync(request.SessionId);
    }
}