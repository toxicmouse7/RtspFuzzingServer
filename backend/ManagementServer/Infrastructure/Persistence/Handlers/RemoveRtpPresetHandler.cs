using ManagementServer.Application.Commands;
using MediatR;

namespace ManagementServer.Infrastructure.Persistence.Handlers;

public class RemoveRtpPresetHandler : IRequestHandler<RemoveRtpPresetCommand>
{
    private readonly ApplicationContext _context;

    public RemoveRtpPresetHandler(ApplicationContext context)
    {
        _context = context;
    }

    public async Task Handle(RemoveRtpPresetCommand request, CancellationToken cancellationToken)
    {
        var query = _context.RtpFuzzingPresets.Where(x => x.Id == request.PresetId);
        _context.RemoveRange(query);
        await _context.SaveChangesAsync(cancellationToken);
    }
}