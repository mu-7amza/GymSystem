using GymSystem.DAL.Data.DbContexts;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;
        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default)
        {
            var query = _dbContext.Sessions.AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);
            return await query.ToListAsync();
        }

        public async Task<int> GetAvailableSlotsCountAsync(int sessionId, CancellationToken ct)
        {
            return await _dbContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId,ct);
        }

        public async Task<Session?> GetSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _dbContext.Sessions.Include(s => s.Trainer).Include(s => s.Category)
                .Where(s => s.Id == sessionId).FirstOrDefaultAsync();
            return session;
        }
    }
}
