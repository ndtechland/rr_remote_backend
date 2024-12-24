namespace RR_Remote.Models.ApiDTO
{
    public class ChangePassword
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
