using System;
using System.Collections.Generic;
using System.Text;

namespace DBRepository.Models
{
    public class StudentView
    {
        public int Id { get; set; }
        public bool isMale { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Nick { get; set; }
        public ICollection<Group> Groups{ get; set; }
    }
}
