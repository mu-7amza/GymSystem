using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Interface
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(bool tracking, CancellationToken ct = default);
        Task<TrainerDetailsViewModel> GetTrainerDetailsByIdAsync(int id, CancellationToken ct);
        Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);
        Task<TrainerToUpdateViewModel?> GetTrainerToUpdate(int id, CancellationToken ct = default);

        Task<Result> UpdateTrainerDetailsAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default);

        Task<Result> DeleteTrainerAsync(int id , CancellationToken ct = default);
    }
}
