import { TestBed } from '@angular/core/testing';

import { TrainerService } from './trainer.service';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TrainingTopic } from '../models/trainingTopic.model';
import { ApiResponse } from '../models/ApiResponse{T}.model';
import { AddTrainingProgramDetail } from '../models/add-training-program-detail.model';
import { UpcomingTraining } from '../models/upcomingtraining.model';
import { UpdateTrainingProgramDetail } from '../models/update-training-program-detail.model';

describe('TrainerService', () => {
  let service: TrainerService;
  let httpMock :HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule,HttpClientTestingModule],
      providers : [TrainerService]
    });
    service = TestBed.inject(TrainerService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  
  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  //---------------------getAllTrainingTopicbyTrainerId--------------------
  it('should get all topic successfully', ()=>{
    //Arrange

    const mockInfo : TrainingTopic[] = [
      {
        trainerTopicId : 1,
        userId : 1,
        topicId : 1,
        jobId : 1,
        topic : {
          topicId : 1,
          topicName : "test",
          jobId : 1
        },
        user : {
          userId : 1,
          firstName : "test"
        },
        job : {
          jobId : 1,
          jobName : "test"
        },
        isTrainingScheduled : false
      },
      {
        trainerTopicId : 2,
        userId : 2,
        topicId : 2,
        jobId : 2,
        topic : {
          topicId : 2,
          topicName : "test",
          jobId : 2
        },
        user : {
          userId : 2,
          firstName : "test"
        },
        job : {
          jobId : 2,
          jobName : "test"
        },
        isTrainingScheduled : true
      }
    ]
      const mockApiResponse : ApiResponse<TrainingTopic[]> = {
        success : true,
        data : mockInfo,
        message : ''
      }

      const apiUrl = 'http://localhost:5038/api/Trainer/GetAllTrainingTopicbyTrainerId/1?page=1&pageSize=6';

    //Act

    service.getAllTrainingTopicbyTrainerId(1,1,6).subscribe((response) =>{
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  })

  it('should handle error response', ()=>{
    //Arrange

    const mockInfo : TrainingTopic[] = [
      {
        trainerTopicId : 1,
        userId : 1,
        topicId : 1,
        jobId : 1,
        topic : {
          topicId : 1,
          topicName : "test",
          jobId : 1
        },
        user : {
          userId : 1,
          firstName : "test"
        },
        job : {
          jobId : 1,
          jobName : "test"
        },
        isTrainingScheduled : false
      },
      {
        trainerTopicId : 2,
        userId : 2,
        topicId : 2,
        jobId : 2,
        topic : {
          topicId : 2,
          topicName : "test",
          jobId : 2
        },
        user : {
          userId : 2,
          firstName : "test"
        },
        job : {
          jobId : 2,
          jobName : "test"
        },
        isTrainingScheduled : true
      }
    ]
      const mockApiResponse : ApiResponse<TrainingTopic[]> = {
        success : false,
        data : mockInfo,
        message : ''
      }

      const apiUrl = 'http://localhost:5038/api/Trainer/GetAllTrainingTopicbyTrainerId/1?page=1&pageSize=6';

    //Act

    service.getAllTrainingTopicbyTrainerId(1,1,6).subscribe((response) =>{
      
      expect(response).toEqual(mockApiResponse);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  })
  it('should handle HTTP error response', ()=>{
    //Arrange

    const mockInfo : TrainingTopic[] = [
      {
        trainerTopicId : 1,
        userId : 1,
        topicId : 1,
        jobId : 1,
        topic : {
          topicId : 1,
          topicName : "test",
          jobId : 1
        },
        user : {
          userId : 1,
          firstName : "test"
        },
        job : {
          jobId : 1,
          jobName : "test"
        },
        isTrainingScheduled : false
      },
      {
        trainerTopicId : 2,
        userId : 2,
        topicId : 2,
        jobId : 2,
        topic : {
          topicId : 2,
          topicName : "test",
          jobId : 2
        },
        user : {
          userId : 2,
          firstName : "test"
        },
        job : {
          jobId : 2,
          jobName : "test"
        },
        isTrainingScheduled : true
      }
    ]
    const mockHttpError = {
      success: false,
      status: 500,
      statusText: 'Internal server error.',
    };
      const apiUrl = 'http://localhost:5038/api/Trainer/GetAllTrainingTopicbyTrainerId/1?page=1&pageSize=6';

    //Act

    service.getAllTrainingTopicbyTrainerId(1,1,6).subscribe({
      next : () => fail('Should have fail with the error'),
      error : (error) => {
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      }
      
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush({},mockHttpError);
  })
  //---------------------totalCountofTrainingTopicbyTrainerId--------------------
  it('should add training program details', () => {
    //Arrange
    const mockSuccessResposne: ApiResponse<number> = {
      success: true,
      data: 1,
      message: '',
    };

    const apiUrl = 'http://localhost:5038/api/Trainer/TotalCountofTrainingTopicbyTrainerId/1';

    //Act
    service.totalCountofTrainingTopicbyTrainerId(1).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');

    req.flush(mockSuccessResposne);
  });

  it('should handle error response', () => {
    //Arrange
 
    const mockSuccessResposne: ApiResponse<number> = {
      success: false,
      data: 1,
      message: '',
    };

    const apiUrl = 'http://localhost:5038/api/Trainer/TotalCountofTrainingTopicbyTrainerId/1';

    //Act
    service.totalCountofTrainingTopicbyTrainerId(1).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');

    req.flush(mockSuccessResposne);
  });

  it('should handle HTTP error response', () => {
    //Arrange
   

    const mockHttpError = {
      success: false,
      status: 500,
      statusText: 'Internal server error.',
    };

    const apiUrl = 'http://localhost:5038/api/Trainer/TotalCountofTrainingTopicbyTrainerId/1';

    //Act
    service.totalCountofTrainingTopicbyTrainerId(1).subscribe({
      next : () => fail('should have fail with error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      },
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');

    req.flush({},mockHttpError);

  });
  //---------------------addTrainingProgramDetail--------------------
  it('should add training program details', () => {
    //Arrange
    const addParticipate: AddTrainingProgramDetail = {
      trainerTopicId : 1,
      startDate : new Date,
      endDate : new Date,
      startTime : "test",
      endTime : "test",
      modePreference : "online",
      targetAudience : "test"
    };

    const mockSuccessResposne: ApiResponse<string> = {
      success: true,
      data: '',
      message: 'Program details added successfully.',
    };

    const apiUrl = 'http://localhost:5038/api/Trainer/AddTrainingProgramDetail';

    //Act
    service.addTrainingProgramDetail(addParticipate).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush(mockSuccessResposne);
  });
  it('should handle error response', () => {
    //Arrange
    const addParticipate: AddTrainingProgramDetail = {
      trainerTopicId : 1,
      startDate : new Date,
      endDate : new Date,
      startTime : "test",
      endTime : "test",
      modePreference : "online",
      targetAudience : "test"
    };

    const mockSuccessResposne: ApiResponse<string> = {
      success: false,
      data: '',
      message: 'Training program already added',
    };

    const apiUrl = 'http://localhost:5038/api/Trainer/AddTrainingProgramDetail';

    //Act
    service.addTrainingProgramDetail(addParticipate).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush(mockSuccessResposne);
  });
  it('should handle HTTP error response', () => {
    //Arrange
    const addParticipate: AddTrainingProgramDetail = {
      trainerTopicId : 1,
      startDate : new Date,
      endDate : new Date,
      startTime : "test",
      endTime : "test",
      modePreference : "online",
      targetAudience : "test"
    };

    const mockHttpError = {
      success: false,
      status: 500,
      statusText: 'Internal server error.',
    };

    const apiUrl = 'http://localhost:5038/api/Trainer/AddTrainingProgramDetail';

    //Act
    service.addTrainingProgramDetail(addParticipate).subscribe({
      next : () => fail('should have fail with error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      },
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush({},mockHttpError);

  });
  //---------------------updateTrainingProgramDetails--------------------
  it('should add training program details', () => {
    //Arrange
    const addParticipate: UpdateTrainingProgramDetail = {
      trainerProgramDetailId : 1,
      trainerTopicId : 1,
      startDate : new Date,
      endDate : new Date,
      startTime : "test",
      endTime : "test",
      modePreference : "online",
      targetAudience : "test"
    };

    const mockSuccessResposne: ApiResponse<string> = {
      success: true,
      data: '',
      message: 'Program details updated successfully.',
    };

    const apiUrl = 'http://localhost:5038/api/Trainer/UpdateTrainingProgramDetail';

    //Act
    service.updateTrainingProgramDetails(addParticipate).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('PUT');

    req.flush(mockSuccessResposne);
  });

  it('should handle error response', () => {
    //Arrange
    const addParticipate: UpdateTrainingProgramDetail = {
      trainerProgramDetailId : 1,
      trainerTopicId : 1,
      startDate : new Date,
      endDate : new Date,
      startTime : "test",
      endTime : "test",
      modePreference : "online",
      targetAudience : "test"
    };

    const mockSuccessResposne: ApiResponse<string> = {
      success: false,
      data: '',
      message: 'Training program already added',
    };

    const apiUrl = 'http://localhost:5038/api/Trainer/UpdateTrainingProgramDetail';

    //Act
    service.updateTrainingProgramDetails(addParticipate).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('PUT');

    req.flush(mockSuccessResposne);
  });
  it('should handle HTTP error response', () => {
    //Arrange
    const addParticipate: UpdateTrainingProgramDetail = {
      trainerProgramDetailId : 1,
      trainerTopicId : 1,
      startDate : new Date,
      endDate : new Date,
      startTime : "test",
      endTime : "test",
      modePreference : "online",
      targetAudience : "test"
    };

    const mockHttpError = {
      success: false,
      status: 500,
      statusText: 'Internal server error.',
    };

    const apiUrl = 'http://localhost:5038/api/Trainer/UpdateTrainingProgramDetail';

    //Act
    service.updateTrainingProgramDetails(addParticipate).subscribe({
      next : () => fail('should have fail with error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      },
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('PUT');

    req.flush({},mockHttpError);

  });
});
 