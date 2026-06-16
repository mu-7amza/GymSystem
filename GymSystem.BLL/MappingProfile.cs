using AutoMapper;
using GymSystem.BLL.ViewModels.MemberViewModel;
using GymSystem.BLL.ViewModels.PlanViewModel;
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

    }
}
