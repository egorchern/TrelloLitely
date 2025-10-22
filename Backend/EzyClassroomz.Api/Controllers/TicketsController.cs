using EzyClassroomz.Library.Repositories.Boards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EzyClassroomz.Api.Extensions;
using EzyClassroomz.Library.Entities;
using Microsoft.AspNetCore.SignalR;
using EzyClassroomz.Api.Hubs;

namespace EzyClassroomz.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly IBoardsRepository _boardsRepository;
    private readonly IHubContext<TicketNotificationHub> _ticketNotificationHub;

    public TicketsController(IBoardsRepository boardsRepository, IHubContext<TicketNotificationHub> ticketNotificationHub)
    {
        _boardsRepository = boardsRepository;
        _ticketNotificationHub = ticketNotificationHub;
    }

    [HttpPost("{boardId}")]
    public async Task<IActionResult> CreateTicket([FromRoute] int boardId, [FromBody] Ticket ticket)
    {
        var board = await _boardsRepository.GetBoardById(boardId, false, true);

        if (board == null)
        {
            return NotFound();
        }

        // Tenant isolation
        var tenantIdFromClaim = User.GetTenantId();

        if (string.IsNullOrEmpty(tenantIdFromClaim) || board.TenantId != tenantIdFromClaim)
        {
            return Forbid();
        }

        board.Tickets.Add(ticket);
        await _boardsRepository.UpdateBoard(board);
        _ticketNotificationHub.Clients.Group(tenantIdFromClaim).SendAsync("Modified", $"A ticket was created with ID {ticket.Id} on board {board.Id}");

        return Ok();
    }
}
