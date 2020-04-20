using DBRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace DBRepository.StudentPortal.Implementations
{
    public class StudentRepositoryContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<GroupStudents> GroupStudents { get; set; }
        public DbSet<UserStudents> UserStudents { get; set; }
        public DbSet<User> Users { get; set; }
        public StudentRepositoryContext(DbContextOptions<StudentRepositoryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasIndex(u => u.Nick)
                .IsUnique();
            modelBuilder.Entity<GroupStudents>()
                .HasIndex(u => new { u.StudentId, u.GroupId } )
                .IsUnique();
        }
    }
}
