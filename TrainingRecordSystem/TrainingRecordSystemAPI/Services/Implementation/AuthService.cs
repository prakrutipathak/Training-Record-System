using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Data.Implementation;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPI.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IPasswordService _passwordService;
        public AuthService(IAuthRepository userRepository, IPasswordService passwordService)
        {
            _authRepository = userRepository;
            _passwordService = passwordService;
        }
        //-------------Login---------------
        public ServiceResponse<string> LoginUserService(LoginDto login)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (login != null)
                {
                    var user = _authRepository.ValidateUser(login.Username);
                    if (user == null)
                    {
                        response.Success = false;
                        response.Message = "Invalid user login id or password";
                        return response;
                    }
                    else if (!_passwordService.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
                    {
                        response.Success = false;
                        response.Message = "Invalid user login id or password";
                        return response;
                    }

                    user.Loginbit = true;

                    _authRepository.UpdateLoginBit(user);
                    string token = _passwordService.CreateToken(user);
                    response.Success = true;
                    response.Data = token;
                    response.Message = "Success";
                    return response;
                }
           

            response.Success = false;
            response.Message = "Something went wrong, please try after some time";

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        //-------------------Change Password----------------
        public ServiceResponse<string> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var response = new ServiceResponse<string>();
            try { 
            var message = string.Empty;
            if (changePasswordDto != null)
            {
                var user = _authRepository.ValidateUser(changePasswordDto.LoginId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                    return response;
                }
                if (!_passwordService.VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt))
                {
                    response.Success = false;
                    response.Message = "Old password is incorrect.";
                    return response;
                }
                if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
                {
                    response.Success = false;
                    response.Message = "Old password and new password can not be same.";
                    return response;
                }
                message = _passwordService.CheckPasswordStrength(changePasswordDto.NewPassword);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    response.Success = false;
                    response.Message = message;
                    return response;
                }
                _passwordService.CreatePasswordHash(changePasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                var result = _authRepository.UpdateLoginBit(user);
                response.Success = result;
                if (result)
                {
                    response.Message = "Successfully changed password, Please signin!";
                }
                else
                {
                    response.Message = "Something went wrong, please try after sometime";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Something went wrong, please try after sometime";
            }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }


        //---------GetUserDetailById-------------

        public ServiceResponse<UserDetailsDto> GetUserDetailById(int userId)
        {
            var response = new ServiceResponse<UserDetailsDto>();
            try { 
            var userDetail = _authRepository.GetUserDetailById(userId);

            if (userDetail != null)
            {

                var userDetailDto = new UserDetailsDto()
                {
                    UserId = userDetail.UserId,
                    LoginId = userDetail.LoginId,
                    FirstName = userDetail.FirstName,
                    LastName = userDetail.LastName,
                    Email = userDetail.Email,
                    Role = userDetail.Role,
                    JobId = userDetail.JobId,
                  

                };

                response.Data = userDetailDto;

            }
            else
            {
                response.Success = false;
                response.Message = "No record found !.";
            }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


    }
}
