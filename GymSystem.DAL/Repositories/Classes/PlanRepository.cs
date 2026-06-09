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
    public class PlanRepository(GymDbContext context) : IplanRepository
    {
        private readonly GymDbContext _context = context;

        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            _context.Plans.Add(plan);
            return await _context.SaveChangesAsync(ct);
        }

        public Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            _context.Plans.Remove(plan);
            return _context.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Plan> query = tracking ? _context.Plans : _context.Plans.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<Plan> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Plans.FindAsync(id);
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            _context.Plans.Update(plan);
            return await _context.SaveChangesAsync(ct);
        }
    }
}
