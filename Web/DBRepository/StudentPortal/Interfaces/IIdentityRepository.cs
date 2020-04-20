using DBRepository.Models;
using System.Threading.Tasks;

namespace DBRepository.StudentPortal.Interfaces
{
    public interface IIdentityRepository
    {
        Task<User> GetUser(string login, string password);
        Task<int> Delete(int userId);
        Task<int> Update(User user);
        Task<User> Add(User user);
        Task<User> FindById(int id);
        Task<bool> CheckByLogin(string login);
    }
}
