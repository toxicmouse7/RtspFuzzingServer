using AutoMapper;
using ManagementServer.Application.Queries;
using ManagementServer.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ManagementServer.Infrastructure.Persistence.Handlers;

public class GetFuzzingPresetsHandler 
    : IRequestHandler<GetFuzzingPresetsQuery, IReadOnlyCollection<RtpFuzzingPreset>>
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;

    public GetFuzzingPresetsHandler(ApplicationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<RtpFuzzingPreset>> Handle(
        GetFuzzingPresetsQuery request,
        CancellationToken cancellationToken)
    {
        var dbPresets = await _context.RtpFuzzingPresets
            .Include(x => x.RawFuzzingData)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<IReadOnlyCollection<RtpFuzzingPreset>>(dbPresets);
    }
}