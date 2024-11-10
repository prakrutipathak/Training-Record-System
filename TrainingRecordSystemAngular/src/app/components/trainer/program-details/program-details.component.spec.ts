import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramDetailsComponent } from './program-details.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TrainerService } from 'src/app/services/trainer.service';
import { AuthService } from 'src/app/services/auth-service.service';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { TrainingProgramDetails } from 'src/app/models/training-program-details.model';
import { ActivatedRoute } from '@angular/router';

describe('ProgramDetailsComponent', () => {
  let component: ProgramDetailsComponent;
  let fixture: ComponentFixture<ProgramDetailsComponent>;
  let trainerSpy: jasmine.SpyObj<TrainerService>;
  let authSpy: jasmine.SpyObj<AuthService>;

  beforeEach(() => {
    const trainerServiceSpy = jasmine.createSpyObj('TrainerService', ['getTrainingProgramDetails'])
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['getUserId']);
    TestBed.configureTestingModule({
      declarations: [ProgramDetailsComponent],
      imports:[HttpClientTestingModule,RouterTestingModule],
      providers: [
        { provide: ActivatedRoute, useValue: {params: of({ id: 1 })}},
        { provide: TrainerService, useValue: trainerServiceSpy },
        { provide: AuthService, useValue: authServiceSpy },
      ]
    });
    fixture = TestBed.createComponent(ProgramDetailsComponent);
    component = fixture.componentInstance;
    trainerSpy = TestBed.inject(TrainerService) as jasmine.SpyObj<TrainerService>;
    authSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get user id on init', () => {
    // Arrange
    const userId = "1";
    const mockResponse: ApiResponse<TrainingProgramDetails> = {
      data: {
        trainerProgramDetailId: 1,
        startDate: new Date('2000-01-01'),
        endDate: new Date('2000-02-01'),
        startTime: '2024-07-22T10:00:00',
        endTime: '2024-07-22T16:00:00',
        duration: 6,
        modePreference: 'online',
        targetAudience: 'Test',
        trainerTopicId: 5,
        trainerTopic: {
          trainerTopicId: 5,
          userId: 4,
          topicId: 1,
          jobId: 1
        }
      },
      success: true,
      message: 'Details found'
    }
    trainerSpy.getTrainingProgramDetails.and.returnValue(of(mockResponse));
    authSpy.getUserId.and.returnValue(of(userId))

    // Act
    fixture.detectChanges();

    // Assert
    expect(authSpy.getUserId).toHaveBeenCalled();
    expect(component.userId).toEqual(Number(userId));
    expect(component.topicId).toEqual(1);
    expect(trainerSpy.getTrainingProgramDetails).toHaveBeenCalled();
  })

  it('should load program details successfully', () => {
    // Arrange
    const mockResponse: ApiResponse<TrainingProgramDetails> = {
      data: {
        trainerProgramDetailId: 1,
        startDate: new Date('2000-01-01'),
        endDate: new Date('2000-02-01'),
        startTime: '2024-07-22T10:00:00',
        endTime: '2024-07-22T16:00:00',
        duration: 6,
        modePreference: 'online',
        targetAudience: 'Test',
        trainerTopicId: 5,
        trainerTopic: {
          trainerTopicId: 5,
          userId: 4,
          topicId: 1,
          jobId: 1
        }
      },
      success: true,
      message: 'Details found'
    }
    spyOn(console, 'log');
    trainerSpy.getTrainingProgramDetails.and.returnValue(of(mockResponse));

    // Act
    component.loadProgramDetails()

    // Assert
    expect(trainerSpy.getTrainingProgramDetails).toHaveBeenCalled();
    expect(component.programData).toEqual(mockResponse.data);
    expect(component.loading).toBeFalse();
  });

  it('should set console error when fetching program details fails', () => {
    // Arrange
    const mockResponse: ApiResponse<TrainingProgramDetails> = {
      data: {
        trainerProgramDetailId: 0,
        startDate: new Date(),
        endDate: new Date(),
        startTime: '',
        endTime: '',
        duration: 0,
        modePreference: '',
        targetAudience: '',
        trainerTopicId: 0,
        trainerTopic: {
          trainerTopicId: 0,
          userId: 0,
          topicId: 0,
          jobId: 0
        }
      },
      success: false,
      message: 'Failed to fetch program details'
    }
    spyOn(console, 'error');

    trainerSpy.getTrainingProgramDetails.and.returnValue(of(mockResponse));

    // Act
    component.loadProgramDetails()

    // Assert
    expect(trainerSpy.getTrainingProgramDetails).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch program details: ', mockResponse.message);
    expect(component.loading).toBeFalse();
  })

  it('should set console error when load details throws Http error', () => {
    // Arrange
    const mockError = {
      error: {
        message: 'Error fetching details'
      }
    }
    spyOn(console, 'error');

    trainerSpy.getTrainingProgramDetails.and.returnValue(throwError(mockError));

    // Act
    component.loadProgramDetails()

    // Assert
    expect(trainerSpy.getTrainingProgramDetails).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching program details: ', mockError.error.message);
    expect(component.loading).toBeFalse();
  })
});
