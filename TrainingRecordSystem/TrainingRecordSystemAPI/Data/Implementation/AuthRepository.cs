using Microsoft.EntityFrameworkCore;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Implementation
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IAppDbContext _context;

        public AuthRepository(IAppDbContext appDbcontext)
        {
            _context = appDbcontext;
        }
        public User ValidateUser(string username)
        {
            User? user = _context.Users.FirstOrDefault(c => c.LoginId.ToLower() == username.ToLower() || c.Email == username.ToLower());
            return user;
        }
  
        //--------------Update User----------------
        public bool UpdateLoginBit(User updatedUser)
        {
            var result = false;
            if (updatedUser != null)
            {
                _context.Users.Update(updatedUser);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }

        //-----------GetUserDetailById------------

        public User GetUserDetailById(int userId)
        {
            var user = _context.Users.Include(c => c.Job).FirstOrDefault(c => c.UserId == userId);

            return user;
        }
    }
}
 