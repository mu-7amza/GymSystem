using GymSystem.BLL.ViewModels;
using GymSystem.BLL.ViewModels.MemberViewModel;
using GymSystem.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interface
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(bool tracking ,CancellationToken ct = default);
        Task<MemberDetailsViewModel> GetMemberDetailsByIdAsync(int id, CancellationToken ct);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetHealthRecordDetails(int id, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdate(int id, CancellationToken ct = default);

        Task<bool> UpdateMemberDetailsAsync(int id ,MemberToUpdateViewModel model , CancellationToken ct = default);

        Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default);

    }
}
