using MediatR;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Application.Commands;

public record PlayCommand(RtspSession Session) : IRequest;