using MediatR;

namespace ManagementServer.Application.Commands;

public record StartFuzzingCommand(long SessionId) : IRequest;