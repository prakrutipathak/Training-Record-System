using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Services.Contract
{
    public interface IPasswordService
    {
        string CheckPasswordStrength(string password);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        string CreateToken(User user);
    }
}
