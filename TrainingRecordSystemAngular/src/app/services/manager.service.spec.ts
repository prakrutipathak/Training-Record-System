import { TestBed } from '@angular/core/testing';

import { ManagerService } from './manager.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiResponse } from '../models/ApiResponse{T}.model';
import { Participants } from '../models/participants.model';
import { NominateParticipants } from '../models/nominatepartipate.model';
import { AddParticipants } from '../models/addParticipate.model';
import { UpcomingTraining } from '../models/upcomingtraining.model';
import { Component } from '@angular/core';

describe('ManagerService', () => {
  let service: ManagerService;
  let httpMock :HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule],
      providers : [ManagerService]
    });
    service = TestBed.inject(ManagerService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  //---------------------------------loadTrainingProgram-------------------
  it('should load training program suessfully when jobid is not null', () => {
    //Arrange
    const jobId = 1;
    const Participants : UpcomingTraining[] = [
      {
        trainerName : "test",
        jobName : "test",
        topicName : "test",
        startDate:'2024-08-12',
        endDate:'2024-08-31'
      },
      {
        trainerName : "test 1",
        jobName : "test 1",
        topicName : "test 1",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      }
    ]

    const mockApiResposne: ApiResponse<UpcomingTraining[]> = {
      success: true,
      data: Participants,
      message: '',
    };
    //Act
    service.loadTrainingProgram(jobId).subscribe((response) => {
      expect(response).toBe(mockApiResposne);
      expect(response.data.length).toEqual(mockApiResposne.data.length);
      expect(response.data).toEqual(mockApiResposne.data);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/UpcomingTrainingProgram?jobId=" + jobId);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResposne);
  });
  it('should handle error training program when jobid is not null', () => {
    //Arrange
    const jobId = 1;
 

    const mockApiResposne: ApiResponse<UpcomingTraining[]> = {
      success: false,
      data: {} as UpcomingTraining[],
      message: 'Error fetching data',
    };
    //Act
    service.loadTrainingProgram(jobId).subscribe((response) => {
      expect(response).toBe(mockApiResposne);
      expect(response.data.length).toEqual(mockApiResposne.data.length);
      expect(response.data).toEqual(mockApiResposne.data);
      expect(response.message).toEqual(mockApiResposne.message);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/UpcomingTrainingProgram?jobId=" + jobId);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResposne);
  });
  it('should  handle HTTP error when jobid is not null', () => {
    //Arrange
    const jobId = 1;
    const mockHttpError = {
      success: false,
      status: 500,
      statusText: 'Internal server error.',
    };
    //Act
    service.loadTrainingProgram(jobId).subscribe({
      next: () => fail('should have fail with the error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      },
    });


    const req = httpMock.expectOne("http://localhost:5038/api/Manager/UpcomingTrainingProgram?jobId=" + jobId);
    expect(req.request.method).toBe('GET');

    req.flush({},mockHttpError);
  });

  it('should load training program suessfully when jobid is null', () => {
    //Arrange
    const Participants : UpcomingTraining[] = [
      {
        trainerName : "test",
        jobName : "test",
        topicName : "test",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      },
      {
        trainerName : "test 1",
        jobName : "test 1",
        topicName : "test 1",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      }
    ]

    const mockApiResposne: ApiResponse<UpcomingTraining[]> = {
      success: true,
      data: Participants,
      message: '',
    };
    //Act
    service.loadTrainingProgram(null).subscribe((response) => {
      expect(response).toBe(mockApiResposne);
      expect(response.data.length).toEqual(mockApiResposne.data.length);
      expect(response.data).toEqual(mockApiResposne.data);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/UpcomingTrainingProgram");
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResposne);
  });
  it('should handle error training program  when jobid is null', () => {
    //Arrange
 

    const mockApiResposne: ApiResponse<UpcomingTraining[]> = {
      success: false,
      data: {} as UpcomingTraining[],
      message: '',
    };
    //Act
    service.loadTrainingProgram(null).subscribe((response) => {
      expect(response).toBe(mockApiResposne);
      expect(response.data.length).toEqual(mockApiResposne.data.length);
      expect(response.data).toEqual(mockApiResposne.data);
      expect(response.message).toEqual(mockApiResposne.message);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/UpcomingTrainingProgram");
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResposne);
  });
  it('should  handle HTTP error suessfully when jobid is null', () => {
    //Arrange
    const mockHttpError = {
      success: false,
      status: 500,
      statusText: 'Internal server error.',
    };
    //Act
    service.loadTrainingProgram(null).subscribe({
      next: () => fail('should have fail with the error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      },
    });


    const req = httpMock.expectOne("http://localhost:5038/api/Manager/UpcomingTrainingProgram");
    expect(req.request.method).toBe('GET');

    req.flush({},mockHttpError);
  });
  //---------------------------------add Participate-------------------
  it('should nominate participant successfully', () => {
    //Arrange
    const addParticipate: AddParticipants = {
      userId : 1,
      firstName : "test",
      lastName  : "test",
      jobId : 1,
      email : "test@gmail.com"
    };

    const mockSuccessResposne: ApiResponse<string> = {
      success: true,
      data: '',
      message: 'Participant added successfully.',
    };

    const apiUrl = 'http://localhost:5038/api/Manager/AddParticipate';

    //Act
    service.addParticipate(addParticipate).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush(mockSuccessResposne);
  });

  it('should handle failed nomination', () => {
    //Arrange
    const addParticipate: AddParticipants = {
      userId : 1,
      firstName : "test",
      lastName  : "test",
      jobId : 1,
      email : "test@gmail.com"
    };
    const mockErrorResposne: ApiResponse<string> = {
      success: false,
      data: '',
      message: 'Participant already nominated',
    };
    const apiUrl = 'http://localhost:5038/api/Manager/AddParticipate';


    //Act
    service.addParticipate(addParticipate).subscribe((response) => {
      expect(response).toBe(mockErrorResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush(mockErrorResposne);
  });

  it('should handle http Error', () => {
    //Arrange
    const addParticipate: AddParticipants = {
      userId : 1,
      firstName : "test",
      lastName  : "test",
      jobId : 1,
      email : "test@gmail.com"
    };
    const mockHttpError = {
      success: false,
      status: 500,
      statusText: 'Internal server error.',
    };

    const apiUrl = 'http://localhost:5038/api/Manager/AddParticipate';


    //Act
    service.addParticipate(addParticipate).subscribe({
      next: () => fail('should have fail with the error'),
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


  //---------------------------------nominateParticipate-------------------
  it('should nominate participant successfully', () => {
    //Arrange
    const nominateParticipate: NominateParticipants = {
      trainerId : 1,
      participateId : 1,
      modePreference : "offline",
      topicId : 1,
      userId:1,
    };

    const mockSuccessResposne: ApiResponse<string> = {
      success: true,
      data: '',
      message: 'Partiipant nominated successfully.',
    };

    const apiUrl = 'http://localhost:5038/api/Manager/NominateParticipate';

    //Act
    service.nominateParticipate(nominateParticipate).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush(mockSuccessResposne);
  });

  it('should handle failed nomination', () => {
    //Arrange
    const nominateParticipate: NominateParticipants = {
      userId : 1,
      participateId : 1,
      modePreference : "offline",
      topicId : 1,
      trainerId : 1,
    };
    const mockErrorResposne: ApiResponse<string> = {
      success: false,
      data: '',
      message: 'Participant already nominated',
    };
    const apiUrl = 'http://localhost:5038/api/Manager/NominateParticipate';


    //Act
    service.nominateParticipate(nominateParticipate).subscribe((response) => {
      expect(response).toBe(mockErrorResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush(mockErrorResposne);
  });

  it('should handle http Error', () => {
    //Arrange
    const nominateParticipate: NominateParticipants = {
      userId : 1,
      participateId : 1,
      modePreference : "offline",
      topicId : 1,
      trainerId : 1,
    };
    const mockHttpError = {
      success: false,
      status: 500,
      statusText: 'Internal server error.',
    };

    const apiUrl = 'http://localhost:5038/api/Manager/NominateParticipate';


    //Act
    service.nominateParticipate(nominateParticipate).subscribe({
      next: () => fail('should have fail with the error'),
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

  //---------------------------------Get partiipant by manager id-------------------
  it('should fetch partiipant by manager id successfully', () => {
    //Arrange
    const participantId = 1;
    const Participants : Participants[] = [
      {
        participantId : 1,
        firstName : "test",
        lastName : "test",
        email : "test",
       
        userId : 1,
        jobId : 1,
       
        user : {
          userId : 1,
          firstName : "test",
          lastName : "test"
        },
        job : {
          jobId : 1,
          jobName : "test"
        },
       
      },
      {
        participantId: 1,
        firstName : "test1",
        lastName : "test1",
        email : "test1",
       
        userId : 1,
        jobId : 1,
        
        user : {
          userId : 1,
          firstName : "test",
          lastName : "test"
        },
        job : {
          jobId : 1,
          jobName : "test"
        },
      
      }
    ]

    const mockApiResposne: ApiResponse<Participants[]> = {
      success: true,
      data: Participants,
      message: '',
    };
    //Act
    service.getAllPartiipantByManagerId(participantId).subscribe((response) => {
      expect(response).toBe(mockApiResposne);
      expect(response.data.length).toEqual(mockApiResposne.data.length);
      expect(response.data).toEqual(mockApiResposne.data);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/GetParticipantByManagerId/" + participantId);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResposne);
  });

  it('should handle failed by manager id participant retrival', () => {
    //Arrange
    const participantId = 1;

    const mockErrorResposne: ApiResponse<Participants[]> = {
      success: false,
      data: {} as Participants[],
      message: 'No record found!',
    };
    //Act
    service.getAllPartiipantByManagerId(participantId).subscribe((response) => {
      expect(response).toBe(mockErrorResposne);
      expect(response.data).toEqual(mockErrorResposne.data);
      expect(response.message).toEqual(mockErrorResposne.message);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/GetParticipantByManagerId/" + participantId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResposne);
  });

  it('should handle http error by manager id', () => {
    //Arrange
    const participantId = 1;
    const mockHttpError = {
      status: 500,
      statusText: 'Internal server error.',
    };

    //Act
    service.getAllPartiipantByManagerId(participantId).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      },
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/GetParticipantByManagerId/" + participantId);
    expect(req.request.method).toBe('GET');
    req.flush({},mockHttpError);
  });
  //---------------------------------Get partiipant by  id-------------------
  it('should fetch partiipant by id successfully', () => {
    //Arrange
    const participantId = 1;
    const Participants : Participants = 
      {
        participantId : 1,
        firstName : "test",
        lastName : "test",
        email : "test",
       
        userId : 1,
        jobId : 1,
       
        user : {
          userId : 1,
          firstName : "test",
          lastName : "test"
        },
        job : {
          jobId : 1,
          jobName : "test"
        },
       
      };

    const mockApiResposne: ApiResponse<Participants> = {
      success: true,
      data: Participants,
      message: '',
    };
    //Act
    service.getParticipantById(participantId).subscribe((response) => {
      expect(response).toBe(mockApiResposne);
      expect(response.data).toEqual(mockApiResposne.data);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/GetParticipantById/" + participantId);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResposne);
  });

  it('should handle failed participant retrival', () => {
    //Arrange
    const participantId = 1;

    const mockErrorResposne: ApiResponse<Participants> = {
      success: false,
      data: {} as Participants,
      message: 'No record found!',
    };
    //Act
    service.getParticipantById(participantId).subscribe((response) => {
      expect(response).toBe(mockErrorResposne);
      expect(response.data).toEqual(mockErrorResposne.data);
      expect(response.message).toEqual(mockErrorResposne.message);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/GetParticipantById/" + participantId);
    expect(req.request.method).toBe('GET');
    req.flush(mockErrorResposne);
  });

  it('should handle http error', () => {
    //Arrange
    const participantId = 1;
    const mockHttpError = {
      status: 500,
      statusText: 'Internal server error.',
    };

    //Act
    service.getParticipantById(participantId).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      },
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Manager/GetParticipantById/" + participantId);
    expect(req.request.method).toBe('GET');
    req.flush({},mockHttpError);
  });

});
