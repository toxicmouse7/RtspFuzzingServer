using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using RtspServer.Application.Commands;
using RtspServer.Domain.Models.Abstract;
using RtspServer.Domain.Models.RtspRequests;
using RtspServer.Enums;
using RtspServer.Extensions;

namespace RtspServer.Application.Handlers.Rtsp;

public class ParseRequestHandler : IRequestHandler<ParseRequestCommand, RtspRequest>
{
    private readonly ISender _sender;
    private readonly ILogger<ParseRequestHandler> _logger;

    public ParseRequestHandler(ISender sender, ILogger<ParseRequestHandler> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task<RtspRequest> Handle(ParseRequestCommand request, CancellationToken cancellationToken)
    {
        var (data, address) = request;
        
        var stringData = Encoding.UTF8.GetString(data);
        _logger.LogTrace("Received request: {request}", stringData);
        
        var lines = stringData.Split("\r\n").ToArray();
        
        var requestLineParts = lines[0].Split(' ');

        var parseMethodCommand = new ParseRtspMethodCommand(requestLineParts[0]);
        var method = await _sender.Send(parseMethodCommand, cancellationToken);

        var parseHeadersCommand = new ParseRtspHeadersCommand(lines.Skip(1));
        var headers = await _sender.Send(parseHeadersCommand, cancellationToken);

        return method switch
        {
            RtspMethod.Options => new RtspOptionsRequest(headers, address),
            RtspMethod.Describe => new RtspDescribeRequest(headers, address),
            RtspMethod.Setup => new RtspSetupRequest(headers, address),
            RtspMethod.Play => new RtspPlayRequest(GetSession(), headers, address),
            RtspMethod.Teardown => new RtspTeardownRequest(GetSession(), headers, address),
            _ => throw new ArgumentOutOfRangeException()
        };

        long GetSession() => headers["Session"].ToInt64();
    }
}