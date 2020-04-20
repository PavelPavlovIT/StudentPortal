using System.Collections.Generic;

namespace StudentPortalDTO.ViewModels
{
    public class StudentViewModel
    {
        public int userId { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Nick { get; set; }
        public bool isMale { get; set; }

        public ICollection<GroupViewModel> Groups { get; set; }
        
    }
}
