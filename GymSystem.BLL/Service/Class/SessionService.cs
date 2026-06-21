using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Service.Interface;
using GymSystem.BLL.ViewModels.SessionsViewModel;
using GymSystem.DAL.Data.Models;
using GymSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Service.Class
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel sessionModel, CancellationToken ct)
        {
            if (sessionModel.StartDate > sessionModel.EndDate) return Result.ValidationFailed("End date must be after than start date");
            if (sessionModel.StartDate < DateTime.Now) return Result.ValidationFailed("Start date must be in the future");
            if (sessionModel.Capacity < 1 || sessionModel.Capacity > 25) return Result.ValidationFailed("Capacity must be between 1 and 25");
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(sessionModel.TrainerId, ct);
            if (trainer == null) return Result.NotFound("Trainer not found");
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(sessionModel.CategoryId, ct);
            if (category == null) return Result.NotFound("Category not found");

            var session = _mapper.Map<Session>(sessionModel);
            _unitOfWork.GetRepository<Session>().AddAsync(session, ct);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("failed to create session");
        } 

                

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct)
        {
            var Sessions = await _unitOfWork._sessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct: ct);
            if (!Sessions.Any() || Sessions == null)
                return null;

            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(Sessions);
            foreach (var session in mappedSessions)
            {

                session.AvailableSlots = session.Capacity - await _unitOfWork._sessionRepository.GetAvailableSlotsCountAsync(session.Id, ct: ct);
            }
            return mappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesDropDownListAsync(CancellationToken ct = default)
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(tracking: false, ct: ct);
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(categories);
        }




        public async Task<SessionToUpdateViewModel> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork._sessionRepository.GetSessionWithTrainerAndCategoryAsync(sessionId, ct: ct);
            if (session is null) return null;

            var mappedSession = _mapper.Map<Session, SessionToUpdateViewModel>(session);
            return mappedSession;
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersDropDownListAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(tracking: false, ct: ct);
            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<Result> UpdateSessionAsync(int id, SessionToUpdateViewModel model, CancellationToken ct)
        {
            var session = await _unitOfWork._sessionRepository.GetSessionWithTrainerAndCategoryAsync(id, ct: ct);
            if (session is null) return Result.NotFound("Session not Found");

            if (model.StartDate > model.EndDate) return Result.ValidationFailed("End date must be after than start date"); ;
            if (model.StartDate < DateTime.Now) return Result.ValidationFailed("Start date must be in the future"); 

            // Validate trainer exists
            var trainerExists = await _unitOfWork.GetRepository<Trainer>().AnyAsync(t => t.Id == model.TrainerId, ct);
            if (!trainerExists) return Result.NotFound("Trainer not found");

            session.Description = model.Description;
            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;
            session.TrainerId = model.TrainerId;

            _unitOfWork.GetRepository<Session>().UpdateAsync(session, ct);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to update session") ;
        }

        public async Task<SessionDetailsViewModel> GetSessionDetailsAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork._sessionRepository.GetSessionWithTrainerAndCategoryAsync(sessionId, ct: ct);
            if (session is null) return null;

            var mappedSession = _mapper.Map<Session, SessionDetailsViewModel>(session);

            var bookedCount = await _unitOfWork._sessionRepository.GetAvailableSlotsCountAsync(sessionId, ct: ct);
            mappedSession.AvailableSlots = mappedSession.Capacity - bookedCount;

            return mappedSession;
        }

        public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork._sessionRepository.GetSessionWithTrainerAndCategoryAsync(id, ct);
            if(session is null) return Result.NotFound("Session not found");

            if(session.EndDate > DateTime.Now ) return Result.Fail("Can't delete session whose end date in the future");

            var bookings = await _unitOfWork._sessionRepository.GetAvailableSlotsCountAsync(session.Id, ct);
            if(bookings >0 ) return Result.Fail("Can't delete session that has bookings");

            _unitOfWork.GetRepository<Session>().DeleteAsync(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.OK() : Result.Fail("Failed to delete session");

        }

        public async Task<SessionViewModel> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct: ct);
            if (session is null) return null;

            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);
            return mappedSession;
        }
    }
}
