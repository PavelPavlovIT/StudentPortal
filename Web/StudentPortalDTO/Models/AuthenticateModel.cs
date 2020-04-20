
using System.ComponentModel.DataAnnotations;

namespace StudentPortalDTO.Models
{
    public class AuthenticateModel
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
