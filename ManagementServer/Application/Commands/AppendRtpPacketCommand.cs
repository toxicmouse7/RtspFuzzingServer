using ManagementServer.Settings;
using MediatR;
using RtspServer.Domain.Models.Rtp;

namespace ManagementServer.Application.Commands;

public record AppendRtpPacketCommand(long SessionId, RtpPacket RtpPacket, AppendSettings AppendSettings) : IRequest;