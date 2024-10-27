using System.Net;
using System.Net.Sockets;
using RtspServer.Abstract;
using RtspServer.Attributes;
using RtspServer.Rtsp;
using RtspServer.RtspResponses;
using RtspServer.SDP;
using SessionDescriptionProtocol = RtspServer.SDP.SessionDescriptionProtocol;

namespace RtspServer.RtspController;

public class RtspController : RtspControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly IRTPStreamingService _rtpStreamingService;

    public RtspController(
        ISessionService sessionService,
        IRTPStreamingService rtpStreamingService)
    {
        _sessionService = sessionService;
        _rtpStreamingService = rtpStreamingService;
    }
    
    [RtspDescribe]
    public IRtspResponse Describe()
    {
        var sdp = new SessionDescriptionProtocol(
            new Origin(GetLocalIPAddress()),
            new Media(5050));
        
        return SessionDescriptionProtocol(sdp);
    }

    [RtspSetup]
    public IRtspResponse Setup(RtspRequest request)
    {
        var availableTransport = request.Headers["Transport"].Split(';');
        if (!availableTransport.Any(t => t.Contains("RTP/AVP")))
        {
            return NotImplemented();
        }
        
        var transportOptions = request.Headers["Transport"]
            .Split(';');
        
        var clientPort = transportOptions
            .First(t => t.StartsWith("client_port"));

        var session = _sessionService.CreateSession(
            Convert.ToInt64(
                clientPort
                    .Split('=')
                    .Last()
                    .Split('-')
                    .First()), request.Ip);
        
        return Ok(new Dictionary<string, string>
        {
            ["Session"] = session.Id.ToString(),
            ["Transport"] = $"RTP/AVP;unicast;{clientPort};server_port=12345-12346"
        });
    }

    [RtspPlay]
    public IRtspResponse Play(RtspRequest request)
    {
        var session = _sessionService.GetSession(Convert.ToInt64(request.Headers["Session"]));

        _rtpStreamingService.StartRTPStream(session);
        
        return Ok();
    }

    [RtspTeardown]
    public IRtspResponse Teardown(RtspRequest request)
    {
        var session = _sessionService.GetSession(Convert.ToInt64(request.Headers["Session"]));
        
        _rtpStreamingService.StopRTPStream(session);
        
        return Ok();
    }

    private static string GetLocalIPAddress() // 91 92 91
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ipAddress in host.AddressList)
        {
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                return ipAddress.ToString();
            }
        }
        
        throw new Exception("Unable to get local ip address.");
    }
}