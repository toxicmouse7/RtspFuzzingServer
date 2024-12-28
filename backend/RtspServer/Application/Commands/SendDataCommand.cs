using System.Net;
using MediatR;

namespace RtspServer.Application.Commands;

public record SendDataCommand(byte[] Data, IPEndPoint EndPoint) : IRequest;