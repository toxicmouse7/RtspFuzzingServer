using System.Net;
using MediatR;
using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Application.Commands;

public record SendRtcpPacketCommand(RtcpPacket Packet, IPEndPoint EndPoint) : IRequest;
