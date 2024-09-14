using Authentication.BusinessLogic.Interfaces;
using Authentication.Common.Enum;
using Authentication.DomainModels;

namespace Authentication.Api.Startup
{
    public class DataInitizalizer
    {
        private static readonly UserRegistrationDto _userRegistrationDto = new UserRegistrationDto
        {
            Email = "neagtovo@yahoo.com",
            Password = "2904Ng*!",
            Name = "Neagtovo",
            SurName = "Admin",
        };

        public static void Initialize(IServiceProvider serviceProvider)
        {
            var userService = serviceProvider.GetService<IUserBusinessService>();

            var userToRegister = userService.Get(x => x.Email == _userRegistrationDto.Email).FirstOrDefault();
            if (userToRegister == null) { 
                userService.Register(_userRegistrationDto, role: (int)RolesEnum.Admin);
            }
        }
    }
}
