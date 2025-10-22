using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzyClassroomz.Library.Data;
using EzyClassroomz.Library.Entities;
using Microsoft.EntityFrameworkCore;

namespace EzyClassroomz.Library.Repositories.Boards
{
    public class BoardsRepository : IBoardsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BoardsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Board?> GetBoardById(long boardId, bool readOnly = false, bool includeTickets = false)
        {
            IQueryable<Board> query = _dbContext.Boards;

            if (readOnly)
            {
                query = query.AsNoTracking();
            }

            if (includeTickets)
            {
                query = query.Include(b => b.Tickets);
            }

            return await query.Where(b => b.Id == boardId).FirstOrDefaultAsync();
        }
    }
}
