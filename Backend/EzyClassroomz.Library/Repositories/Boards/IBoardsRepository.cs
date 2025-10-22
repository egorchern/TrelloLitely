using EzyClassroomz.Library.Entities;

namespace EzyClassroomz.Library.Repositories.Boards;

public interface IBoardsRepository
{
    public Task<Board?> GetBoardById(long boardId, bool readOnly = false, bool includeTickets = false);
}