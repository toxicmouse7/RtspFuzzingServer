using MediatR;
using RtspServer.Domain.Models.Sessions;

namespace RtspServer.Application.Notifications;

public record SessionCreatedNotification(RtspSession Session) : INotification;