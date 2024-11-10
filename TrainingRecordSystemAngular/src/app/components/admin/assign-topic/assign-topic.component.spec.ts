import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignTopicComponent } from './assign-topic.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { AdminService } from 'src/app/services/admin.service';
import { Topic } from 'src/app/models/topic.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { TopicService } from 'src/app/services/topic.service';
import { of, throwError } from 'rxjs';
import { TrainerService } from 'src/app/services/trainer.service';
import { TrainingTopic } from 'src/app/models/trainingTopic.model';
import { UserDetails } from 'src/app/models/userDetails.model';
import { AuthService } from 'src/app/services/auth-service.service';
import { ActivatedRoute, Router } from '@angular/router';

describe('AssignTopicComponent', () => {
  let component: AssignTopicComponent;
  let fixture: ComponentFixture<AssignTopicComponent>;
  let authSpy: jasmine.SpyObj<AuthService>;
  let adminSpy: jasmine.SpyObj<AdminService>;
  let topicSpy: jasmine.SpyObj<TopicService>;
  let trainerSpy: jasmine.SpyObj<TrainerService>;
  let router: Router;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['getUserDetailsByUserId'])
    const adminServiceSpy = jasmine.createSpyObj('AdminService', ['assignTopicToTrainer', 'unassignTopic']);
    const topicServiceSpy = jasmine.createSpyObj('TopicService', ['getTopicsByJobId']);
    const trainerServiceSpy = jasmine.createSpyObj('TrainerService', ['getAllTrainingTopicbyTrainerId'])
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, FormsModule, RouterTestingModule.withRoutes([])],
      declarations: [AssignTopicComponent],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: AdminService, useValue: adminServiceSpy },
        { provide: TopicService, useValue: topicServiceSpy },
        { provide: TrainerService, useValue: trainerServiceSpy },
        { provide: ActivatedRoute, useValue: {params: of({ id: 1 })}}
      ]
    });
    fixture = TestBed.createComponent(AssignTopicComponent);
    component = fixture.componentInstance;
    authSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    adminSpy = TestBed.inject(AdminService) as jasmine.SpyObj<AdminService>;
    topicSpy = TestBed.inject(TopicService) as jasmine.SpyObj<TopicService>;
    trainerSpy = TestBed.inject(TrainerService) as jasmine.SpyObj<TrainerService>;
    router = TestBed.inject(Router);
    // fixture.detectChanges();
  });

  const mockTopics: Topic[] = [
    {
      topicId: 1,
      topicName: 'Topic 1',
      jobId: 1
    },
    {
      topicId: 2,
      topicName: 'Topic 2',
      jobId: 2
    },
  ]

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load user details on init', () => {
    // Arrange
    let userId: number = 1;
    let assignedTopics: TrainingTopic[] = [
      {
        trainerTopicId: 1,
        userId: 5,
        topicId: 1,
        jobId: 2,
        user: {
          userId: 5,
          firstName: 'Test'
        },
        topic: {
          topicId: 1,
          topicName: 'Topic 1',
          jobId: 2
        },
        job: {
          jobId: 2,
          jobName: 'Test'
        },
        isTrainingScheduled:true
      }
    ]
    const mockTopicsResponse: ApiResponse<Topic[]> = {
      success: true,
      data: mockTopics,
      message: ''
    }
    const mockAssignedTopicsResponse: ApiResponse<TrainingTopic[]> = {
      success: true,
      data: assignedTopics,
      message: ''
    }
    let mockUserDetails: UserDetails = {
      userId: 0,
      loginId: '',
      firstName: '',
      lastName: '',
      email: '',
      role: 0,
      jobId: 0
    }
    let mockResponse: ApiResponse<UserDetails> = {
      data: mockUserDetails,
      success: true,
      message: ''
    }
    
    authSpy.getUserDetailsByUserId.and.returnValue(of(mockResponse));
    topicSpy.getTopicsByJobId.and.returnValue(of(mockTopicsResponse));
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockAssignedTopicsResponse))

    // Act
    fixture.detectChanges();

    // Assert
    expect(authSpy.getUserDetailsByUserId).toHaveBeenCalled();
    expect(component.userDetails).toEqual(mockResponse.data);
  })

  it('should load topics successfully', () => {
    // Arrange
    let jobId: number = 1;
    const mockResponse: ApiResponse<Topic[]> = {
      success: true,
      data: mockTopics,
      message: ''
    }
    
    topicSpy.getTopicsByJobId.and.returnValue(of(mockResponse));

    // Act
    component.loadTopics(jobId);

    expect(topicSpy.getTopicsByJobId).toHaveBeenCalled();
    expect(component.topics).toEqual(mockResponse.data);
    expect(component.loading).toBeFalse();
  });

  it('should set console error when load topics returns false', () => {
    // Arrange
    let jobId: number = 1;
    const mockResponse: ApiResponse<Topic[]> = {
      success: false,
      message: '',
      data: []
    }
    spyOn(console, 'error');
    
    topicSpy.getTopicsByJobId.and.returnValue(of(mockResponse));

    // Act
    component.loadTopics(jobId);

    expect(topicSpy.getTopicsByJobId).toHaveBeenCalled();
    expect(component.topics).toEqual(mockResponse.data);
    expect(component.loading).toBeFalse();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch topics: ', mockResponse.message)
  });

  it('should set console error when load topics returns error response', () => {
    // Arrange
    let jobId: number = 1;
    const mockHttpError = {
      error: {
        message: 'Error fetching details',
      }
    }
    spyOn(console, 'error');
    
    topicSpy.getTopicsByJobId.and.returnValue(throwError(mockHttpError));

    // Act
    component.loadTopics(jobId);

    expect(topicSpy.getTopicsByJobId).toHaveBeenCalled();
    expect(component.topics).toEqual(null);
    expect(component.loading).toBeFalse();
    expect(console.error).toHaveBeenCalledWith('Error fetching topics: ', mockHttpError.error.message)
  });

  it('should load assigned topics successfully', () => {
    // Arrange
    let userId: number = 5;
    let assignedTopics: TrainingTopic[] = [
      {
        trainerTopicId: 1,
        userId: 5,
        topicId: 1,
        jobId: 2,
        user: {
          userId: 5,
          firstName: 'Test'
        },
        topic: {
          topicId: 1,
          topicName: 'Topic 1',
          jobId: 2
        },
        job: {
          jobId: 2,
          jobName: 'Test'
        },
        isTrainingScheduled:true
      }
    ]
    const mockResponse: ApiResponse<TrainingTopic[]> = {
      success: true,
      data: assignedTopics,
      message: ''
    }
    
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockResponse));

    // Act
    component.loadAssignedTopics(userId);

    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.assignedTopics).toEqual(mockResponse.data);
    expect(component.loading).toBeFalse();
  });

  it('should set console error when failed to fetch assigned topics', () => {
    // Arrange
    let userId: number = 5;
    const mockResponse: ApiResponse<TrainingTopic[]> = {
      success: false,
      data: [],
      message: 'Error'
    }
    spyOn(console, 'error');
    
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockResponse));

    // Act
    component.loadAssignedTopics(userId);

    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch topics: ', mockResponse.message)
    expect(component.assignedTopics).toEqual(mockResponse.data);
    expect(component.loading).toBeFalse();
  });

  it('should set console error when load assigned topics returns error response', () => {
    // Arrange
    let userId: number = 5;
    const mockHttpError = {
      error: {
        message: 'Error fetching details',
      }
    }
    spyOn(console, 'error');
    
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(throwError(mockHttpError));

    // Act
    component.loadAssignedTopics(userId);

    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching topics: ', mockHttpError.error.message)
    expect(component.assignedTopics).toEqual([]);
    expect(component.loading).toBeFalse();
  });

  it('should load user details successfully', () => {
    // Arrange
    let userId: number = 1;
    let assignedTopics: TrainingTopic[] = [
      {
        trainerTopicId: 1,
        userId: 5,
        topicId: 1,
        jobId: 2,
        user: {
          userId: 5,
          firstName: 'Test'
        },
        topic: {
          topicId: 1,
          topicName: 'Topic 1',
          jobId: 2
        },
        job: {
          jobId: 2,
          jobName: 'Test'
        },
        isTrainingScheduled:true
      }
    ]
    const mockTopicsResponse: ApiResponse<Topic[]> = {
      success: true,
      data: mockTopics,
      message: ''
    }
    const mockAssignedTopicsResponse: ApiResponse<TrainingTopic[]> = {
      success: true,
      data: assignedTopics,
      message: ''
    }
    let mockUserDetails: UserDetails = {
      userId: 0,
      loginId: '',
      firstName: '',
      lastName: '',
      email: '',
      role: 0,
      jobId: 0
    }
    let mockResponse: ApiResponse<UserDetails> = {
      data: mockUserDetails,
      success: true,
      message: ''
    }
    
    authSpy.getUserDetailsByUserId.and.returnValue(of(mockResponse));
    topicSpy.getTopicsByJobId.and.returnValue(of(mockTopicsResponse));
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockAssignedTopicsResponse))

    // Act
    component.loadUserDetails(userId);

    // Assert
    expect(authSpy.getUserDetailsByUserId).toHaveBeenCalled();
    expect(component.userDetails).toEqual(mockResponse.data);
  });

  it('should set console error when fetching user details fails', () => {
    // Arrange
    let userId: number = 1;
    
    let mockUserDetails: UserDetails = {
      userId: 0,
      loginId: '',
      firstName: '',
      lastName: '',
      email: '',
      role: 0,
      jobId: 0
    }
    let mockResponse: ApiResponse<UserDetails> = {
      data: mockUserDetails,
      success: false,
      message: 'Failure'
    }
    
    spyOn(console, 'error');
    
    authSpy.getUserDetailsByUserId.and.returnValue(of(mockResponse));
    
    // Act
    component.loadUserDetails(userId);

    // Assert
    expect(authSpy.getUserDetailsByUserId).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch user details: ', mockResponse.message);
  });

  it('should set console error when user details throws http error', () => {
    // Arrange
    let userId: number = 1;
    
    let mockUserDetails: UserDetails = {
      userId: 0,
      loginId: '',
      firstName: '',
      lastName: '',
      email: '',
      role: 0,
      jobId: 0
    }
    let mockError = {
      error: {
        message: 'Failure'
      }
    }
    
    spyOn(console, 'error');
    
    authSpy.getUserDetailsByUserId.and.returnValue(throwError(mockError));
    
    // Act
    component.loadUserDetails(userId);

    // Assert
    expect(authSpy.getUserDetailsByUserId).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching user details ', mockError.error.message);
  })

  it('should set alert message when assigned successfully', () => {
    // Arrange
    let mockResponse: ApiResponse<string> = {
      data: '',
      success: true,
      message: 'Success'
    }
    let assignedTopics: TrainingTopic[] = [
      {
        trainerTopicId: 1,
        userId: 5,
        topicId: 1,
        jobId: 2,
        user: {
          userId: 5,
          firstName: 'Test'
        },
        topic: {
          topicId: 1,
          topicName: 'Topic 1',
          jobId: 2
        },
        job: {
          jobId: 2,
          jobName: 'Test'
        },
        isTrainingScheduled:true
      }
    ]
    const mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      success: true,
      data: assignedTopics,
      message: ''
    }
    
    spyOn(window, 'alert');
    
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse));
    adminSpy.assignTopicToTrainer.and.returnValue(of(mockResponse));
    
    // Act
    component.assignTopicToTrainer();

    // Assert
    expect(adminSpy.assignTopicToTrainer).toHaveBeenCalled();
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message);
    expect(component.loading).toBeFalse();
  });

  it('should alert error message when failed to assign topic', () => {
    // Arrange
    let mockResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Failure'
    }
    spyOn(window, 'alert');

    adminSpy.assignTopicToTrainer.and.returnValue(of(mockResponse));
    
    // Act
    component.assignTopicToTrainer();

    // Assert
    expect(adminSpy.assignTopicToTrainer).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledOnceWith('Failed to assign topic: ' + mockResponse.message);
    expect(component.loading).toBeFalse();
  });

  it('should alert error message when assign topic returns http error', () => {
    // Arrange
    let mockErrorResponse = {
      error: {
        message: 'Failure'
      }
    }
    spyOn(window, 'alert');

    adminSpy.assignTopicToTrainer.and.returnValue(throwError(mockErrorResponse));
    
    // Act
    component.assignTopicToTrainer();

    // Assert
    expect(adminSpy.assignTopicToTrainer).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledOnceWith('Failed to assign topic: ' + mockErrorResponse.error.message);
    expect(component.loading).toBeFalse();
  })

  it('should call unassign topic when user confirms', () => {
    // Arrange
    const topicId = 1;
    const mockResponse: ApiResponse<string> = {
      data: '',
      success: true,
      message: 'Successfully unassigned topics'
    }
    const mockUserDetails: UserDetails = {
      userId: 1,
      loginId: 'Test',
      firstName: 'First',
      lastName: 'Last',
      email: 'test@example.com',
      role: 1,
      jobId: 1
    }
    const mockTopics: Topic[] | null = [
      {
        topicId: 1,
        topicName: 'Topic 1',
        jobId: 1
      },
      {
        topicId: 2,
        topicName: 'Topic 2',
        jobId: 1
      },
      {
        topicId: 3,
        topicName: 'Topic 3',
        jobId: 1
      },
    ]
    const assignedTopics: TrainingTopic[] = [
      {
        trainerTopicId: 1,
        userId: 5,
        topicId: 1,
        jobId: 2,
        user: {
          userId: 5,
          firstName: 'Test'
        },
        topic: {
          topicId: 1,
          topicName: 'Topic 1',
          jobId: 2
        },
        job: {
          jobId: 2,
          jobName: 'Test'
        },
        isTrainingScheduled:true
      }
    ]
    const mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      success: true,
      data: assignedTopics,
      message: ''
    }
    
    component.userDetails = mockUserDetails;
    component.topics = mockTopics;
    spyOn(window, 'confirm').and.callFake(function() { return true; });
    spyOn(window, 'alert')
    
    adminSpy.unassignTopic.and.returnValue(of(mockResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse));

    // Act
    component.confirmUnassign(topicId);

    // Assert
    expect(window.confirm).toHaveBeenCalled();
    expect(adminSpy.unassignTopic).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalled();
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.loading).toBeFalse();
  });

  it('should not call unassign topic when user cancels confirm', () => {
    // Arrange
    const topicId = 1;
    const mockTopics: Topic[] | null = [
      {
        topicId: 1,
        topicName: 'Topic 1',
        jobId: 1
      },
      {
        topicId: 2,
        topicName: 'Topic 2',
        jobId: 1
      },
      {
        topicId: 3,
        topicName: 'Topic 3',
        jobId: 1
      },
    ]
    component.topics = mockTopics;
    spyOn(window, 'confirm').and.callFake(function() { return false; });

    // Act
    component.confirmUnassign(topicId);

    // Assert
    expect(window.confirm).toHaveBeenCalled();
    expect(adminSpy.unassignTopic).toHaveBeenCalledTimes(0);
  });

  it('should set alert message when topic unassigned successfully', () => {
    // Arrange
    const topicId = 1;
    const userId = 1;
    const assignedTopics: TrainingTopic[] = [
      {
        trainerTopicId: 1,
        userId: 5,
        topicId: 1,
        jobId: 2,
        user: {
          userId: 5,
          firstName: 'Test'
        },
        topic: {
          topicId: 1,
          topicName: 'Topic 1',
          jobId: 2
        },
        job: {
          jobId: 2,
          jobName: 'Test'
        },
        isTrainingScheduled:true
      }
    ]
    const mockUserDetails: UserDetails = {
      userId: userId,
      loginId: 'Test',
      firstName: 'First',
      lastName: 'Last',
      email: 'test@example.com',
      role: 1,
      jobId: 1
    }
    const mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      success: true,
      data: assignedTopics,
      message: ''
    }
    const mockResponse: ApiResponse<string> = {
      data: '',
      success: true,
      message: 'Topic unassigned successfully'
    }
    component.userDetails = mockUserDetails;
    
    spyOn(window, 'alert')
    
    adminSpy.unassignTopic.and.returnValue(of(mockResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse));

    // Act
    component.unassignTopic(userId, topicId);

    // Assert
    expect(adminSpy.unassignTopic).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message);
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.loading).toBeFalse();
  });

  it('should set alert message when unassign topic fails', () => {
    // Arrange
    const topicId = 1;
    const userId = 1;
    const mockResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Failed to unassign topic'
    }
    
    spyOn(window, 'alert')
    
    adminSpy.unassignTopic.and.returnValue(of(mockResponse))

    // Act
    component.unassignTopic(userId, topicId);

    // Assert
    expect(adminSpy.unassignTopic).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('Failed to unassign topic: ' + mockResponse.message);
    expect(component.loading).toBeFalse();
  });

  it('should set alert message when unassign topic throws Http error', () => {
    // Arrange
    const topicId = 1;
    const userId = 1;
    const mockError = {
      error: {
        message: 'Something went wrong'
      }
    }
    
    spyOn(window, 'alert')
    
    adminSpy.unassignTopic.and.returnValue(throwError(mockError))

    // Act
    component.unassignTopic(userId, topicId);

    // Assert
    expect(adminSpy.unassignTopic).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith('Failed to unassign topic: ' + mockError.error.message);
    expect(component.loading).toBeFalse();
  })
});
