using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymSystem.BLL.ViewModels.NewFolder;

namespace GymSystem.BLL.Service.Interface
{
    public interface IAnalyticsService
    {
        Task<AnalyticsViewModel> GetAnalyticsDataAync(CancellationToken ct = default);
        

    }
}
