using DBRepository.StudentPortal.Implementations;

namespace DBRepository.StudentPortal.Interfaces
{
    public interface IRepositoryContextFactory
    {
        StudentRepositoryContext CreateDbContext(string connectionString);
    }
}
