using System.Collections.Generic;

namespace WebApi.ViewModels
{
    public class GroupStudentViewModel
    {
        //public ICollection<int> GroupIds { get; set; }
        public int[] GroupIds { get; set; }
        public int StudentId { get; set; }
    }
}
