using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using DBRepository.StudentPortal.Implementations;
using Microsoft.Extensions.Configuration;

namespace WebApi
{
    public class DesignTimeRepositoryContextFactory : IDesignTimeDbContextFactory<StudentRepositoryContext>
	{
		public StudentRepositoryContext CreateDbContext(string[] args)
		{
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString("DefaultConnection");
            var repositoryFactory = new RepositoryContextFactory();
            return repositoryFactory.CreateDbContext(connectionString);
		}
	}
}
