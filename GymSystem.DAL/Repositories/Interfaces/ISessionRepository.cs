using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct = default);
        Task<Session?> GetSessionWithTrainerAndCategoryAsync(int seassionId,CancellationToken ct = default);

        Task<int> GetAvailableSlotsCountAsync(int sessionId, CancellationToken ct);
    }
}
