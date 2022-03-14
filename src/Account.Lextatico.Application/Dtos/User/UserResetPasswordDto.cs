namespace Account.Lextatico.Application.Dtos.User
{
    public class UserResetPasswordDto : UserDto
    {
        public string ConfirmPassword { get; set; }
        public string ResetToken { get; set; }
    }
}
