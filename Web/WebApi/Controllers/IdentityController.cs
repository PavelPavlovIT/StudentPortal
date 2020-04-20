using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentPortalDTO.Models;
using StudentPortalDTO.Services.Interfaces;
using StudentPortalDTO.ViewModels;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public IdentityController(
            IConfiguration config,
            IIdentityService userService,
            IMapper mapper)
        {
            _identityService = userService;
            _mapper = mapper;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel model)
        {
            var user = await _identityService.Authenticate(model);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenString = GenerateJSONWebToken(user);
            return Ok(new AuthenticateViewModel { UserId = user.UserId, Login = user.Login, StudentId = user.StudentId, Token = tokenString });
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]AuthenticateModel model)
        {
            var user = await _identityService.GetUser(model.Login, model.Password);
            if (user != null)
            {
                return BadRequest(new { message = "User exist" });
            }
            var result = await _identityService.Create(model);
            return Ok(result);
        }

        [Route("userbyid")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _identityService.GetById(id);
            return Ok(user);
        }


        [Route("setstudent")]
        [Authorize]
        [HttpPut]
        public async Task SetStudent([FromBody]SetStudentModel body)
        {
            await _identityService.SetStudent(body.UserId, body.StudentId);
        }

        [Route("delete")]
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _identityService.Delete(id);
            return Ok();
        }
        private string GenerateJSONWebToken(AuthenticateViewModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}