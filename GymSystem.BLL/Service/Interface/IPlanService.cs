using GymSystem.BLL.ViewModels.MemberViewModel;
using GymSystem.BLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interface
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(bool tracking, CancellationToken ct = default);
        Task<PlanViewModel> GetPlanDetailsByIdAsync(int id, CancellationToken ct);
        Task<PlanToUpdateViewModel?> GetPlanToUpdate(int id, CancellationToken ct = default);
        Task<bool> UpdatePlanDetailsAsync(int id, PlanToUpdateViewModel model, CancellationToken ct = default);


    }
}
