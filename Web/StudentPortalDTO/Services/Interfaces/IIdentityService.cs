using StudentPortalDTO.Models;
using StudentPortalDTO.ViewModels;
using System.Threading.Tasks;

namespace StudentPortalDTO.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticateViewModel> Authenticate(AuthenticateModel user);
        Task<AuthenticateViewModel> GetById(int id);
        Task<AuthenticateViewModel> Create(AuthenticateModel user);
        Task<AuthenticateViewModel> GetUser(string login = null, string password = null);
        Task<int> Delete(int id);
        Task SetStudent(int userId, int studentId);
    }
}
