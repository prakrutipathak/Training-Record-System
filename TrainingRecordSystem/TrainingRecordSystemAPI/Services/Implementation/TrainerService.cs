using Microsoft.AspNetCore.Components.Web;
using TrainingRecordSystemAPI.Data.Contract;
using TrainingRecordSystemAPI.Dtos;
using TrainingRecordSystemAPI.Models;
using TrainingRecordSystemAPI.Services.Contract;

namespace TrainingRecordSystemAPI.Services.Implementation
{
    public class TrainerService : ITrainerService
    {
        private readonly ITrainerReository _trainerReository;



        public TrainerService(ITrainerReository trainerReository)
        {
            _trainerReository = trainerReository;

        }

        public ServiceResponse<IEnumerable<TrainingTopicDto>> GetAllTrainingTopicbyTrainerId(int trainerId, int page, int pageSize)
        {
            var response = new ServiceResponse<IEnumerable<TrainingTopicDto>>();
            try
            {
                var tariningTopics = _trainerReository.GetAllTrainingTopicbyTrainerId(trainerId, page, pageSize);

                if (tariningTopics != null)
                {
                    List<TrainingTopicDto> trainingTopicDto = new List<TrainingTopicDto>();

                    foreach (var tariningTopic in tariningTopics)
                    {
                        trainingTopicDto.Add(

                            new TrainingTopicDto()
                            {
                                TrainerTopicId = tariningTopic.TrainerTopicId,
                                UserId = tariningTopic.UserId,
                                JobId = tariningTopic.JobId,
                                TopicId = tariningTopic.TopicId,
                                User = new User()
                                {
                                    UserId = tariningTopic.UserId,
                                    FirstName = tariningTopic.User.FirstName,
                                },
                                Job = new Job()
                                {
                                    JobId = tariningTopic.JobId,
                                    JobName = tariningTopic.Job.JobName,
                                },
                                Topic = new Topic()
                                {
                                    TopicId = tariningTopic.TopicId,
                                    TopicName = tariningTopic.Topic.TopicName,
                                },
                                isTrainingScheduled = false
                            });
                    }

                    foreach (var item in trainingTopicDto)
                    {
                        item.isTrainingScheduled = _trainerReository.TrainingProgramDetailExists(item.UserId, item.TrainerTopicId);
                    }
                    response.Success = true;
                    response.Data = trainingTopicDto;
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found!";
                    return response;
                }
            }
            
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public ServiceResponse<int> TotalCountofTrainingTopicbyTrainerId(int trainerId)
        {
            var response = new ServiceResponse<int>();
            try
            {
                var result = _trainerReository.TotalCountofTrainingTopicbyTrainerId(trainerId);
                response.Data = result;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        //------------Get All Participants by Pagination---------------------
        public ServiceResponse<IEnumerable<NominationDto>> GetAllParticipantsByPAgination(int page, int pageSize, string sort_name)
        {
            var response = new ServiceResponse<IEnumerable<NominationDto>>();
            try
            {
                var participants = _trainerReository.GetAllParticipateByPagination(page, pageSize, sort_name);
                if (participants != null && participants.Any())
                {
                    List<NominationDto> participantsDto = new List<NominationDto>();
                    foreach (var p in participants.ToList())
                    {
                        participantsDto.Add(new NominationDto()
                        {
                            NominationId = p.NominationId,
                            ModePreference = p.ModePreference,
                            TopicId = p.TopicId,
                            ParticipateId = p.ParticipateId,
                            Topic = new Topic()
                            {
                                TopicId = p.TopicId,
                                TopicName = p.Topic.TopicName
                            },
                            Participate = new Participate()
                            {
                                ParticipateId = p.ParticipateId,
                                FirstName = p.Participate.FirstName,
                                LastName = p.Participate.LastName,
                                Email = p.Participate.Email,
                                UserId = p.Participate.UserId,
                                JobId = p.Participate.JobId,
                                User = new User()
                                {
                                    UserId = p.Participate.UserId,
                                    FirstName = p.Participate.User.FirstName,
                                    LastName = p.Participate.User.LastName
                                },
                                Job = new Job()
                                {
                                    JobId = p.Participate.JobId,
                                    JobName = p.Participate.Job.JobName
                                },

                            },
                        });
                    }
                    response.Data = participantsDto;
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

        public ServiceResponse<int> TotalParticipants()
        {
            var response = new ServiceResponse<int>();
            try
            {
                int totalParticipants = _trainerReository.TotalNofParticipants();
                if (totalParticipants != 0)
                {
                    response.Data = totalParticipants;
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

        public ServiceResponse<string> AddTrainingProgramDetail(AddTrainingProgramDetailDto trainingProgramDetailDto) 
        {
            var response = new ServiceResponse<string>();
            try
            {
                var startDate = trainingProgramDetailDto.StartDate.Date;
                var endDate = trainingProgramDetailDto.EndDate.Date;

                var startTime = DateTime.Parse(trainingProgramDetailDto.StartTime, System.Globalization.CultureInfo.CurrentCulture);
                var endTime = DateTime.Parse(trainingProgramDetailDto.EndTime, System.Globalization.CultureInfo.CurrentCulture);

                if (_trainerReository.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId))
                {
                    response.Success = false;
                    response.Message = "Training program details have been already added";
                    return response;
                }

                if (startDate < DateTime.Now.Date)
                {
                    response.Success = false;
                    response.Message = "Start date needs to be a future date";
                    return response;
                }

                if (startDate > endDate)
                {
                    response.Success = false;
                    response.Message = "Start date cannot be after end date";
                    return response;
                }

                if (startTime > endTime)
                {
                    response.Success = false;
                    response.Message = "Start time cannot be after end time";
                    return response;
                }

                if (trainingProgramDetailDto.ModePreference.ToLower() != "hybrid" &&
                    trainingProgramDetailDto.ModePreference.ToLower() != "offline" &&
                    trainingProgramDetailDto.ModePreference.ToLower() != "online")
                {
                    response.Success = false;
                    response.Message = "Mode preference can only be either 'Hybrid', 'Online', or 'Offline'";
                    return response;
                }

                var duration = endTime.Subtract(startTime).Hours;
                var trainerProgramDetail = new TrainerProgramDetail()
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    Duration = (int)duration,
                    ModePreference = trainingProgramDetailDto.ModePreference.ToLower(),
                    TargetAudience = trainingProgramDetailDto.TargetAudience,
                    TrainerTopicId = trainingProgramDetailDto.TrainerTopicId,
                };

                var result = _trainerReository.AddTrainingProgramDetail(trainerProgramDetail);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Training program details added successfully";
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


        public ServiceResponse<TrainingProgramDetailsDto> GetAllTraniningProgramDetails(int userId, int topicId)
        {
            var response = new ServiceResponse<TrainingProgramDetailsDto>();
            try
            {
                var programDetails = _trainerReository.GetAllTrainerProgramDetails(userId, topicId);

                if (programDetails != null)
                {
                    var data = new TrainingProgramDetailsDto
                    {
                        TrainerProgramDetailId = programDetails.TrainerProgramDetailId,
                        StartDate = programDetails.StartDate,
                        EndDate = programDetails.EndDate,
                        StartTime = programDetails.StartTime,
                        EndTime = programDetails.EndTime,
                        Duration = programDetails.Duration,
                        ModePreference = programDetails.ModePreference,
                        TargetAudience = programDetails.TargetAudience,
                        TrainerTopic = new TrainerTopicDtoForProgramDetails
                        {
                            TrainerTopicId = programDetails.TrainerTopicId,
                            UserId = programDetails.TrainerTopic.UserId,
                            TopicId = programDetails.TrainerTopic.TopicId,
                            JobId = programDetails.TrainerTopic.JobId,

                        },
                        TrainerTopicId = programDetails.TrainerTopicId

                    };


                    response.Success = true;
                    response.Data = data;
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public ServiceResponse<string> UpdateTrainingProgramDetails(UpdateTrainingProgramDetailDto trainingProgramDetailDto)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var startDate = trainingProgramDetailDto.StartDate.Date;
                var endDate = trainingProgramDetailDto.EndDate.Date;

                var startTime = DateTime.Parse(trainingProgramDetailDto.StartTime, System.Globalization.CultureInfo.CurrentCulture);
                var endTime = DateTime.Parse(trainingProgramDetailDto.EndTime, System.Globalization.CultureInfo.CurrentCulture);

                if (!_trainerReository.TrainingProgramDetailExists(trainingProgramDetailDto.TrainerTopicId))
                {
                    response.Success = false;
                    response.Message = "Please add training program details first";
                    return response;
                }

                if (startDate < DateTime.Now.Date)
                {
                    response.Success = false;
                    response.Message = "Start date needs to be a future date";
                    return response;
                }

                if (startDate > endDate)
                {
                    response.Success = false;
                    response.Message = "Start date cannot be after end date";
                    return response;
                }

                if (startTime > endTime)
                {
                    response.Success = false;
                    response.Message = "Start time cannot be after end time";
                    return response;
                }

                if (trainingProgramDetailDto.ModePreference.ToLower() != "hybrid" &&
                    trainingProgramDetailDto.ModePreference.ToLower() != "offline" &&
                    trainingProgramDetailDto.ModePreference.ToLower() != "online")
                {
                    response.Success = false;
                    response.Message = "Mode preference can only be either 'Hybrid', 'Online', or 'Offline'";
                    return response;
                }

                var duration = endTime.Subtract(startTime).TotalHours;
                var trainerProgramDetail = new TrainerProgramDetail()
                {
                    TrainerProgramDetailId = trainingProgramDetailDto.TrainerProgramDetailId,
                    StartDate = startDate,
                    EndDate = endDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    Duration = (int)duration,
                    ModePreference = trainingProgramDetailDto.ModePreference.ToLower(),
                    TargetAudience = trainingProgramDetailDto.TargetAudience,
                    TrainerTopicId = trainingProgramDetailDto.TrainerTopicId,
                };

                var result = _trainerReository.UpdateTrainingProgramDetails(trainerProgramDetail);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Training program details updated successfully";
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
    }
}
