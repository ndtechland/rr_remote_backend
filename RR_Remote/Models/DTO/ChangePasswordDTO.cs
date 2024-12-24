namespace RR_Remote.Models.DTO
{
    public class ChangePasswordDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
