using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Contract
{
    public interface IAuthRepository
    {
        User ValidateUser(string username);
        bool UpdateLoginBit(User updatedUser);
        User GetUserDetailById(int userId);
    }
}
