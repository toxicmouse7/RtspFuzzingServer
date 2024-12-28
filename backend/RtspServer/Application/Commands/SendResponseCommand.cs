using System.Net.Sockets;
using MediatR;
using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Application.Commands;

public record SendResponseCommand(NetworkStream Stream, RtspResponse Response) : IRequest;