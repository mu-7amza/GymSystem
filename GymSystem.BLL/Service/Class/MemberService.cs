using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels;
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
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

     

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            // Check Email Exists
            var emailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email, ct);

            // Check Phone Exists
            var phoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone, ct);

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

             _unitOfWork.GetRepository<Member>().AddAsync(member, ct);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id,ct);
            if (member == null) return false;

            var hasFuturebooking = await _unitOfWork.GetRepository<Booking>().AnyAsync(x => x.Id == member.Id && x.Session.StartDate > DateTime.Now,ct);

            if (hasFuturebooking) return false;

             _unitOfWork.GetRepository<Member>().DeleteAsync(member, ct);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;


        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMemberAsync(bool tracking, CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(tracking: tracking, ct: ct);

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

        public async Task<HealthRecordViewModel?> GetHealthRecordDetails(int MemberId, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x => x.Id == MemberId,ct:ct);
            if (record == null) return null;

            return new HealthRecordViewModel
            {
                Height = record.Height,
                Weight = record.Weight,
                BloodType = record.BloodType,
                Note = record.Note
            } ;
        }

        public async Task<MemberDetailsViewModel> GetMemberDetailsByIdAsync(int id, CancellationToken ct)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if(member == null) return null;

            var memberDetails = new MemberDetailsViewModel
            {
                Name = member.Name,
                Phone = member.Phone,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} {member.Address.Streat} {member.Address.City}"
            };

            var activeMemberShip = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(x => x.MemberId == member.Id && x.EndDate > DateTime.Now);

            if(activeMemberShip is not null)
            {
                var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMemberShip.PlanId, ct);

                memberDetails.PlanName = plan.Name;
                memberDetails.MembershipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
                memberDetails.MembershipEndDate = activeMemberShip.EndDate.ToShortDateString();
            }
            return memberDetails;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdate(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member == null) return null;
            else
                return new MemberToUpdateViewModel
                {
                    Name = member.Name,
                    Phone = member.Phone,
                    Email = member.Email,
                    BuildingNumber = member.Address.BuildingNumber,
                    City = member.Address.City,
                    Street = member.Address.Streat,
                    Photo = member.Photo
                };
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);

            if (member == null) return false;

            // Check Email Exists
            var emailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == member.Email && x.Id != id, ct);

            // Check Phone Exists
            var phoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == member.Phone && x.Id != id, ct);

            if (emailExist || phoneExist) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.Streat = model.Street;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Member>().UpdateAsync(member, ct);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
            
        }
    }
}
