using Authentication.Common.Enum;
using Authentication.DataModels.Models;
using Authentication.DomainModels;

namespace Authentication.BusinessLogic.Interfaces
{
    public interface IUserBusinessService : IBaseBusinessService<UserModel>
    {
        AuthenticationResponseDto Register(UserRegistrationDto user, int role = (int)RolesEnum.User);
        AuthenticationResponseDto Login(LoginUserDto loginUser);
        string RefreshToken(RefreshTokenDto refreshToken);
    }
}
