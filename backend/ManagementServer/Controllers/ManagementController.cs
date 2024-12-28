using AutoMapper;
using ManagementServer.Application.Commands;
using ManagementServer.Domain.Abstract;
using ManagementServer.Domain.Models;
using ManagementServer.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Models.Abstract;
using RtspServer.Domain.Models.Rtcp;
using RtspServer.Domain.Models.Rtp;
using RtspServer.Extensions;

namespace ManagementServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ManagementController : ControllerBase
{
    private readonly IRtspSessionsRepo _sessionsRepo;
    private readonly IFuzzingService _fuzzingService;
    private readonly IMapper _mapper;
    private readonly ISender _sender;

    public ManagementController(
        IRtspSessionsRepo sessionsRepo,
        IFuzzingService fuzzingService,
        IMapper mapper,
        ISender sender)
    {
        _sessionsRepo = sessionsRepo;
        _fuzzingService = fuzzingService;
        _mapper = mapper;
        _sender = sender;
    }

    [HttpDelete("sessions")]
    public async Task<IActionResult> CloseSession([FromQuery] string sessionId)
    {
        var session = await _sessionsRepo.GetSessionAsync(sessionId.ToInt64());
        if (session is null)
        {
            return NotFound();
        }
        
        await session.CloseAsync();
        
        return Ok();
    }

    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessions()
    {
        return Ok(_mapper.Map<IEnumerable<Dto.Rest.Out.Session>>(await _sessionsRepo.GetSessionsAsync()));
    }

    [HttpPost("send_rtp")]
    public async Task<IActionResult> SendRTP([FromQuery] string sessionId, [FromBody] Dto.Rest.RTPPacket rtpPacket)
    {
        var session = await _sessionsRepo.GetSessionAsync(sessionId.ToInt64());
        if (session is null)
        {
            return NotFound();
        }

        var appendSettings = new AppendSettings
        {
            UseOriginalPayload = rtpPacket.Base64Content is null,
            UseOriginalTimestamp = rtpPacket.RTPHeader.Timestamp is null,
            UseOriginalSequence = rtpPacket.RTPHeader.SequenceNumber is null
        };

        var sendRtpPacketCommand = new AppendRtpPacketCommand(
            sessionId.ToInt64(),
            _mapper.Map<RtpPacket>(rtpPacket),
            appendSettings);
        await _sender.Send(sendRtpPacketCommand);

        return Ok();
    }

    [HttpPost("send_rtcp")]
    public async Task<IActionResult> SendRTCP([FromQuery] string sessionId, [FromBody] Dto.Rest.SenderReport senderReport)
    {
        var session = await _sessionsRepo.GetSessionAsync(sessionId.ToInt64());
        if (session is null)
        {
            return NotFound();
        }
        
        var sendRtcpPacketCommand = new AppendRtcpPacketCommand(session, _mapper.Map<RtcpPacket>(senderReport));
        await _sender.Send(sendRtcpPacketCommand);

        return Ok();
    }

    [HttpGet("rtp_presets")]
    public async Task<IActionResult> GetRtpPresets()
    {
        var presets = await _fuzzingService.GetAllPresetsAsync();
        return Ok(_mapper.Map<IEnumerable<Dto.Rest.Out.RtpFuzzingPreset>>(presets));
    }
    
    [HttpDelete("rtp_presets/{presetId:guid}")]
    public async Task<IActionResult> DeleteRtpPreset(Guid presetId)
    {
        await _fuzzingService.RemovePresetAsync(presetId);
        return Ok();
    }

    [HttpPost("rtp_presets")]
    public async Task<IActionResult> AddRtpPreset([FromBody] Dto.Rest.RTPPacket packet)
    {
        var preset = _mapper.Map<RtpFuzzingPreset>(packet);

        var domainPreset = await _fuzzingService.AddRtpPresetAsync(preset);
        
        return Ok(_mapper.Map<Dto.Rest.Out.RtpFuzzingPreset>(domainPreset));
    }

    [HttpPost("start_rtp_fuzzing")]
    public async Task<IActionResult> StartRtpFuzzing([FromQuery] string sessionId)
    {
        var startFuzzingCommand = new StartFuzzingCommand(sessionId.ToInt64());
        await _sender.Send(startFuzzingCommand);
        
        return Accepted();
    }

    [HttpPost("stop_rtp_fuzzing")]
    public async Task<IActionResult> StopRtpFuzzing([FromQuery] string sessionId)
    {
        await _sender.Send(new StopFuzzingCommand(sessionId.ToInt64()));
        
        return Ok();
    }
}