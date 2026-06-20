using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels.NewFolder;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;

namespace GymSystem.BLL.Service.Class
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsViewModel> GetAnalyticsDataAync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.GetRepository<Session>().GetAllAsync(ct: ct);

            var totalmembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct:ct);

            var totalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct: ct);

            var activeMembers = await _unitOfWork.GetRepository<MemberShip>().CountAsync( m => m.EndDate > DateTime.Now ,ct);

            return new AnalyticsViewModel
            {
                TotalMembers = totalmembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = activeMembers,
                UpcomingSessions = sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now),
                CompletedSessions = sessions.Count(s => s.EndDate < DateTime.Now)
            };

            
        }
    }
}
