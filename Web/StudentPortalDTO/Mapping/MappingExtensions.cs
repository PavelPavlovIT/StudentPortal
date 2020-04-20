using System.Collections.Generic;
using AutoMapper;
using DBRepository.Models;
using StudentPortalDTO.Models;

namespace StudentPortalDTO.Mapping
{
    public static class MappingExtensions
    {
        public static ICollection<TDestination> ToMapCollection<TDestination, TSource>(this IMapper mapper, ICollection<TSource> source)
        {
            List<TDestination> dest = new List<TDestination>();
            foreach (var item in source)
            {
                dest.Add(mapper.Map<TDestination>(item));
            }
            return dest;
        }

        public static Page<TDestination> MapPage<TDestination, TSource>(this IMapper mapper, Page<TSource> source)
        {
            Page<TDestination> pageDest= new Page<TDestination>();
            pageDest.CurrentPage = source.CurrentPage;
            pageDest.PageSize= source.PageSize;
            pageDest.TotalRecords = source.TotalRecords;
            List<TDestination> dest = new List<TDestination>();
            foreach (var item in source.Records)
            {
                dest.Add(mapper.Map<TDestination>(item));
            }
            pageDest.Records = dest;
            return pageDest;

        }
    }
}
