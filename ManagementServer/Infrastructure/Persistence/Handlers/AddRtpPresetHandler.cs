using AutoMapper;
using ManagementServer.Application.Commands;
using ManagementServer.Domain.Models;
using MediatR;

namespace ManagementServer.Infrastructure.Persistence.Handlers;

public class AddRtpPresetHandler : IRequestHandler<AddRtpPresetCommand, RtpFuzzingPreset>
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public AddRtpPresetHandler(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RtpFuzzingPreset> Handle(AddRtpPresetCommand request, CancellationToken cancellationToken)
    {
        var dbPreset = _mapper.Map<Models.RtpFuzzingPreset>(request.Preset);
        await _context.RtpFuzzingPresets.AddAsync(dbPreset, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RtpFuzzingPreset>(dbPreset);
    }
}