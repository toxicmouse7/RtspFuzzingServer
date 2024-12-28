using MediatR;

namespace RtspServer.Application.Commands;

public record ParseRtspHeadersCommand(IEnumerable<string> Data) : IRequest<IReadOnlyDictionary<string, string>>;