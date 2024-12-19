using MediatR;
using RtspServer.Enums;

namespace RtspServer.Application.Commands;

public record ParseRtspMethodCommand(string Method) : IRequest<RtspMethod>;