using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels.PlanViewModel;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace GymManagementSystem.Controllers
{
    [Authorize]
    public class PlansController : Controller
    {
        private readonly IPlanService _planService ;
        public PlansController(IPlanService planService)
        {
            _planService = planService;
        }

        // GET: AllPlans
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            ViewBag.ActivePage = "Plans";
            var plans = await _planService.GetAllPlansAsync(false,ct);
            return View(plans);
        }

        // Details
        // Get BaseUrl/Plans/Details/id
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanDetailsByIdAsync(id, ct);
            if (plan is null)
            {
                TempData["Error"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        // Get BaseUrl/Plans/EditPlan/id
        [HttpGet]
        public async Task<IActionResult> EditPlan(int id, CancellationToken ct)
        {
            var plan = await _planService.GetPlanToUpdate(id, ct);
            if (plan is null)
            {
                TempData["Error"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);

        }

        // Get BaseUrl/Plans/EditPlan/id
        [HttpPost]
        public async Task<IActionResult> EditPlan(int id,PlanToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _planService.UpdatePlanDetailsAsync(id, model, ct);

            if (result.success)
            {
                TempData["Success"] = "Plan Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = result.error;
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
