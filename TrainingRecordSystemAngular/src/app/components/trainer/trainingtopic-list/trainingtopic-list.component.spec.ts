import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingtopicListComponent } from './trainingtopic-list.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth-service.service';
import { of, throwError } from 'rxjs';
import { TrainerService } from 'src/app/services/trainer.service';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { TrainingTopic } from 'src/app/models/trainingTopic.model';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';

describe('TrainingtopicListComponent', () => {
  let component: TrainingtopicListComponent;
  let fixture: ComponentFixture<TrainingtopicListComponent>;
  let authSpy: jasmine.SpyObj<AuthService>;
  let trainerSpy: jasmine.SpyObj<TrainerService>;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['getUserDetailsByUserId', 'getUserId'])
    const trainerServiceSpy = jasmine.createSpyObj('TrainerService', ['getAllTrainingTopicbyTrainerId', 'totalCountofTrainingTopicbyTrainerId'])
    TestBed.configureTestingModule({
      imports: [HttpClientModule, FormsModule, RouterTestingModule.withRoutes([])],
      declarations: [TrainingtopicListComponent],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: TrainerService, useValue: trainerServiceSpy },
      ]
    });
    fixture = TestBed.createComponent(TrainingtopicListComponent);
    component = fixture.componentInstance;
    authSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    trainerSpy = TestBed.inject(TrainerService) as jasmine.SpyObj<TrainerService>;
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set user id on init', () => {
    // Arrange
    let mockTopics: TrainingTopic[] = [
      {
        trainerTopicId: 0,
        userId: 0,
        topicId: 0,
        jobId: 0,
        user: {
          userId: 0,
          firstName: ''
        },
        topic: {
          topicId: 0,
          topicName: '',
          jobId: 0
        },
        job: {
          jobId: 0,
          jobName: ''
        },
        isTrainingScheduled:true
      },
    ]
    let count = 10;
    let mockCountResponse: ApiResponse<number> = {
      data: count,
      success: true,
      message: ''
    };
    let mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      data: mockTopics,
      message: 'Success',
      success: true,
    }
    authSpy.getUserId.and.returnValue(of("1"))
    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse))

    // Act
    fixture.detectChanges();

    // Assert
    expect(authSpy.getUserId).toHaveBeenCalled();
    expect(component.userId).toEqual(1);
    expect(component.totalItems).toEqual(mockCountResponse.data);
    expect(component.trainingTopics).toEqual(mockTopics);
  })

  it('should load topic count successfully', () => {
    // Arrange
    let count = 10;
    let mockCountResponse: ApiResponse<number> = {
      data: count,
      success: true,
      message: ''
    };

    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))

    // Act
    component.loadTopicCount(1);

    // Assert
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.totalItems).toEqual(mockCountResponse.data);
  })

  it('should set console error when load topic count fails', () => {
    // Arrange
    let mockCountResponse: ApiResponse<number> = {
      data: 0,
      success: false,
      message: 'Failed to get count'
    };
    spyOn(console, 'error');

    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))

    // Act
    component.loadTopicCount(1);

    // Assert
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch participants count', mockCountResponse.message)
    expect(component.totalItems).toEqual(mockCountResponse.data);
    expect(component.loading).toBeFalse();
  });

  it('should set console error and empty the list when load count returns Http error', () => {
    // Arrange
    let mockError = {
      error: {
        message: "Failure"
      }
    }
    
    spyOn(console, 'error');

    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(throwError(mockError))

    // Act
    component.loadTopicCount(1);

    // Assert
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error featching participants count', mockError.error)
    expect(component.totalItems).toEqual(0);
    expect(component.trainingTopics).toEqual([]);
    expect(component.loading).toBeFalse();
  });

  it('should load topics successfully', () => {
    // Arrange
    let mockTopics: TrainingTopic[] = [
      {
        trainerTopicId: 0,
        userId: 0,
        topicId: 0,
        jobId: 0,
        user: {
          userId: 0,
          firstName: ''
        },
        topic: {
          topicId: 0,
          topicName: '',
          jobId: 0
        },
        job: {
          jobId: 0,
          jobName: ''
        },
        isTrainingScheduled:true
      },
    ]
    let count = 10;
    let mockCountResponse: ApiResponse<number> = {
      data: count,
      success: true,
      message: ''
    };
    let mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      data: mockTopics,
      message: 'Success',
      success: true,
    }
    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse))

    // Act
    component.loadTopics(1, 1);

    // Assert
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.totalItems).toEqual(mockCountResponse.data);
    expect(component.trainingTopics).toEqual(mockTopics);
  });

  it('should set console error when load topics fails', () => {
    // Arrange
    let mockTopics: TrainingTopic[] = []
    let count = 10;
    let mockCountResponse: ApiResponse<number> = {
      data: count,
      success: true,
      message: ''
    };
    let mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      data: mockTopics,
      message: 'Failed to get topics',
      success: false,
    }
    spyOn(console, 'error');
    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse))

    // Act
    component.loadTopics(1, 1);

    // Assert
    expect(console.error).toHaveBeenCalledWith('Failed to participants trainer', mockTopicsResponse.message)
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.totalItems).toEqual(mockCountResponse.data);
    expect(component.loading).toBeFalse();
  })

  it('should set console error when load topics returns Http error', () => {
    // Arrange
    let count = 10;
    let mockCountResponse: ApiResponse<number> = {
      data: count,
      success: true,
      message: ''
    };
    let mockError = {
      error: {
        message: 'Failed to get topics'
      }
    }
    spyOn(console, 'error');
    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(throwError(mockError))

    // Act
    component.loadTopics(1, 1);

    // Assert
    expect(console.error).toHaveBeenCalledWith('Error participants trainer', mockError)
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.totalItems).toEqual(mockCountResponse.data);
    expect(component.trainingTopics).toBeNull();
    expect(component.loading).toBeFalse();
  })

  it('should set page number and call load topics on change page', () => {
    // Arrange
    let mockTopics: TrainingTopic[] = [
      {
        trainerTopicId: 0,
        userId: 0,
        topicId: 0,
        jobId: 0,
        user: {
          userId: 0,
          firstName: ''
        },
        topic: {
          topicId: 0,
          topicName: '',
          jobId: 0
        },
        job: {
          jobId: 0,
          jobName: ''
        },
        isTrainingScheduled:true
      },
    ]
    let count = 10;
    let mockCountResponse: ApiResponse<number> = {
      data: count,
      success: true,
      message: ''
    };
    let mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      data: mockTopics,
      message: 'Success',
      success: true,
    }
    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse))
    let pageNumber = 1;

    // Act
    component.changePage(pageNumber);

    // Assert
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.pageNumber).toEqual(pageNumber);
    expect(component.totalItems).toEqual(mockCountResponse.data);
    expect(component.trainingTopics).toEqual(mockTopics);
  })

  it('should change page size and call load topics on change page size', () => {
    // Arrange
    let mockTopics: TrainingTopic[] = [
      {
        trainerTopicId: 0,
        userId: 0,
        topicId: 0,
        jobId: 0,
        user: {
          userId: 0,
          firstName: ''
        },
        topic: {
          topicId: 0,
          topicName: '',
          jobId: 0
        },
        job: {
          jobId: 0,
          jobName: ''
        },
        isTrainingScheduled:true
      },
    ]
    let count = 10;
    let mockCountResponse: ApiResponse<number> = {
      data: count,
      success: true,
      message: ''
    };
    let mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      data: mockTopics,
      message: 'Success',
      success: true,
    }
    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse))
    let pageSize = 10;
    let expectedPages = Math.ceil(count / pageSize)

    // Act
    component.changePageSize(pageSize);

    // Assert
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.pageSize).toEqual(pageSize);
    expect(component.pageNumber).toEqual(1);
    expect(component.totalPages).toEqual(expectedPages);
    expect(component.totalItems).toEqual(mockCountResponse.data);
    expect(component.trainingTopics).toEqual(mockTopics);
  })

  it('should change page and load topics when next page is selected', () => {
    // Arrange
    let mockTopics: TrainingTopic[] = [
      {
        trainerTopicId: 0,
        userId: 0,
        topicId: 0,
        jobId: 0,
        user: {
          userId: 0,
          firstName: ''
        },
        topic: {
          topicId: 0,
          topicName: '',
          jobId: 0
        },
        job: {
          jobId: 0,
          jobName: ''
        },
        isTrainingScheduled:true
      },
    ]
    let count = 10;
    let mockCountResponse: ApiResponse<number> = {
      data: count,
      success: true,
      message: ''
    };
    let mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      data: mockTopics,
      message: 'Success',
      success: true,
    }
    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse))
    component.totalPages = 10;
    component.pageNumber = 2;
    let expectedPageNumber = component.pageNumber + 1;

    // Act
    component.nextPage();

    // Assert
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.pageNumber).toEqual(expectedPageNumber);
    expect(component.totalItems).toEqual(mockCountResponse.data);
    expect(component.trainingTopics).toEqual(mockTopics);
  })

  it('should not go to next page when on last page', () => {
    // Arrange
    component.totalPages = 1;
    component.pageNumber = 2;
    let expectedPageNumber = component.pageNumber;

    // Act
    component.nextPage();

    // Assert
    expect(component.pageNumber).toEqual(expectedPageNumber);
  })

  it('should go to previous page and call load topics when previous page is selected', () => {
    // Arrange
    let mockTopics: TrainingTopic[] = [
      {
        trainerTopicId: 0,
        userId: 0,
        topicId: 0,
        jobId: 0,
        user: {
          userId: 0,
          firstName: ''
        },
        topic: {
          topicId: 0,
          topicName: '',
          jobId: 0
        },
        job: {
          jobId: 0,
          jobName: ''
        },
        isTrainingScheduled:true
      },
    ]
    let count = 10;
    let mockCountResponse: ApiResponse<number> = {
      data: count,
      success: true,
      message: ''
    };
    let mockTopicsResponse: ApiResponse<TrainingTopic[]> = {
      data: mockTopics,
      message: 'Success',
      success: true,
    }
    trainerSpy.totalCountofTrainingTopicbyTrainerId.and.returnValue(of(mockCountResponse))
    trainerSpy.getAllTrainingTopicbyTrainerId.and.returnValue(of(mockTopicsResponse))
    component.pageNumber = 2;
    let expectedPageNumber = component.pageNumber - 1;

    // Act
    component.previousPage();

    // Assert
    expect(trainerSpy.getAllTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(trainerSpy.totalCountofTrainingTopicbyTrainerId).toHaveBeenCalled();
    expect(component.pageNumber).toEqual(expectedPageNumber);
    expect(component.totalItems).toEqual(mockCountResponse.data);
    expect(component.trainingTopics).toEqual(mockTopics);
  })

  it('should not go to previous page when page 1 is selected', () => {
    // Arrange
    component.pageNumber = 1;
    let expectedPageNumber = component.pageNumber;

    // Act
    component.previousPage();

    // Assert
    expect(component.pageNumber).toEqual(expectedPageNumber);
  })
});
