import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NominateparticipantComponent } from './nominateparticipant.component';
import { ManagerService } from 'src/app/services/manager.service';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TopicService } from 'src/app/services/topic.service';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { Topic } from 'src/app/models/topic.model';
import { of, throwError } from 'rxjs';
import { TrainingTopic } from 'src/app/models/trainingTopic.model';
import { UserDetails } from 'src/app/models/userDetails.model';
import { Participants } from 'src/app/models/participants.model';
import { ReactiveFormsModule } from '@angular/forms';
import { TrainingProgramDetails } from 'src/app/models/training-program-details.model';
import { LoadTrainingProgramDetail } from 'src/app/models/trainingprogramdetails.model';

describe('NominateparticipantComponent', () => {
  let component: NominateparticipantComponent;
  let fixture: ComponentFixture<NominateparticipantComponent>;
  let managerService : jasmine.SpyObj<ManagerService>;
  let topicService : jasmine.SpyObj<TopicService>;
  let authService : jasmine.SpyObj<AuthService>;
  let router: Router;

  beforeEach(() => {
    const managerServicespy = jasmine.createSpyObj('ManagerService',['nominateParticipate','getParticipantById', 'getModeofTrainingByTopicId']);
    const authServicespy = jasmine.createSpyObj('AuthService',['getUserId','getParticipantById']);
    const topicServicespy = jasmine.createSpyObj('TopicService',['getTopicsByJobId', 'getTrainerTopicsByJobId']);
    TestBed.configureTestingModule({
      declarations: [NominateparticipantComponent],
      imports : [HttpClientTestingModule,RouterTestingModule, ReactiveFormsModule],
      providers : [
        {
          provide : ManagerService, useValue : managerServicespy
        },
        {
          provide : AuthService,useValue : authServicespy
        },
        {
          provide : TopicService, useValue : topicServicespy

        }
      
      ]

    });
    fixture = TestBed.createComponent(NominateparticipantComponent);
    component = fixture.componentInstance;
    managerService = TestBed.inject(ManagerService) as jasmine.SpyObj<ManagerService>;
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    topicService = TestBed.inject(TopicService) as jasmine.SpyObj<TopicService>;
    router = TestBed.inject(Router);
   // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });


  //------------onsubmit---------------
  

  //-----------load user detials----------

  it('should load user details successfully', () => {
    // Arrange
    let userId: number = 1;
    const mockTopics : Topic[] = [
      {
        topicId : 1,
        topicName : "test",
        jobId : 1
      },
      {
        topicId : 2,
        topicName : "test 2",
        jobId : 1
      }
    ]
    let assignedTopics: LoadTrainingProgramDetail[] = [
      // {
      //   trainerTopicId: 1,
      //   userId: 5,
      //   topicId: 1,
      //   jobId: 2,
      //   user: {
      //     userId: 5,
      //     firstName: 'Test'
      //   },
      //   topic: {
      //     topicId: 1,
      //     topicName: 'Topic 1',
      //     jobId: 2
      //   },
      //   job: {
      //     jobId: 2,
      //     jobName: 'Test'
      //   }
      // }
    ]
    const mockTopicsResponse: ApiResponse<Topic[]> = {
      success: true,
      data: mockTopics,
      message: ''
    }
    const mockAssignedTopicsResponse: ApiResponse<LoadTrainingProgramDetail[]> = {
      success: true,
      data: assignedTopics,
      message: ''
    }
    let mockUserDetails: Participants = {
      participantId : 1,
      firstName: '',
      lastName: '',
      email: '',
    
      job : {
        jobId : 1,
        jobName : "test"
      },
      jobId : 1,
     
     
      userId: 1,
      user : {
        userId: 1,
firstName : "",
lastName : "",

      }
      
    }
    let mockResponse: ApiResponse<Participants> = {
      data: mockUserDetails,
      success: true,
      message: ''
    }
   
    managerService.getParticipantById.and.returnValue(of(mockResponse));
    topicService.getTrainerTopicsByJobId.and.returnValue(of(mockAssignedTopicsResponse));
 
    // Act
    component.loadUserDetails(userId);
 
    // Assert
    expect(managerService.getParticipantById).toHaveBeenCalled();
    expect(topicService.getTrainerTopicsByJobId).toHaveBeenCalled();
    expect(component.assignedTopics).toEqual(assignedTopics);
  });
 
  it('should set console error when fetching user details fails', () => {
    // Arrange
    let userId: number = 1;
   
    let mockUserDetails: Participants = {
      participantId : 1,
      firstName: '',
      lastName: '',
      email: '',
     
      job : {
        jobId : 1,
        jobName : "test"
      },
      jobId : 1,
     
      userId: 1,
      user : {
        userId: 1,
firstName : "",
lastName : "",

      }
      
    }
    let mockResponse: ApiResponse<Participants> = {
      data: mockUserDetails,
      success: false,
      message: 'Failure'
    }
   
    spyOn(console, 'error');
   
    managerService.getParticipantById.and.returnValue(of(mockResponse));
   
    // Act
    component.loadUserDetails(userId);
 
    // Assert
    expect(managerService.getParticipantById).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed ot fetch participant details', mockResponse.message);
  });
 
  it('should set console error when user details throws http error', () => {
    // Arrange
    let userId: number = 1;
   
    let mockError = {
      error: {
        message: 'Failure'
      }
    }
   
    spyOn(console, 'error');
   
    managerService.getParticipantById.and.returnValue(throwError(mockError));
   
    // Act
    component.loadUserDetails(userId);
 
    // Assert
    expect(managerService.getParticipantById).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching participant details', mockError);
  })
  //-----------load topic----------
  it('should load topics successfully', () => {
    // Arrange
    let jobId = 1;
    let assignedTopics: LoadTrainingProgramDetail[] = [
      // {
      //   trainerTopicId: 1,
       
      
      //   tra
      //   user: {
      //     userId: 5,
      //     firstName: 'Test'
      //   },
      //   topic: {
      //     topicId: 1,
      //     topicName: 'Topic 1',
      //     jobId: 2
      //   },
      //   job: {
      //     jobId: 2,
      //     jobName: 'Test'
      //   }
      // }
    ]
    
    const mockAssignedTopicsResponse: ApiResponse<LoadTrainingProgramDetail[]> = {
      success: true,
      data: assignedTopics,
      message: ''
    }
   
    topicService.getTrainerTopicsByJobId.and.returnValue(of(mockAssignedTopicsResponse));
 
    // Act
    component.loadTopics(jobId);
 
    // Assert
    expect(topicService.getTrainerTopicsByJobId).toHaveBeenCalled();
    expect(component.assignedTopics).toEqual(assignedTopics);
    expect(component.loading).toBeFalse();
  });
 
  it('should set console error when load topics returns false', () => {
    // Arrange
    let jobId = 1;
    
    const mockAssignedTopicsResponse: ApiResponse<LoadTrainingProgramDetail[]> = {
      success: false,
      data: [],
      message: 'Failure'
    }
    spyOn(console, 'error');
   
    topicService.getTrainerTopicsByJobId.and.returnValue(of(mockAssignedTopicsResponse));
 
    // Act
    component.loadTopics(jobId);
 
    // Assert
    expect(topicService.getTrainerTopicsByJobId).toHaveBeenCalled();
    expect(component.assignedTopics).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch topics: ', mockAssignedTopicsResponse.message)
  });
 
  it('should set console error when load topics returns error response', () => {
    // Arrange
    let jobId = 1;

    const mockError = {
      error: {
        message: "Failed to get topics"
      }
    }
    spyOn(console, 'error');
   
    topicService.getTrainerTopicsByJobId.and.returnValue(throwError(mockError));
 
    // Act
    component.loadTopics(jobId);
 
    // Assert
    expect(topicService.getTrainerTopicsByJobId).toHaveBeenCalled();
    expect(component.assignedTopics).toEqual([]);
    expect(component.loading).toBeFalse();
    expect(console.error).toHaveBeenCalledWith('Error fetching topics:', mockError)
  });
  
  // -------- loadModeOfTrainingByTopicsId
  it('should set default mode preference and call patch mode preference on success', () => {
    // Arrange
    let topicId = 1;
    let userId=3;
    let mockResponse: ApiResponse<string> = {
      data: 'online',
      success: true,
      message: ''
    }
    const nominationForm = {
      participateId: 1,
      modePreference: 'online',
      topicId: 0,
      trainerId:1,
      userId:1
    }

    authService.getUserId.and.returnValue(of("1"))
    managerService.getModeofTrainingByTopicId.and.returnValue(of(mockResponse))
    fixture.detectChanges();
    component.nominationForm.setValue(nominationForm);

    // Act
    component.loadModeofTrainingByTopicId(topicId,userId)

    // Assert
    expect(authService.getUserId).toHaveBeenCalled();
    expect(managerService.getModeofTrainingByTopicId).toHaveBeenCalled();
  })

  it('should set default mode preference null when load mode of training fails', () => {
    // Arrange
    let topicId = 1;
    let userId=3;
    let mockResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Failure'
    }
    spyOn(console, 'error');

    managerService.getModeofTrainingByTopicId.and.returnValue(of(mockResponse))

    // Act
    component.loadModeofTrainingByTopicId(topicId,userId)

    // Assert
    expect(managerService.getModeofTrainingByTopicId).toHaveBeenCalled();
    expect(component.defaultModePrefrence).toBeNull();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch topics: ', mockResponse.message)
    expect(component.loading).toBeFalse();
  });

  it('should set default mode preference null when load mode returns Http error', () => {
    // Arrange
    let topicId = 1;
    let userId=3;
    let mockError = {
      error: {
        message: 'Failure'
      }
    }
    spyOn(console, 'error');

    managerService.getModeofTrainingByTopicId.and.returnValue(throwError(mockError))

    // Act
    component.loadModeofTrainingByTopicId(topicId,userId)

    // Assert
    expect(managerService.getModeofTrainingByTopicId).toHaveBeenCalled();
    expect(component.defaultModePrefrence).toBeNull();
    expect(console.error).toHaveBeenCalledWith('Error fecthing topics: ', mockError)
    expect(component.loading).toBeFalse();
  });
});
