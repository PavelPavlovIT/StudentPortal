using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBRepository.Models
{
    public class Group
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string GroupName { get; set; }
        public ICollection<GroupStudents> GroupStudents { get; set; }
    }
}
