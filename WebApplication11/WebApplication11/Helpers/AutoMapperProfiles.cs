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
            CreateMap<UserForUpdate, User>();
            CreateMap<Photo, PhotoForReturn>();
            CreateMap<PhotoForCreation, Photo>();
            CreateMap<UserForRegister, User>();
            CreateMap<MessageForCreation, Message>().ReverseMap();
            CreateMap<Message, MessageToReturn>()
                .ForMember(m=> m.SenderPhotoUrl, opt => opt
                .MapFrom(u=> u.Sender.Photos.FirstOrDefault(p=> p.IsMain).Url))
                  .ForMember(m => m.RecipientPhotoUrl, opt => opt
                 .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));

        }
    }
}
