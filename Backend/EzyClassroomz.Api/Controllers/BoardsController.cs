using EzyClassroomz.Library.Repositories.Boards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EzyClassroomz.Api.Extensions;

namespace EzyClassroomz.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BoardsController : ControllerBase
{
    private readonly IBoardsRepository _boardsRepository;
    public BoardsController(IBoardsRepository boardsRepository)
    {
        _boardsRepository = boardsRepository;
    }
    
    [HttpGet("{boardId}")]
    [Authorize(policy: "readBoard")]
    [Authorize(policy: "readTicket")]
    public async Task<IActionResult> GetBoard([FromRoute] int boardId)
    {
        var board = await _boardsRepository.GetBoardById(boardId, true, true);

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

        return Ok(board);
    }
}
