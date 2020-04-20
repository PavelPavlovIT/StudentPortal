using System.Collections.Generic;

namespace DBRepository.Models
{
    public class User
    {
        public User() { }
        public int Id { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public int StudentId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ICollection<UserStudents> UserStudents { get; set; }
    }
}
