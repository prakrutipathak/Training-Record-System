using Microsoft.EntityFrameworkCore;
using System.IO;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;

namespace TrainingRecordSystemAPI.Data.Implementation
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IAppDbContext _context;

        public AdminRepository(IAppDbContext appDbcontext)
        {
            _context = appDbcontext;
        }



        //--------Report------------//
        public IEnumerable<MonthlyAdminReportDto> MonthlyAdminReport(int userId, int? month,int? year)
        {
            var results = _context.MonthlyAdminReport(userId, month, year);
            return results.ToList(); 
        } 
        
        public IEnumerable<DaterangeBasedReportDto> DaterangeBasedReport(int jobId, DateTime? startDate, DateTime? endDate)
        {
            var results = _context.DaterangeBasedReport(jobId, startDate, endDate);
            return results.ToList(); 
        }

        //----------------------Add User-------------------
        public bool InsertUser(User user)
        {
            var result = false;
            if (user != null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }

        public IEnumerable<Job> GetAllJobs()
        {
            return _context.Jobs.Take(3).ToList();
        }

        //-------------------User Exist---------------
        public bool UserExists(string email)
        {
            var userExist = _context.Users.FirstOrDefault(c => c.Email.ToLower() == email.ToLower());
            if (userExist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //--------------User Exist with id----------------
        public bool UserExists(int userId, string email)
        {
            var userExist = _context.Users.FirstOrDefault(c => c.UserId != userId && c.Email.ToLower() == email.ToLower());
            if (userExist != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //-------------GetAllTrainer --------------------

        public IEnumerable<User> GetAllTrainer()
        {


            return _context.Users
                .Where(u => u.Role == 2)
                .Include(u => u.Job)
                .ToList();
        }

        public IEnumerable<User> GetAllTrainerPagination(int page, int pageSize)
        {
            
            int skip = (page - 1) * pageSize;
            
            return _context.Users
                .Where(u => u.Role == 2)
                .Include(u => u.Job)
                .Skip(skip)
                .Take(pageSize)
                .ToList();
        }
        public int TotalNoOfTrainer()
        {
            return _context.Users.Where(u=>u.Role==2).Count();
        }
        public bool AssignTopicToTrainer(TrainerTopic trainerTopic)
        {
            var result = false;
            if (trainerTopic != null)
            {
                _context.TrainerTopics.Add(trainerTopic);
                _context.SaveChanges();
                result = true;
            }
            return result;
        }

        public User GetUserDetails(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            return user;
        }

        public bool TopicAlreadyAssigned(int userId, int topicId)
        {
            var topicAssigned = _context.TrainerTopics.FirstOrDefault(c => c.UserId == userId && c.TopicId == topicId);
            if (topicAssigned != null)
            {
                return true;
            }
            return false;
        }


        public User GetTrainerDetailsByLoginId(string LoginId)
        {
            var user = _context.Users.FirstOrDefault(u => u.LoginId == LoginId);
            return user;
        }

        public bool UnassignTopic(int userId, int topicId)
        {
            var result = false;
            var trainerTopic = _context.TrainerTopics.FirstOrDefault(c => c.UserId == userId && c.TopicId == topicId);
            var trainerProgramDetails = _context.TrainerProgramDetails.FirstOrDefault(c => c.TrainerTopicId == trainerTopic.TrainerTopicId);
            if (trainerTopic != null)
            {
                _context.TrainerTopics.Remove(trainerTopic);
                if(trainerProgramDetails != null)
                {
                    _context.TrainerProgramDetails.Remove(trainerProgramDetails);
                }
                _context.SaveChanges();
                result = true;
            }

            return result;
        }
    }
}
