using AutoMapper;
using ManagementServer.Application.Commands;
using ManagementServer.Domain.Abstract;
using ManagementServer.Domain.Models;
using ManagementServer.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RtspServer.Domain.Models.Rtp;

namespace ManagementServer.Infrastructure.Persistence.Handlers;

public class AddRtpPresetHandler : IRequestHandler<AddRtpPresetCommand, RtpFuzzingPreset>
{
    private readonly ApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly IRtpFuzzingPayloadGenerator _generator;

    public AddRtpPresetHandler(
        ApplicationContext context,
        IMapper mapper,
        IRtpFuzzingPayloadGenerator generator)
    {
        _context = context;
        _mapper = mapper;
        _generator = generator;
    }

    public async Task<RtpFuzzingPreset> Handle(AddRtpPresetCommand request, CancellationToken cancellationToken)
    {
        var preset = request.Preset;
        var generatePayloadsFor = TimeSpan.FromSeconds(10);
        
        var generatedPayloads = await _generator.GenerateRtpPayloadsAsync(preset, generatePayloadsFor);
        preset.AddRawFuzzingData(generatedPayloads);
        
        var dbPreset = _mapper.Map<Models.RtpFuzzingPreset>(preset);
        await _context.RtpFuzzingPresets.AddAsync(dbPreset, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RtpFuzzingPreset>(dbPreset);
    }
}