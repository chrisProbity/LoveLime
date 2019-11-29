using AutoMapper;
using DatingApp.Core.DTOs;
using DatingApp.Core.Models;
using System.Linq;


namespace DatingApp.API.Helpers
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserListDTO>().ForMember(x => x.PhotoUrl, opt => {
                opt.MapFrom(p => p.Photos.FirstOrDefault(n => n.IsMain).Url);
            }).ForMember(dest => dest.Age, opt => {
                opt.ResolveUsing(x => x.DateOfBirth.CalculateAge());
            });
            CreateMap<User, UserDetailDTO>().ForMember(x =>x.PhotoUrl, opt => { opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            }).ForMember(dest =>dest.Age, opt => {
                opt.ResolveUsing(x => x.DateOfBirth.CalculateAge());
            });
            CreateMap<Photo, PhotoDTO>();
            CreateMap<ProfileUpdateDTO, User>();
            CreateMap<PhotoUploadDTO, Photo>();
            CreateMap<Photo, PhotoForReturnDTO>();
        }
       
    }
}
