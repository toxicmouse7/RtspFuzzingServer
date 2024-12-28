using MediatR;

namespace ManagementServer.Application.Commands;

public record RemoveRtpPresetCommand(Guid PresetId) : IRequest;