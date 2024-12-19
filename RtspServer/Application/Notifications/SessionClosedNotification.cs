using MediatR;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Application.Notifications;

public record SessionClosedNotification(RtspSession Session) : INotification;