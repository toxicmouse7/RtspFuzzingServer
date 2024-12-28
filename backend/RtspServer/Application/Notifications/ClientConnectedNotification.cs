using System.Net.Sockets;
using MediatR;

namespace RtspServer.Application.Notifications;

public record ClientConnectedNotification(TcpClient Client) : INotification;