using ManagementServer.Domain.Models;
using MediatR;

namespace ManagementServer.Application.Commands;

public record AddRtpPresetCommand(RtpFuzzingPreset Preset) : IRequest<RtpFuzzingPreset>;