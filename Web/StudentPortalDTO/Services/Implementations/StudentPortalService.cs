using AutoMapper;
using DBRepository.Models;
using DBRepository.StudentPortal.Interfaces;
using Microsoft.Extensions.Configuration;
using StudentPortalDTO.Mapping;
using StudentPortalDTO.Services.Interfaces;
using StudentPortalDTO.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentPortalDTO.Services.Implementations
{
    public class StudentPortalService : IStudentPortalService
    {
        private readonly IStudentRepository _repository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public StudentPortalService(IStudentRepository repository, IConfiguration configuration, IMapper mapper)
        {
            _repository = repository;
            _config = configuration;
            _mapper = mapper;
        }
        public async Task<StudentViewModel> AddStudent(int userId, StudentViewModel student)
        {
            var _student = await _repository.AddStudent(userId, _mapper.Map<Student>(student));
            return _mapper.Map<StudentViewModel>(_student);
        }

        public async Task<GroupViewModel> AddGroup(GroupViewModel request)
        {
            var group = _mapper.Map<Group>(request);
            var _group = await _repository.AddGroup(group);
            return _mapper.Map<GroupViewModel>(_group);
        }

        public async Task<int> DeleteStudent(int studentId)
        {
            return await _repository.DeleteStudent(studentId);
        }

        public async Task<ICollection<StudentViewModel>> GetAllStudents()
        {
            var list = await _repository.GetStudents();
            return _mapper.ToMapCollection<StudentViewModel, Student>(list);
        }

        public async Task<Page<StudentViewModel>> GetStudents(int index, int id, string filter, bool? isMale, string orderby,
            bool? desc)
        {
            var pageSize = _config.GetValue<int>("pageSize");
            var list = await _repository.GetStudents(index, pageSize, id, filter, isMale, orderby, desc);
            return _mapper.MapPage<StudentViewModel, Student>(list);
        }

        public async Task<int> UpdateStudent(StudentViewModel inputStudent)
        {
            var _student = _mapper.Map<Student>(inputStudent);
            return await _repository.UpdateStudent(_student);
        }

        public async Task<ICollection<GroupViewModel>> GetAllGroups()
        {
            var _list = await _repository.GetGroups();
            return _mapper.ToMapCollection<GroupViewModel, Group>(_list);
        }
        public async Task<Page<GroupViewModel>> GetGroups(int index)
        {
            var pageSize = _config.GetValue<int>("pageSize");
            var page = await _repository.GetGroups(index, pageSize);
            return _mapper.MapPage<GroupViewModel, Group>(page);
        }

        public async Task<int> DeleteGroup(int groupId)
        {
            return await _repository.DeleteGroup(groupId);
        }

        public async Task<int> UpdateGroup(GroupViewModel request)
        {
            var group = _mapper.Map<Group>(request);
            return await _repository.UpdateGroup(group);
        }

        public async Task<StudentViewModel> GetStudentById(int studentId)
        {
            var student = await _repository.GetStudentById(studentId);
            return _mapper.Map<StudentViewModel>(student);
        }
        public async Task<GroupViewModel> GetGroupById(int groupId)
        {
            var group = await _repository.GetGroupById(groupId);
            return _mapper.Map<GroupViewModel>(group);
        }

        public async Task<int> RemoveGroupFromStudent(int[] groupIds, int studentId)
        {
            return await _repository.RemoveGroupFromStudent(groupIds, studentId);
        }

        public async Task<int> AddGroupToStudent(int[] groupIds, int studentId)
        {
            return await _repository.AddGroupToStudent(groupIds, studentId);
        }

        public async Task<Page<GroupViewModel>> GetGroupsByStudentId(int index, int studentId)
        {
            var pageSize = _config.GetValue<int>("pageSize");
            var page = await _repository.GetGroupsByStudentId(index, pageSize, studentId);
            return _mapper.MapPage<GroupViewModel, Group>(page);
        }
    }

}