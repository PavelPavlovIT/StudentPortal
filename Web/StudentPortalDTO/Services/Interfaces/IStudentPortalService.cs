using DBRepository.Models;
using StudentPortalDTO.Models;
using StudentPortalDTO.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentPortalDTO.Services.Interfaces
{
    public interface IStudentPortalService
    {
        Task<StudentViewModel> GetStudentById(int studentId);
        Task<GroupViewModel> GetGroupById(int groupId);
        Task<ICollection<StudentViewModel>> GetAllStudents();
        Task<Page<StudentViewModel>> GetStudents(int index, int id, string filter, bool? isMale, string orderby, bool? desc);
        Task<ICollection<GroupViewModel>> GetAllGroups();
        Task<Page<GroupViewModel>> GetGroups(int index);
        Task<StudentViewModel> AddStudent(int userId, StudentViewModel student);
        Task<int> DeleteStudent(int studentId);
        Task<int> UpdateStudent(StudentViewModel student);
        Task<GroupViewModel> AddGroup(GroupViewModel request);
        Task<int> DeleteGroup(int groupId);
        Task<int> UpdateGroup(GroupViewModel request);
        Task<int> RemoveGroupFromStudent(int[] groupIds, int studentId);
        Task<int> AddGroupToStudent(int[] groupIds, int studentId);
        Task<Page<GroupViewModel>> GetGroupsByStudentId(int index, int studentId);
    }
}
