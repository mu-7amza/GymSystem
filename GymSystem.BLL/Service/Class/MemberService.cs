using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels.MemberViewModel;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Class
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepo;
        private readonly IGenericRepository<MemberShip> _memberShipRepo;
        private readonly IGenericRepository<Plan> _planRepo;


        public MemberService(IGenericRepository<Member> memberRepo, IGenericRepository<MemberShip> memberShipRepo, IGenericRepository<Plan> planRepo)
        {
            _memberRepo = memberRepo;
            _memberShipRepo = memberShipRepo;
            _planRepo = planRepo;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            // Check Email Exists
            var emailExist = await _memberRepo.AnyAsync(x => x.Email == model.Email, ct);

            // Check Phone Exists
            var phoneExist = await _memberRepo.AnyAsync(x => x.Phone == model.Phone, ct);

            if (emailExist || phoneExist) return false;

            var member = new Member
            {
                Name = model.Name,
                Email = model.Email,    
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Streat = model.Street
                },
                HealthRecord = new HealthRecord
                {
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }
            };

            var result =  await _memberRepo.AddAsync(member, ct);
            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(bool tracking, CancellationToken ct = default)
        {
            var members = await _memberRepo.GetAllAsync(tracking: tracking, ct: ct);

            if (!members.Any()) return Enumerable.Empty<MemberViewModel>();

            List<MemberViewModel> memberViewModels = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()
            }).ToList();

            return memberViewModels;
        }

        public async Task<MemberDetailsViewModel> GetMemberDetailsByIdAsync(int id, CancellationToken ct)
        {
            var member = await _memberRepo.GetByIdAsync(id, ct);
            if(member == null) return null;

            var memberDetails = new MemberDetailsViewModel
            {
                Name = member.Name,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} {member.Address.Streat} {member.Address.City}"
            };

            var activeMemberShip = await _memberShipRepo.FirstOrDefaultAsync(x => x.MemberId == member.Id && x.EndDate > DateTime.Now);

            if(activeMemberShip is not null)
            {
                var plan = await _planRepo.GetByIdAsync(activeMemberShip.PlanId, ct);

                memberDetails.PlanName = plan.Name;
                memberDetails.MembershipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
                memberDetails.MembershipEndDate = activeMemberShip.EndDate.ToShortDateString();
            }
            return memberDetails;
        }
    }
}
