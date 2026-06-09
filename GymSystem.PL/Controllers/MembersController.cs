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
            if (!ModelState.IsValid) return View(nameof(Create), model);     

            var result = await _memberService.CreateMemberAsync(model);
            return RedirectToAction(nameof(Index));
            
           
        }
    }
}
