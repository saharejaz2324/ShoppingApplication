using AutoMapper;
using ShoppingApplication.API.Controllers;
using ShoppingApplication.API.DTO;
using ShoppingApplication.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApplication.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForLists>()
                .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src =>
                src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                src.DateofBirth.CalculateAge()));
            CreateMap<User, UserForDeatiled>()
                .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src =>
                src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                src.DateofBirth.CalculateAge()));
            CreateMap<Photo, PhotoDetaild>();
        }
    }
}
