using System.Net;
using MediatR;
using RtspServer.Application.Commands;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Models.Abstract;
using RtspServer.Domain.Models.RtspRequests;
using RtspServer.Domain.Models.RtspResponses;
using RtspServer.Domain.Models.SessionDescriptionProtocol;
using RtspServer.Extensions;
using RtspServer.Infrastructure;

namespace RtspServer.Application.Handlers.Rtsp;

public class HandleRequestHandler : IRequestHandler<HandleRequestCommand, RtspResponse>
{
    private readonly INetworkService _networkService;
    private readonly IRtspSessionsRepo _sessionsRepo;
    private readonly ISender _sender;

    public HandleRequestHandler(
        INetworkService networkService,
        IRtspSessionsRepo sessionsRepo,
        ISender sender)
    {
        _networkService = networkService;
        _sessionsRepo = sessionsRepo;
        _sender = sender;
    }

    public Task<RtspResponse> Handle(HandleRequestCommand request, CancellationToken cancellationToken)
    {
        var rtspRequest = request.Request;

        return rtspRequest switch
        {
            RtspDescribeRequest describeRequest => HandleDescribe(describeRequest),
            RtspOptionsRequest optionsRequest => HandleOptions(optionsRequest),
            RtspPlayRequest playRequest => HandlePlay(playRequest),
            RtspSetupRequest setupRequest => HandleSetup(setupRequest),
            RtspTeardownRequest teardownRequest => HandleTeardown(teardownRequest),
            _ => Task.FromResult<RtspResponse>(new RtspNotImplementedResponse())
        };
    }

    private Task<RtspResponse> HandleOptions(RtspOptionsRequest optionsRequest)
    {
        var response = new RtspOptionsResponse(
            ["OPTIONS", "DESCRIBE", "SETUP", "PLAY", "TEARDOWN"],
            optionsRequest.Sequence);
        return Task.FromResult<RtspResponse>(response);
    }

    private Task<RtspResponse> HandleDescribe(RtspDescribeRequest describeRequest)
    {
        var sdp = new SessionDescriptionProtocol(
            new Origin(_networkService.GetLocalIpAddress()),
            new Media(7070));

        var response = new RtspDescribeResponse(sdp, describeRequest.Sequence);

        return Task.FromResult<RtspResponse>(response);
    }

    private async Task<RtspResponse> HandleSetup(RtspSetupRequest setupRequest)
    {
        if (!setupRequest.Transport.Any(t => t.Contains("RTP/AVP")))
        {
            return new RtspNotImplementedResponse();
        }


        var clientPort = setupRequest.Transport
            .First(t => t.StartsWith("client_port"))
            .Replace("client_port=", string.Empty);

        var rtpPort = clientPort.Split("-").First().ToUInt16();
        var rtcpPort = clientPort.Split("-").Last().ToUInt16();

        var session = await _sessionsRepo.CreateSessionAsync(
            IPEndPoint.Parse($"{setupRequest.Address}:{rtpPort}"),
            IPEndPoint.Parse($"{setupRequest.Address}:{rtcpPort}"));

        var response = new RtspSetupResponse(
            session.Id,
            $"RTP/AVP;unicast;client_port={clientPort};server_port=12345-12346",
            setupRequest.Sequence);

        return response;
    }

    private async Task<RtspResponse> HandlePlay(RtspPlayRequest playRequest)
    {
        var response = new RtspPlayResponse(playRequest.SessionId, playRequest.Sequence);

        var session = await _sessionsRepo.GetSessionAsync(playRequest.SessionId);

        if (session is null)
        {
            return new RtspNotImplementedResponse();
        }

        var playCommand = new PlayCommand(session);

        await _sender.Send(playCommand);

        return response;
    }

    private async Task<RtspResponse> HandleTeardown(RtspTeardownRequest teardownRequest)
    {
        var session = await _sessionsRepo.GetSessionAsync(teardownRequest.SessionId);
        if (session is not null)
        {
            await session.CloseAsync();
        }

        var response = new RtspTeardownResponse(teardownRequest.Sequence);

        return response;
    }
}