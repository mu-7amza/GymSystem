using AutoMapper;
using GymSystem.BLL.ViewModels.MemberViewModel;
using GymSystem.BLL.ViewModels.PlanViewModel;
using GymSystem.BLL.ViewModels.SessionsViewModel;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            TrainerMap();
            MemberMap();
            PlansMap();
            SessionMap();
        }

        private void TrainerMap()
        {
            CreateMap<Trainer, TrainerViewModel>()
               .ForMember(dest => dest.Specilaities, opt => opt.MapFrom(src => src.Specilaities.ToString()));

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<Trainer, TrainerDetailsViewModel>()
                .ForMember(dest => dest.Specilaities, opt => opt.MapFrom(src => src.Specilaities.ToString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("dd-MM-yyyy")));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber)).ReverseMap();
        }
        private void MemberMap()
        {
            CreateMap<Member, MemberViewModel>();

            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.HealthRecord,opt => opt.MapFrom(src => new HealthRecord
                {
                    BloodType = src.HealthRecordViewModel.BloodType,
                    Weight = src.HealthRecordViewModel.Weight,
                    Height = src.HealthRecordViewModel.Height,
                    Note = src.HealthRecordViewModel.Note
                }))
                .ForMember(dest => dest.Address,opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }));

            CreateMap<Member, MemberDetailsViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("dd-MM-yyyy")))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));

            CreateMap<HealthRecord, HealthRecordViewModel>();

            CreateMap<Member,MemberToUpdateViewModel>()
                .ForMember(dest => dest.Street,opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City,opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.BuildingNumber,opt => opt.MapFrom(src => src.Address.BuildingNumber));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    City = src.City,
                    Street = src.Street
                }))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));




        }
        private void PlansMap()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, PlanToUpdateViewModel>();
            CreateMap<PlanToUpdateViewModel, Plan>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        }
        private void SessionMap()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName,opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.CategoryName , opt => opt.MapFrom(src => src.Category.CategoryName)).ReverseMap();
            CreateMap<Session, CreateSessionViewModel>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, SessionDetailsViewModel>()
                 .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName)).ReverseMap();
            CreateMap<Session, SessionToUpdateViewModel>()
                   .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                   .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                   .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                   .ForMember(dest => dest.TrainerId, opt => opt.MapFrom(src => src.TrainerId));

            CreateMap<SessionToUpdateViewModel, Session>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.TrainerId, opt => opt.MapFrom(src => src.TrainerId))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Capacity, opt => opt.UseDestinationValue())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Trainer, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore());

        }

    }
}
