using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBRepository.Models
{
    public class Student
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool isMale{ get; set; }

        [Required]
        [MaxLength(40)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(40)]
        public string LastName { get; set; }

        [MaxLength(60)]
        public string Patronymic { get; set; }

        [MinLength(6)]
        [MaxLength(16)]
        public string Nick { get; set; }
        public ICollection<UserStudents> UserStudents { get; set; }
        public ICollection<GroupStudents> GroupStudents { get; set; }
    }
}
