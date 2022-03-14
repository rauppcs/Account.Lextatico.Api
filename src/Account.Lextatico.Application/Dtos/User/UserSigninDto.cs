namespace Account.Lextatico.Application.Dtos.User
{
    public class UserSignInDto : UserDto
    {
        public string Name { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
