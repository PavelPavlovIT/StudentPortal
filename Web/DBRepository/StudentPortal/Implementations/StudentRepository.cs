using DBRepository.StudentPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using DBRepository.Models;
using DBRepository.StudentPortal.Implementations.Helpers;

namespace DBRepository.StudentPortal.Implementations
{
    public class StudentRepository : BaseRepository, IStudentRepository
    {
        public StudentRepository(string connectionString, IRepositoryContextFactory contextFactory)
            : base(connectionString, contextFactory) { }
        public async Task<Group> AddGroup(Group group)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Groups.Add(group);
                await context.SaveChangesAsync();
                return group;
            }
        }
        public async Task<int> DeleteGroup(int groupId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var group = new Group() { Id = groupId };
                context.Groups.Remove(group);
                return await context.SaveChangesAsync();
            }
        }
        public async Task<int> UpdateGroup(Group group)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Groups.Update(group);
                return await context.SaveChangesAsync();
            }
        }
        public async Task<Group> GetGroupById(int groupId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var query = context.Groups.AsQueryable();
                query = query.Where(p => p.Id == groupId);
                return await query.FirstOrDefaultAsync();
            }
        }
        public async Task<Student> AddStudent(int userId, Student student)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                student.UserStudents = new List<UserStudents>
                {
                    new UserStudents{UserId = userId, Student =  student}
                };
                context.Students.Add(student);
                await context.SaveChangesAsync();
                return student;
            }
        }
        public async Task<int> DeleteStudent(int studentId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var student = new Student() { Id = studentId };
                context.Students.Remove(student);
                return await context.SaveChangesAsync();
            }
        }
        public async Task<ICollection<Student>> GetStudents()
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var query = context.Students.Include(p => p.GroupStudents)
                    .ThenInclude(group => group.Group)
                    .AsQueryable();
                return await query.ToListAsync();
            }
        }

        public async Task<ICollection<Group>> GetGroups()
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var query = context.Groups.Include(p => p.GroupStudents).AsQueryable();
                List<Group> list = await query.ToListAsync();
                return list;
            }
        }

        public async Task<Page<Student>> GetStudents(int index, int pageSize, int id, string filter, bool? isMale, string orderby, bool? desc)
        {
            var result = new Page<Student>() { CurrentPage = index, PageSize = pageSize };
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var query = context.Students.Include(p => p.GroupStudents)
                    .ThenInclude(group => group.Group)
                    .AsQueryable();
                //check on my stidents
                if (id != 0)
                {
                    query = query.Where(p => p.UserStudents.Any(us => us.UserId == id));
                }
                if (!string.IsNullOrEmpty(filter))
                {
                    query = query.Where(p => (
                            p.FirstName + " " +
                            p.LastName + " " +
                            (p.Patronymic == null ? " " : p.Patronymic) + " " +
                            (p.Nick == null ? " " : p.Nick)).Contains(filter));
                    //                            p.Nick == null ? false : p.Nick.Contains(filter)));

                    //query = query.Where(p => (
                    //        p.FirstName.Contains(filter) ||
                    //        p.LastName.Contains(filter) ||
                    //        p.Patronymic.Contains(filter) ||
                    //        p.Nick == null ? false : p.Nick.Contains(filter)));
                }
                if (isMale.HasValue)
                {
                    query = query.Where(p => p.isMale == isMale);
                }
                if (desc.HasValue)
                {

                    query = query.OrderByFields<Student>(orderby, desc.Value);
                }
                result.TotalRecords = await query.CountAsync();
                result.Records = await query.Skip(index * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }
            return result;
        }
        public async Task<Page<Group>> GetGroups(int index, int pageSize)
        {
            var result = new Page<Group>() { CurrentPage = index, PageSize = pageSize };
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var query = context.Groups.AsQueryable();
                result.TotalRecords = await query.CountAsync();
                result.Records = await query.Skip(index * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }
            return result;
        }
        public async Task<int> UpdateStudent(Student student)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                student.GroupStudents = null;
                context.Students.Update(student);
                return await context.SaveChangesAsync();
            }
        }
        public async Task<Student> GetStudentById(int studentId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var query = context.Students.Include(p => p.GroupStudents).ThenInclude(group => group.Group).AsQueryable();
                query = query.Where(p => p.Id == studentId);
                return await query.FirstOrDefaultAsync();
            }
        }
        public async Task<int> AddGroupToStudent(int[] groupIds, int studentId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var list = await context.GroupStudents.AsQueryable().Where(u => u.StudentId == studentId).ToListAsync();
                context.GroupStudents.RemoveRange(list);
                //await context.SaveChangesAsync();
                if (groupIds != null)
                    foreach (var groupId in groupIds)
                    {
                        context.Set<GroupStudents>().Add(new GroupStudents { StudentId = studentId, GroupId = groupId });
                    }

                return await context.SaveChangesAsync();
            }
        }
        public async Task<int> RemoveGroupFromStudent(int[] groupIds, int studentId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                foreach (var groupId in groupIds)
                {
                    var item = context.GroupStudents.AsNoTracking().FirstOrDefault(q => q.GroupId == groupId && q.StudentId == studentId);
                    context.Set<GroupStudents>().Remove(new GroupStudents { Id = item.Id });
                }
                return await context.SaveChangesAsync();
            }
        }
        public async Task<Page<Group>> GetGroupsByStudentId(int index, int pageSize, int studentId)
        {
            var result = new Page<Group>() { CurrentPage = index, PageSize = pageSize };
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var query = context.Groups.Include(p => p.GroupStudents).ThenInclude(p => p.Group).AsQueryable();
                query = query.Where(p => p.GroupStudents.Any(q => q.StudentId == studentId));
                result.TotalRecords = await query.CountAsync();
                result.Records = await query.Skip(index * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }
            return result;
        }

    }
}
