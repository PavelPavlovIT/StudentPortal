namespace StudentPortalDTO.ViewModels
{
    public class AuthenticateViewModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public int StudentId { get; set; }
        public string Token { get; set; }

        public string Message { get; set; }
    }
}
