using DBRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBRepository.StudentPortal.Interfaces
{
    public interface IStudentRepository
    {
        Task<Page<Student>> GetStudents(int index, int pageSize, int id, string filter, bool? isMale, string orderby,
            bool? desc);
        Task<Page<Group>> GetGroups(int index, int pageSize);
        Task<ICollection<Student>> GetStudents();
        Task<Student> AddStudent(int userId, Student student);
        Task<int> DeleteStudent(int studentId);
        Task<int> UpdateStudent(Student student);
        
        Task<Group> AddGroup(Group group);
        Task<int> DeleteGroup(int groupId);
        Task<int> UpdateGroup(Group group);
        Task<Student> GetStudentById(int studentId);
        Task<Group> GetGroupById(int groupId);
        Task<int> AddGroupToStudent(int[] groupIds, int studentId);
        Task<int> RemoveGroupFromStudent(int[] groupIds, int studentId);
        Task<Page<Group>> GetGroupsByStudentId(int index, int pageSize, int studentId);
        Task<ICollection<Group>> GetGroups();
    }
}
