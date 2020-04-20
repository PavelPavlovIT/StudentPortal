using DBRepository.StudentPortal.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace DBRepository.StudentPortal.Implementations
{
    public class RepositoryContextFactory : IRepositoryContextFactory
	{
		public StudentRepositoryContext CreateDbContext(string connectionString)
		{
			if (String.IsNullOrEmpty(connectionString)){
				var options = new DbContextOptionsBuilder<StudentRepositoryContext>()
				   .UseInMemoryDatabase(databaseName: "UnitTest")
				   .Options;
				return new StudentRepositoryContext(options);
			}
			else
			{
				var optionsBuilder = new DbContextOptionsBuilder<StudentRepositoryContext>();
				optionsBuilder.UseSqlServer(connectionString);
				return new StudentRepositoryContext(optionsBuilder.Options);
			}
		}
	}
}
