using MediatR;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Application.Queries;

public record GetSessionQuery(long SessionId) : IRequest<RtspSession?>;