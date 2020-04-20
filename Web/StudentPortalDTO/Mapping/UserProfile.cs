using AutoMapper;
using DBRepository.Models;
using StudentPortalDTO.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentPortalDTO.Mapping
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<User, AuthenticateViewModel>()
                .ForMember(dest =>
                        dest.UserId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                        dest.Login,
                    opt => opt.MapFrom(src => src.Login))
                .ReverseMap();
        }
    }
}
