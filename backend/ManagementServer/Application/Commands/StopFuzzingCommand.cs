using MediatR;

namespace ManagementServer.Application.Commands;

public record StopFuzzingCommand(long SessionId) : IRequest;