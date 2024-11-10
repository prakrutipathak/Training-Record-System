using Microsoft.Win32;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Data.Implementation;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPI.Services.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITopicRepository _topicRepository;

        public AdminService(IAdminRepository userRepository, IPasswordService passwordService, ITopicRepository topicRepository)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _topicRepository = topicRepository;
        }


        public ServiceResponse<IEnumerable<MonthlyAdminReportDto>> MonthlyAdminReport(int userId, int? month, int? year)
        {
            var response = new ServiceResponse<IEnumerable<MonthlyAdminReportDto>>();
            try
            {
                var reports = _userRepository.MonthlyAdminReport(userId, month, year);

                if (reports != null && reports.Any())
                {
                    List<MonthlyAdminReportDto> contactDtos = new List<MonthlyAdminReportDto>();
                    foreach (var contact in reports.ToList())
                    {
                        contactDtos.Add(new MonthlyAdminReportDto()
                        {
                            TopicName = contact.TopicName,
                            StartDate = contact.StartDate,
                            EndDate = contact.EndDate,
                            Duration = contact.Duration,
                            ModePreference = contact.ModePreference,
                            TotalParticipateNo = contact.TotalParticipateNo,
                        });
                    }
                    response.Data = contactDtos;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }


            return response;
        }  
        
        public ServiceResponse<IEnumerable<DaterangeBasedReportDto>> DaterangeBasedReport(int jobId, DateTime? startDate, DateTime? endDate)
        {
            var response = new ServiceResponse<IEnumerable<DaterangeBasedReportDto>>();
            try
            {
                var reports = _userRepository.DaterangeBasedReport(jobId, startDate, endDate);

                if (reports != null && reports.Any())
                {
                    List<DaterangeBasedReportDto> contactDtos = new List<DaterangeBasedReportDto>();
                    foreach (var contact in reports.ToList())
                    {
                        contactDtos.Add(new DaterangeBasedReportDto()
                        {
                            TopicName = contact.TopicName,
                            TrainerName = contact.TrainerName,
                            TotalParticipateNo = contact.TotalParticipateNo,
                            StartDate = contact.StartDate,
                            EndDate = contact.EndDate,
                            Duration = contact.Duration,
                            ModePreference = contact.ModePreference,
                        });
                    }
                    response.Data = contactDtos;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        //----------------add user-----------------
        public ServiceResponse<string> AddUser(AddUserDto userDataDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (userDataDto != null)
                {
                    if (_userRepository.UserExists(userDataDto.Email))
                    {
                        response.Success = false;
                        response.Message = "User already exist";
                        return response;
                    }
                    User user = new User()
                    {
                        FirstName = userDataDto.FirstName,
                        LastName = userDataDto.LastName,
                        Email = userDataDto.Email,
                        LoginId = userDataDto.LoginId,
                        JobId = userDataDto.JobId,
                        Role = 2,
                    };

                    var password = createPassword(user.Email);

                    _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    var result = _userRepository.InsertUser(user);
                    response.Success = result;
                    response.Message = result ? "Trainer added successfully." : "Something went wrong, please try after sometime";

                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }

        private string createPassword(string email)
        {
            int index = email.IndexOf('@');
            email = email.Substring(0, index);
            return email;
        }


        //-------------------Get All trainer ---------------------

        public ServiceResponse<IEnumerable<TrainerDto>> GetAllTrainer()
        {
            var response = new ServiceResponse<IEnumerable<TrainerDto>>();
            try
            {
                var positions = _userRepository.GetAllTrainer();
                if (positions != null && positions.Any())
                {
                    List<TrainerDto> positionDtos = new List<TrainerDto>();
                    foreach (var position in positions.ToList())
                    {
                        positionDtos.Add(new TrainerDto()
                        {
                            UserId = position.UserId,
                            FirstName = position.FirstName,
                            LastName = position.LastName,
                            JobId = position.JobId,
                            Job = new Models.Job()
                            {
                                JobId = position.JobId,
                                JobName = position.Job.JobName
                            }


                        });
                    }
                    response.Data = positionDtos;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse<IEnumerable<TrainerDto>> GetAllTrainerByPagination(int page, int pageSize)
        {
            var response = new ServiceResponse<IEnumerable<TrainerDto>>();
            try
            {
                var positions = _userRepository.GetAllTrainerPagination(page, pageSize);
                if (positions != null && positions.Any())
                {
                    List<TrainerDto> positionDtos = new List<TrainerDto>();
                    foreach (var position in positions.ToList())
                    {
                        positionDtos.Add(new TrainerDto()
                        {
                            UserId = position.UserId,
                            FirstName = position.FirstName,
                            LastName = position.LastName,
                            JobId = position.JobId,
                            Job = new Models.Job()
                            {
                                JobId = position.JobId,
                                JobName = position.Job.JobName
                            }


                        });
                    }
                    response.Data = positionDtos;
                    response.Success = true;
                    response.Message = "Success";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse<int> TotalTrainer()
        {
            var response = new ServiceResponse<int>();
            try
            {
                int totalPositions = _userRepository.TotalNoOfTrainer();
                if (totalPositions != 0)
                {
                    response.Data = totalPositions;
                    response.Success = true;
                    response.Message = "Found";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceResponse<string> AssignTopicToTrainer(AssignTrainingTopicDto assignDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (assignDto == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after sometime";
                    return response;
                }
                var user = _userRepository.GetUserDetails(assignDto.UserId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after sometime";
                    return response;
                }
                var topic = _topicRepository.GetTopicDetails(assignDto.TopicId);
                if (topic == null)
                {
                    response.Success = false;
                    response.Message = "Topic doesn't exist";
                    return response;
                }
                if (user.JobId != topic.JobId)
                {
                    response.Success = false;
                    response.Message = "Cannot be assigned to trainer as job role doesn't match";
                    return response;
                }
                if (_userRepository.TopicAlreadyAssigned(assignDto.UserId, assignDto.TopicId))
                {
                    response.Success = false;
                    response.Message = "Topic has already been assigned";
                    return response;
                }

                TrainerTopic trainerTopic = new TrainerTopic()
                {
                    TopicId = assignDto.TopicId,
                    UserId = assignDto.UserId,
                    JobId = user.JobId,
                };

                var result = _userRepository.AssignTopicToTrainer(trainerTopic);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Topic successfully assigned to trainer";
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

        //---get all jobs---
        public ServiceResponse<IEnumerable<AllJobsDto>> getAllJobs()
        {
            var response = new ServiceResponse<IEnumerable<AllJobsDto>>();
            try
            {
                var jobs = _userRepository.GetAllJobs();

                if (jobs != null && jobs.Any())
                {
                    List<AllJobsDto> allJobsDtos = new List<AllJobsDto>();
                    foreach (var job in jobs)
                    {
                        allJobsDtos.Add(new AllJobsDto()
                        {
                            JobId = job.JobId,
                            JobName = job.JobName,
                        });

                        response.Data = allJobsDtos;
                        response.Success = true;
                        response.Message = "Success";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        //--get trainer by login id--
        public ServiceResponse<UserDto> GetTrainerByLoginId(string id)
        {
            var response = new ServiceResponse<UserDto>();
            try
            {
                var existingProduct = _userRepository.GetTrainerDetailsByLoginId(id);
                if (existingProduct != null)
                {
                    var product = new UserDto()
                    {
                        Email = existingProduct.Email,
                        LoginId = existingProduct.LoginId,
                        LastName = existingProduct.LastName,
                        Loginbit = existingProduct.Loginbit,
                        FirstName = existingProduct.FirstName,
                        JobId = existingProduct.JobId,
                        UserId = existingProduct.UserId,
                        Role = existingProduct.Role,

                    };
                    response.Data = product;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found!";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ServiceResponse<string> UnassignTopic(int userId, int topicId)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var result = _userRepository.UnassignTopic(userId, topicId);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Topic unassigned successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong, please try after some time";
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
