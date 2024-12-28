using AutoMapper;
using ManagementServer.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RtspServer.Application.Notifications;

namespace ManagementServer.Application.Handlers;

public class SessionCreatedHandler : INotificationHandler<SessionCreatedNotification>
{
    private readonly IHubContext<SessionsHub> _hub;
    private readonly IMapper _mapper;

    public SessionCreatedHandler(IHubContext<SessionsHub> hub, IMapper mapper)
    {
        _hub = hub;
        _mapper = mapper;
    }

    public async Task Handle(SessionCreatedNotification notification, CancellationToken cancellationToken)
    {
        var session = notification.Session;
        
        await _hub.Clients.All.SendAsync(
            "NewSession",
            _mapper.Map<Dto.Rest.Out.Session>(session),
            cancellationToken: cancellationToken);
    }
}