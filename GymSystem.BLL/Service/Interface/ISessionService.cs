using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.SessionsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interface
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default);
        Task<SessionViewModel> GetSessionByIdAsync(int sessionId, CancellationToken ct = default);

        Task<Result> CreateSessionAsync(CreateSessionViewModel session , CancellationToken ct = default);

        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersDropDownListAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesDropDownListAsync(CancellationToken ct = default);

        Task<SessionDetailsViewModel> GetSessionDetailsAsync(int sessionId, CancellationToken ct = default);
        Task<SessionToUpdateViewModel> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default);

        Task<Result> UpdateSessionAsync( int id ,SessionToUpdateViewModel model, CancellationToken ct = default);
        Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);


    }
}
