using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Class
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            // Check Email Exists
            var emailExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email, ct);

            // Check Phone Exists
            var phoneExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone, ct);

            if (emailExist || phoneExist) return Result.ValidationFailed("Email or phone is redundant !");

            var trainer = _mapper.Map<Trainer>(model);

             _unitOfWork.GetRepository<Trainer>().AddAsync(trainer, ct);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to create trainer") ;
        }

        public async Task<Result> DeleteTrainerAsync(int id , CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
            if (trainer == null) return Result.NotFound("Trainer not found");
            _unitOfWork.GetRepository<Trainer>().DeleteAsync(trainer, ct);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to delete trainer") ;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(bool tracking, CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(tracking, ct);
            if (!trainers.Any()) return [];
            return _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
        }

        public async Task<TrainerDetailsViewModel> GetTrainerDetailsByIdAsync(int id, CancellationToken ct)
        {
            var trainer = await  _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
            if (trainer == null) return null;
            var trainerDetails = _mapper.Map<TrainerDetailsViewModel>(trainer);
            return trainerDetails;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdate(int id, CancellationToken ct )
        {
            var trainer = await  _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
            if (trainer == null) return null;
            var trainerToUpdate = _mapper.Map<TrainerToUpdateViewModel>(trainer);
            return trainerToUpdate;
        }

        public async Task<Result> UpdateTrainerDetailsAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);

            if (trainer == null) return Result.NotFound("Trainer not found");

            // Check Email Exists
            var emailExist = await  _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email && x.Id != id, ct);

            // Check Phone Exists
            var phoneExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone && x.Id != id, ct);

            if (emailExist || phoneExist) return Result.ValidationFailed("Email or phone is redundant !");

            var trainerUpdate =  _mapper.Map(model, trainer);

            _unitOfWork.GetRepository<Trainer>().UpdateAsync(trainerUpdate, ct);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to update trainer") ;
        }
    }
}
