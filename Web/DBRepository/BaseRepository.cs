using DBRepository.StudentPortal.Interfaces;

namespace DBRepository
{
    public abstract class BaseRepository
    {
        protected string ConnectionString { get; }

        protected IRepositoryContextFactory ContextFactory { get; }
        public BaseRepository(string connectionString, IRepositoryContextFactory contextFactory)
        {
            ConnectionString = connectionString;
            ContextFactory = contextFactory;
        }
    }
}
