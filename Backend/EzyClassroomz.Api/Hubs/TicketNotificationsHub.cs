using EzyClassroomz.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EzyClassroomz.Api.Hubs;

[Authorize]
public class TicketNotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        // Add the client to a group
        var tenantId = Context!.User.GetTenantId();
        await Groups.AddToGroupAsync(Context.ConnectionId, tenantId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        var tenantId = Context!.User.GetTenantId();
        // Remove the client from a group
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, tenantId);
        await base.OnDisconnectedAsync(ex);
    }
}