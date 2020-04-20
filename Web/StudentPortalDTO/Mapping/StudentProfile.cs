using AutoMapper;
using DBRepository.Models;
using StudentPortalDTO.ViewModels;

namespace StudentPortalDTO.Mapping
{
    public class StudentProfile: Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentViewModel>()
                .ForMember(dest =>
                        dest.FirstName,
                    opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest =>
                        dest.LastName,
                    opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest =>
                        dest.Patronymic,
                    opt => opt.MapFrom(src => src.Patronymic))
                .ForMember(dest =>
                        dest.Nick,
                    opt => opt.MapFrom(src => src.Nick))

                .ForMember(dest =>
                        dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                        dest.isMale,
                    opt => opt.MapFrom(src => src.isMale))
                .ForMember(dest =>
                        dest.Groups,
                    opt => opt.MapFrom(src => src.GroupStudents))
                .ReverseMap();
        }
    }
}
