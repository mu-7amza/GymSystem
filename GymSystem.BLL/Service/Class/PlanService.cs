using AutoMapper;
using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels.PlanViewModel;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Class
{
    
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(bool tracking, CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(false, ct);
            if (!plans.Any()) return [];

            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);

        }

        public async Task<PlanViewModel> GetPlanDetailsByIdAsync(int id, CancellationToken ct)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan == null) return null;
            return _mapper.Map<PlanViewModel>(plan);
        }

        public async Task<PlanToUpdateViewModel?> GetPlanToUpdate(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if(plan == null) return null;
            return _mapper.Map<PlanToUpdateViewModel>(plan);
        }

        public async Task<bool> UpdatePlanDetailsAsync(int id, PlanToUpdateViewModel model, CancellationToken ct = default)
        {
            var plan = await  _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);

            if(plan == null) return false;

            var planMemberShip = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(x => x.PlanId == id);

            if(planMemberShip is not null) return false;

           var planUpdate = _mapper.Map(model, plan);

            _unitOfWork.GetRepository<Plan>().UpdateAsync(planUpdate);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }
    }
}
