using System.Net;
using MediatR;
using RtspServer.Domain.Models.Abstract;

namespace RtspServer.Application.Commands;

public record ParseRequestCommand(byte[] Data, IPAddress Address) : IRequest<RtspRequest>;