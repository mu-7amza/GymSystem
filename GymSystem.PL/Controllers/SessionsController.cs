using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels.SessionsViewModel;
using GymSystem.DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymSystem.PL.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // GetAll Sessions  BaseUrl/Sessions
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            ViewBag.ActivePage = "Sessions";
            var sessions = await _sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropDownList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownList();
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model, ct);

            if (result.success)
            {
                TempData["Success"] = "Session Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = result.error;
                await PopulateDropDownList();

                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Details(int id,CancellationToken ct)
        {
            var session = await _sessionService.GetSessionDetailsAsync(id,ct);
            if (session == null)
            {

                TempData["Error"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if (session == null)
            {

                TempData["Error"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownList();
            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id ,SessionToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownList();
                return View(model);
            }

            var result = await _sessionService.UpdateSessionAsync(id,model, ct);

            if (result.success) 
            {
                TempData["Success"] = "Session Updated Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = result.error;
                await PopulateDropDownList();

                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionByIdAsync(id, ct);
            if (session == null)
            {

                TempData["Error"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await _sessionService.DeleteSessionAsync(id, ct);

            if (result.success)
            {
                TempData["Success"] = "Session Deleted Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = result.error;

                return RedirectToAction(nameof(Index));
            }
        }


        private async Task PopulateDropDownList()
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersDropDownListAsync(),"Id","Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesDropDownListAsync(),"Id","CategoryName");
        }
    }
}
