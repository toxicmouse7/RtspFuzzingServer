using ManagementServer.Domain.Models;
using MediatR;

namespace ManagementServer.Application.Queries;

public record GetFuzzingPresetsQuery : IRequest<IReadOnlyCollection<RtpFuzzingPreset>>;