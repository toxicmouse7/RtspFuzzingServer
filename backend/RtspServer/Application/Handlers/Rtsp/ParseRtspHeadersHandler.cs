using MediatR;
using RtspServer.Application.Commands;

namespace RtspServer.Application.Handlers.Rtsp;

public class ParseRtspHeadersHandler 
    : IRequestHandler<ParseRtspHeadersCommand, IReadOnlyDictionary<string, string>>
{
    public Task<IReadOnlyDictionary<string, string>> Handle(ParseRtspHeadersCommand request, CancellationToken cancellationToken)
    {
        var headers = new Dictionary<string, string>();
        foreach (var line in request.Data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }
            
            var colonIndex = line.IndexOf(':');
            if (colonIndex <= 0) continue;
            
            var headerName = line[..colonIndex].Trim();
            var headerValue = line[(colonIndex + 1)..].Trim();
            headers[headerName] = headerValue;
        }

        return Task.FromResult<IReadOnlyDictionary<string, string>>(headers);
    }
}