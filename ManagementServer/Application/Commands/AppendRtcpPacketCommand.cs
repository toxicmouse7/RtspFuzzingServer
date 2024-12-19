using MediatR;
using RtspServer.Domain.Models.Abstract;
using RtspServer.Domain.Models.Sessions;

namespace ManagementServer.Application.Commands;

public record AppendRtcpPacketCommand(RtspSession Session, RtcpPacket Packet) : IRequest;