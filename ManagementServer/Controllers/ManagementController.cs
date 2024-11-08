using Microsoft.AspNetCore.Mvc;
using RtspServer.Abstract;

namespace ManagementServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ManagementController : ControllerBase
{
    private readonly ISessionService _sessionService;
    private readonly IRTPStreamingService _rtpStreamingService;

    public ManagementController(
        ISessionService sessionService,
        IRTPStreamingService rtpStreamingService)
    {
        _sessionService = sessionService;
        _rtpStreamingService = rtpStreamingService;
    }

    [HttpDelete]
    public IActionResult DeleteSession([FromQuery] long sessionId)
    {
        var session = _sessionService.GetSession(sessionId);
        if (session is null)
        {
            return NotFound();
        }
        
        _rtpStreamingService.StopRTPStream(session);
        
        _sessionService.DeleteSession(sessionId);
        
        return Ok();
    }

    [HttpGet("sessions")]
    public IActionResult GetSessions()
    {
        return Ok(_sessionService.GetSessions());
    }
}