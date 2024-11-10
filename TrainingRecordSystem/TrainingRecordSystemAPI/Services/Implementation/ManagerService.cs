    using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPI.Services.Implementation
{
    public class ManagerService: IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }


        public ServiceResponse<string> AddParticipate(Participate participate)
        {
            var response = new ServiceResponse<string>();
            try
            {
                if (_managerRepository.ParticipateExists(participate.Email))
                {
                    response.Success = false;
                    response.Message = "Participate already exists.";
                    return response;
                }
                var result = _managerRepository.InsertParticipate(participate);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Participate saved successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong try after sometime";
                }
            }
            catch(Exception ex) 
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public ServiceResponse<string> NominateParticipant(NominateParticipateDto participate)
        {
            var response = new ServiceResponse<string>();
            try
            {

                if (_managerRepository.NominationExists(participate.TopicId, participate.TrainerId, participate.ParticipateId))
                {
                    response.Success = false;
                    response.Message = "Participate is nominated already.";
                    return response;
                }
                var count = _managerRepository.ManagerCountForParticipate(participate.UserId, participate.TopicId, participate.TrainerId);
                if (count > 5)
                {
                    response.Success = false;
                    response.Message = "5 Participant nominated already.";
                    return response;
                }
                if (participate.ModePreference == "Hybrid" || participate.ModePreference == "hybrid")
                {
                    response.Success = false;
                    response.Message = "Please specify either 'Offline' or 'Online' for Hybrid mode.";
                    return response;
                }
                Nomination nomination = new Nomination()
                {
                    ParticipateId = participate.ParticipateId,
                    ModePreference = participate.ModePreference,
                    TopicId = participate.TopicId,
                    TrainerId = participate.TrainerId,
                };
                var result = _managerRepository.AddNomination(nomination);

                if (result)
                {
                    response.Success = true;
                    response.Message = "Nominated successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Something went wrong after sometime";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public ServiceResponse<IEnumerable<ManagerReport>> UpcomingTrainingProgram(int? jobId)
        {
            var response = new ServiceResponse<IEnumerable<ManagerReport>>();
            try
            {
                var reports = _managerRepository.UpcomingTrainingProgram(jobId);
                if (reports != null && reports.Any())
                {
                    List<ManagerReport> contactDtos = new List<ManagerReport>();
                    foreach (var contact in reports.ToList())
                    {
                        contactDtos.Add(new ManagerReport()
                        {
                            TrainerName = contact.TrainerName,
                            TopicName = contact.TopicName,
                            JobName = contact.JobName,
                            StartDate = contact.StartDate,
                            EndDate = contact.EndDate,
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

        public ServiceResponse<IEnumerable<ParticipateDto>> GetParticipateByManageId(int managerId)
        {
            var response = new ServiceResponse<IEnumerable<ParticipateDto>>();
            try
            {
                var existingParticipate = _managerRepository.GetParticipatesByManagerId(managerId);
                if (existingParticipate != null)
                {
                    var responseData = new List<ParticipateDto>();
                    foreach (var item in existingParticipate)
                    {
                        var participate = new ParticipateDto()
                        {
                            ParticipantId = item.ParticipateId,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Email = item.Email,
                            JobId = item.JobId,
                            UserId = item.UserId,
                            Job = new Job()
                            {
                                JobId = item.JobId,
                                JobName = item.Job.JobName,
                            },
                        };
                        responseData.Add(participate);
                    }
                    response.Data = responseData;
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

        public ServiceResponse<ParticipateDto> GetParticipateById(int participantId)
        {
            var response = new ServiceResponse<ParticipateDto>();
            try
            {
                var existingParticipate = _managerRepository.GetParticipate(participantId);
                if (existingParticipate != null)
                {
                    var participate = new ParticipateDto()
                    {
                        ParticipantId = existingParticipate.ParticipateId,
                        FirstName = existingParticipate.FirstName,
                        LastName = existingParticipate.LastName,
                        Email = existingParticipate.Email,
                        JobId = existingParticipate.JobId,
                    };


                    response.Data = participate;
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


        public ServiceResponse<string> GetModeofTrainingByTopicId(int userId, int topicId)
        {
            var response = new ServiceResponse<string>();
            try
            {

                var result = _managerRepository.GetModeofTrainingByTopicId(userId, topicId);

                if (result != "")
                {
                    response.Data = result;
                    response.Success = true;
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


    }
}
