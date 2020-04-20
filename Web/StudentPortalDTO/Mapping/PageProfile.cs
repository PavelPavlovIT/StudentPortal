using AutoMapper;
using DBRepository.Models;
using StudentPortalDTO.Models;
using StudentPortalDTO.ViewModels;

namespace StudentPortalDTO.Mapping
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap<Page<Student>, Page<StudentViewModel>>()
                .ForMember(dest =>
                        dest.CurrentPage,
                    opt => opt.MapFrom(src => src.CurrentPage))
                .ForMember(dest =>
                        dest.PageSize,
                    opt => opt.MapFrom(src => src.PageSize))
                .ForMember(dest =>
                        dest.TotalRecords,
                    opt => opt.MapFrom(src => src.TotalRecords))
                .ForMember(dest =>
                        dest.Records,
                    opt => opt.MapFrom(src => src.Records))
                .ReverseMap();
        }
    }
}
