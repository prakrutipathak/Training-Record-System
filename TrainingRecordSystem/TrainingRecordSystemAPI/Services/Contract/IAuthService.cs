using TrainingRecordSystemAPI.Dtos;

namespace TrainingRecordSystemAPI.Services.Contract
{
    public interface IAuthService
    {
        ServiceResponse<string> LoginUserService(LoginDto login);
        ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto);

        ServiceResponse<UserDetailsDto> GetUserDetailById(int userId);
    }
}
