using DBRepository.Models;
using DBRepository.StudentPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DBRepository.StudentPortal.Implementations
{
    public class IdentityRepository : BaseRepository, IIdentityRepository
    {
        public IdentityRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory) { }

        public async Task<User> GetUser(string login, string password)
        {
            //todo Replace password to passwordhash
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Password == password && u.Login == login);
                return user;
            }
        }

        public async Task<int> Delete(int userId)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var user = new User() { Id = userId };
                context.Users.Remove(user);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> Update(User user)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Users.Update(user);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<User> FindById(int id)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                return await context.Users.FirstOrDefaultAsync(p => p.Id == id);
            }
        }

        public async Task<User> Add(User user)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return user;
            }
        }

        public async Task<bool> CheckByLogin(string login)
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                User user;
                user = await context.Users.FirstOrDefaultAsync(prop => prop.Login == login);
                var result = user == null ? false : true;
                return result;
            }
        }

    }
}
