using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DBRepository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentPortalDTO.Services.Interfaces;
using StudentPortalDTO.ViewModels;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly IStudentPortalService _studentPortalService;
        private readonly IMapper _mapper;
        //todo add return IActionResult
        public StudentController(IStudentPortalService studentPortalService, IMapper mapper)
        {
            _studentPortalService = studentPortalService;
            _mapper = mapper;
        }

        [Authorize]
//        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Produces("application/json")]
        [Microsoft.AspNetCore.Mvc.Route("list")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IEnumerable<StudentViewModel>> Get()
        {
            return await _studentPortalService.GetAllStudents();
        }

        [Authorize]
  //      [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.Route("page")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetStudents(int pageIndex, int id, string filter, bool? isMale, string orderBy, bool? desc)
        {
            return Ok(await _studentPortalService.GetStudents(pageIndex <= 0 ? 0 : pageIndex - 1, id, filter, isMale, orderBy, desc));
        }

        [Authorize]
        //[Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.Route("student")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<StudentViewModel> GetStudentById(int id)
        {
            return await _studentPortalService.GetStudentById(id);
        }

        [Authorize]
        //[Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.Route("group")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<GroupViewModel> GetGroupById(int id)
        {
            return await _studentPortalService.GetGroupById(id);
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("add")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> AddStudent([Microsoft.AspNetCore.Mvc.FromBody] StudentViewModel request)
        {
            return Ok(await _studentPortalService.AddStudent(request.userId, request));
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("delete")]
        [Microsoft.AspNetCore.Mvc.HttpDelete]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            return Ok(await _studentPortalService.DeleteStudent(studentId));
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("update")]
        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateStudent([Microsoft.AspNetCore.Mvc.FromBody] StudentViewModel value)
        {
            value.Groups = null;
            return Ok(await _studentPortalService.UpdateStudent(value));
        }

        [Authorize]
        //[Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.Route("listgroup")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetAllGroup()
        {
            return Ok(await _studentPortalService.GetAllGroups());
        }

        [Authorize]
        //[Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [Microsoft.AspNetCore.Mvc.Route("pagegroup")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> GetGroups(int pageIndex)
        {
            return Ok(await _studentPortalService.GetGroups(pageIndex <= 0 ? 0 : pageIndex - 1));
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("addGroup")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> AddGroup([Microsoft.AspNetCore.Mvc.FromBody]GroupViewModel request)
        {
            return Ok(await _studentPortalService.AddGroup(request));
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("deleteGroup")]
        [Microsoft.AspNetCore.Mvc.HttpDelete]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            return Ok(_studentPortalService.DeleteGroup(groupId));
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("updateGroup")]
        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> UpdateGroup([Microsoft.AspNetCore.Mvc.FromBody] GroupViewModel value)
        {
            return Ok(await _studentPortalService.UpdateGroup(value));
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("addGroupToStudent")]
        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> AddGroupToStudent([Microsoft.AspNetCore.Mvc.FromBody] GroupStudentViewModel value)
        {
            return Ok(await _studentPortalService.AddGroupToStudent(value.GroupIds, value.StudentId));
        }

        [Authorize]
        [Microsoft.AspNetCore.Mvc.Route("removeGroupFromStudent")]
        [Microsoft.AspNetCore.Mvc.HttpPut]
        public async Task<IActionResult> RemoveGroupFromStudent([Microsoft.AspNetCore.Mvc.FromBody] GroupStudentViewModel value)
        {
            return Ok(await _studentPortalService.RemoveGroupFromStudent(value.GroupIds, value.StudentId));
        }

    }
}
