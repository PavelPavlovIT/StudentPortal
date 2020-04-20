using AutoMapper;
using DBRepository.Models;
using StudentPortalDTO.ViewModels;

namespace StudentPortalDTO.Mapping
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupViewModel>()
                .ForMember(dest =>
                        dest.Name,
                    opt => opt.MapFrom(src => src.GroupName))
                .ReverseMap();

            CreateMap<GroupStudents, GroupViewModel>()
                .ForMember(dest =>
                        dest.Name,
                    opt => opt.MapFrom(src => src.Group.GroupName))
                .ForMember(dest =>
                        dest.Id,
                    //opt => opt.MapFrom(src => src.Group.Id))
                    opt => opt.MapFrom(src => src.GroupId))
                .ReverseMap();
        }
    }
}
