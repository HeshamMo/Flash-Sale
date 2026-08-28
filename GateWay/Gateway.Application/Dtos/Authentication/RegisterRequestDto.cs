namespace Gateway.Application.Dtos.Authentication
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
