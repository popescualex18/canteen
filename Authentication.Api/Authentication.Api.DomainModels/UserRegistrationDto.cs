namespace Authentication.DomainModels
{
    public class UserRegistrationDto : LoginUserDto
    {
        public string? SurName { get; set; }
        public string Name { get; set; }
    }
}