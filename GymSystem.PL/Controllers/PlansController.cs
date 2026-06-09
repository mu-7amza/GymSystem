using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> _planRepository;

        public PlansController(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        // GET: AllPlans
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(tracking:false ,ct: ct);
            return View(plans);
        }

        // Details
        // Get BaseUrl/Plans/Details/1
        public async Task<IActionResult> Details(int id, CancellationToken ct) 
        {
            var plan = await _planRepository.GetByIdAsync(id: id, ct: ct);
            if (plan is null)
                return RedirectToAction(nameof(Index));
            return View(plan);
        }
    }
}
