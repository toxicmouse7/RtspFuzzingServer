using MediatR;
using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Application.Commands;

public record HandleRequestCommand(RtspRequest Request) : IRequest<RtspResponse>;