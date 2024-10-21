using RtspServer.Attributes;
using RtspServer.Rtsp;
using RtspServer.RtspResponses;
using RtspServer.SDP;
using SessionDescriptionProtocol = RtspServer.SDP.SessionDescriptionProtocol;

namespace RtspServer.RtspController;

public class RtspController : RtspControllerBase
{
    [RtspDescribe]
    public IRtspResponse Describe()
    {
        var sdp = new SessionDescriptionProtocol(
            new Origin("192.168.50.17"),
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
        
        var clientPort = request.Headers["Transport"]
            .Split(';')
            .First(t => t.StartsWith("client_port"));
        
        return Ok(new Dictionary<string, string>
        {
            ["Session"] = Random.Shared.Next().ToString(),
            ["Transport"] = $"RTP/AVP;unicast;{clientPort};server_port=12345-12346"
        });
    }

    [RtspPlay]
    public IRtspResponse Play()
    {
        return NotImplemented();
    }

    [RtspTeardown]
    public IRtspResponse Teardown()
    {
        return Ok();
    }
}