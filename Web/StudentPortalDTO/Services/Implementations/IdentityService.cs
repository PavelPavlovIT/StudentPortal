using AutoMapper;
using DBRepository.Models;
using DBRepository.StudentPortal.Interfaces;
using StudentPortalDTO.Models;
using StudentPortalDTO.Services.Interfaces;
using StudentPortalDTO.ViewModels;
using System;
using System.Threading.Tasks;

namespace StudentPortalDTO.Services.Implementations
{
    public class IdentityService : IIdentityService
    {
        IIdentityRepository _repository;
        IMapper _mapper;

        public IdentityService(IIdentityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<AuthenticateViewModel> Authenticate(AuthenticateModel user)
        {
            if (string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password))
                return null;

            var _user = await _repository.GetUser(user.Login, user.Password);

            if (_user == null)
                return null;

            if (!VerifyPasswordHash(user.Password, _user.PasswordHash, _user.PasswordSalt))
                return null;

            return _mapper.Map<AuthenticateViewModel>(_user);
        }
        public async Task<AuthenticateViewModel> GetById(int id)
        {
            var _user = await _repository.FindById(id);
            if (_user == null)
                return new AuthenticateViewModel() { Message = "User not found" };
            return _mapper.Map<AuthenticateViewModel>(_user);
        }
        public async Task<AuthenticateViewModel> Create(AuthenticateModel user)
        {
            if (string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Login))
                return new AuthenticateViewModel() { Message = "Password is required" };

            var userFound = await _repository.CheckByLogin(user.Login);
            if (userFound)
                return new AuthenticateViewModel() { Message = "Username \"" + user.Login + "\" is already taken" };
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

            var _user = await _repository.Add(new User() { Login = user.Login, Password = user.Password, PasswordHash = passwordHash, PasswordSalt = passwordSalt });
            return _mapper.Map<AuthenticateViewModel>(_user);
        }
        public async Task<AuthenticateViewModel> GetUser(string login = null, string password = null)
        {
            var user = await _repository.GetUser(login, password);
            return _mapper.Map<AuthenticateViewModel>(user);
        }
        public async Task<int> Delete(int id)
        {
            return await _repository.Delete(id);
        }
        public async Task SetStudent(int userId, int studentId)
        {
            var user = await _repository.FindById(userId);
            //if (user == null)
            //    throw new AppException("User not found");
            user.StudentId = studentId;
            await _repository.Update(user);
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
