using DBRepository.Models;
using System.Linq;

namespace DBRepository.StudentPortal.Implementations.Helpers
{
    public static class SortingStudentByFields
    {
        public static IQueryable<T> OrderByFields<T>(
            this IQueryable<Student> query,
            string orderBy,
            bool desc)
        {
            if (desc)
            {
                switch (orderBy)
                {
                    case "FirstName":
                        query = query.OrderByDescending(p => p.FirstName);
                        break;
                    case "LastName":
                        query = query.OrderByDescending(p => p.LastName);
                        break;
                    case "Patronymic":
                        query = query.OrderByDescending(p => p.Patronymic);
                        break;
                    case "Nick":
                        query = query.OrderByDescending(p => p.Nick);
                        break;
                    case "isMale":
                        query = query.OrderByDescending(p => p.isMale);
                        break;
                };
            }
            else
            {
                switch (orderBy)
                {
                    case "FirstName":
                        query = query.OrderBy(p => p.FirstName);
                        break;
                    case "LastName":
                        query = query.OrderBy(p => p.LastName);
                        break;
                    case "Patronymic":
                        query = query.OrderBy(p => p.Patronymic);
                        break;
                    case "Nick":
                        query = query.OrderBy(p => p.Nick);
                        break;
                    case "isMale":
                        query = query.OrderBy(p => p.isMale);
                        break;
                };
            }
            return (IQueryable<T>) query;
        }
    }
}
