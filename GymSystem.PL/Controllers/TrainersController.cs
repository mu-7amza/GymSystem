using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymSystem.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public async Task <IActionResult> Index(CancellationToken ct)
        {
            ViewBag.ActivePage = "Trainers";
            var trainers = await _trainerService.GetAllTrainersAsync(tracking:false,ct:ct);
            return View(trainers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _trainerService.CreateTrainerAsync(model, ct);
            if (result.success)
            {
                TempData["Success"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = result.error;
                return View(model);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id ,CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerDetailsByIdAsync(id, ct);

            if (trainer is null)
            {
                TempData["Error"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerToUpdate(id, ct);

            if (trainer is null)
            {
                TempData["Error"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id ,TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _trainerService.UpdateTrainerDetailsAsync(id,model, ct);
            if (result.success)
            {
                TempData["Success"] = "Trainer Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerDetailsByIdAsync(id, ct);

            if (trainer is null)
            {
                TempData["Error"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
         
            var result = await _trainerService.DeleteTrainerAsync(id, ct);
            if (result.success)
            {
                TempData["Success"] = "Trainer Deleted Successfully";
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
