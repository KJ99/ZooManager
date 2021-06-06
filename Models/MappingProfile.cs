using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZooManager.Models.DTO.Requests;
using ZooManager.Models.DTO.Responses;
using ZooManager.Models.Entities;

//Zasady czystego kodu

namespace ZooManager.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Animal, AnimalPartialResponse>();
            CreateMap<Species, SpeciesPartialResponse>();
            CreateMap<Area, AreaPartialResponse>();
            CreateMap<Species, SpeciesResponse>();
            CreateMap<Area, AreaResponse>();
            CreateMap<Animal, AnimalResponse>();
            CreateMap<SpeciesRequest, Species>();
            CreateMap<AreaRequest, Area>();
            CreateMap<AreaUpdateRequest, Area>();
            CreateMap<AnimalRequest, Animal>();

            CreateMap<UserRequest, IdentityUser>()
                .ForMember(
                    dest => dest.UserName,
                    action => action.MapFrom(src => src.Username)
                );

            CreateMap<IdentityUser, UserResponse>()
                .ForMember(
                    dest => dest.Username,
                    action => action.MapFrom(src => src.UserName)
                );
            
        }
    }
}
