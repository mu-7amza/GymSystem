using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels.MemberViewModel;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }



        // Get BaseUrl/Members/Index
        // Index => List of Members
        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMemberAsync(tracking: false);
            return View(members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model)
        {
            if (!ModelState.IsValid) 
                return View(nameof(Create), model);

            var result = await _memberService.CreateMemberAsync(model);

            if (result)
            {
                TempData["Success"] = "Member Created Succcesfully";
            }
            else
            {
                TempData["Error"] = "Failed to Create Member";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MemberDetails(int id,CancellationToken ct)
        {
            // Get Member By Id
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if(member is null)
            {
                TempData["Error"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            // Get HealthRecord By MemberId
            var healthRecord = await _memberService.GetHealthRecordDetails(id, ct);
            if (healthRecord is null)
            {
                TempData["Error"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(healthRecord);
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id , CancellationToken ct)
        {
            // Get Member By Id
            var member = await _memberService.GetMemberToUpdate(id, ct);
            if (member is null)
            {
                TempData["Error"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
          if (!ModelState.IsValid) 
                return View(model);
           var result = await _memberService.UpdateMemberDetailsAsync(id, model, ct);

            if (result)
            {
                TempData["Success"] = "Member Updated Successfully";
            }
            else
            {
                TempData["Error"] = "Failed to update member";
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> DeleteMember(int id, CancellationToken ct)
        {
            // Get Member By Id
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null)
            {
                TempData["Error"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
           
            var result = await _memberService.DeleteMemberAsync(id, ct);

            if (result)
            {
                TempData["Success"] = "Member deleted Successfully";
            }
            else
            {
                TempData["Error"] = "Failed to delete member";
            }
            return RedirectToAction(nameof(Index));

        }
    }
}
