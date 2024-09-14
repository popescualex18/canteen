using Authentication.Api.Core.NotificationHub;
using Authentication.BusinessLogic.Interfaces;
using Authentication.Common.Enum;
using Authentication.Core.Helpers;
using Authentication.Core.Implementations;
using Authentication.DataModels.Models;
using Authentication.DomainModels;
using AuthenticationCore.Interfaces;
using AuthenticationDataModels;
using DataAccessLayer.Implementations;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace Authentication.BusinessLogic.Implementations
{
    public class UserBusinessService : BaseBusinessService<UserModel>, IUserBusinessService
    {
        private readonly IGeneratePasswordHelper _generatePasswordHelper;
        private readonly IOptions<ApiOptions> _apiOptions;
        private readonly ITokenHelper _tokenHelper;
        private readonly IHubContext<RegisterHub> _hubContext;
        public UserBusinessService(IHubContext<RegisterHub> hubContext, IGenericRepository<UserModel,GlobalServDbContext> generic, IGeneratePasswordHelper generatePasswordHelper, IOptions<ApiOptions> options, ITokenHelper tokenHelper) : base(generic)
        {
            _generatePasswordHelper = generatePasswordHelper;
            _apiOptions = options;
            _tokenHelper = tokenHelper;
            _hubContext = hubContext;
        }

        public AuthenticationResponseDto Login(LoginUserDto loginUser)
        {
            var response = new AuthenticationResponseDto();
            var loggedUser = GenericRepository.Get(x => x.Email == loginUser.Email, include: x => x.Role).FirstOrDefault();
            if (loggedUser == null)
            {
                response.Error = "Emailul nu exista.";
                return response;
            }

            var password = _generatePasswordHelper.GetHash(loginUser.Password + loggedUser.PasswordSalt);

            if (password != loggedUser.PasswordHash)
            {
                response.Error = "Parola nu este corecta.";
                return response;
            }


            var refreshToken = GenerateRefreshToken(loggedUser);
            response.AccessToken = _tokenHelper.GenerateToken(
                loggedUser.Id.ToString(),
                loggedUser.Email,
                loggedUser.Role.Name,
                DateTime.UtcNow.AddHours(_apiOptions.Value.TokenOptions.HoursUntilExpiration),
                _apiOptions.Value.TokenOptions.Secret
            );
            response.FullName = loggedUser.SurName;
            response.RefreshToken = refreshToken;
            return response;
        }

        public string RefreshToken(RefreshTokenDto refreshToken)
        {
            var principal = _tokenHelper.GetPrincipalFromToken(refreshToken.Token, _apiOptions.Value.TokenOptions.Secret, true);
            var user = GenericRepository.Get(x => x.Id.ToString() == principal.Identity!.Name).FirstOrDefault();
            if (user == null)
            {
                return string.Empty;
            }

            return _tokenHelper.GenerateToken(
                user.Id.ToString(),
                user.Email,
                user.RoleId.ToString(),
                DateTime.UtcNow.AddHours(_apiOptions.Value.TokenOptions.HoursUntilExpiration),
                _apiOptions.Value.TokenOptions.Secret
            );
        }

        public AuthenticationResponseDto Register(UserRegistrationDto model, int role = (int)RolesEnum.User)
        {
            var response = new AuthenticationResponseDto();
            var existingUser = GenericRepository.Get(x => x.Email == model.Email).FirstOrDefault();
            if (existingUser != null)
            {
                response.Error = "Email este folosit. Va rugam folositi alta adresa de email";
                return response;
            }

            var userToAdd = new UserModel()
            {
                Email = model.Email,
                SurName = model.SurName ?? "",
                Name = model.Name,
                RoleId = role,
            };
            var salt = _generatePasswordHelper.GetSalt();

            userToAdd.PasswordSalt = salt;
            userToAdd.PasswordHash = _generatePasswordHelper.GetHash(model.Password + salt);
            Add(userToAdd);
            _hubContext.Clients.All.SendAsync("RegisterMessage", userToAdd.Id);
            return Login(model);
        }
        private string GenerateRefreshToken(UserModel user)
        {
            var tokenHelp = new TokenHelper();
            var refreshTokenExpirationDate = DateTime.UtcNow
                .AddHours(_apiOptions.Value.RefreshTokenOptions.HoursUntilExpiration);

            var refreshToken = tokenHelp.GenerateToken(
                user.Id.ToString(),
                user.Email,
                user.RoleId.ToString(),
                refreshTokenExpirationDate,
                _apiOptions.Value.RefreshTokenOptions.Secret);

            return refreshToken;
        }
    }
}
