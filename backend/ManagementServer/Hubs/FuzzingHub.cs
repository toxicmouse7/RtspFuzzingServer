using Microsoft.AspNetCore.SignalR;

namespace ManagementServer.Hubs;

public class FuzzingHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("Ready");
    }
}