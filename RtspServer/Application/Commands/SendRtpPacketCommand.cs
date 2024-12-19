using System.Net;
using MediatR;
using RtspServer.Domain.Models.Rtp;

namespace RtspServer.Application.Commands;

public record SendRtpPacketCommand(RtpPacket Packet, IPEndPoint EndPoint) : IRequest;