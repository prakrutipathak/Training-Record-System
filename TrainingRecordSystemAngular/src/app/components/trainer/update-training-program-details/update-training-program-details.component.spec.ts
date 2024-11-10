import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateTrainingProgramDetailsComponent } from './update-training-program-details.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TrainerService } from 'src/app/services/trainer.service';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { AuthService } from 'src/app/services/auth-service.service';
import { TrainingProgramDetails } from 'src/app/models/training-program-details.model';

describe('UpdateTrainingProgramDetailsComponent', () => {
  let component: UpdateTrainingProgramDetailsComponent;
  let fixture: ComponentFixture<UpdateTrainingProgramDetailsComponent>;
  let router: Router;
  let trainerSpy: jasmine.SpyObj<TrainerService>;
  let authSpy: jasmine.SpyObj<AuthService>;

  beforeEach(() => {
    const trainerServiceSpy = jasmine.createSpyObj('TrainerService', ['updateTrainingProgramDetails', 'getTrainingProgramDetails'])
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['getUserId']);
    TestBed.configureTestingModule({
      declarations: [UpdateTrainingProgramDetailsComponent],
      imports:[HttpClientTestingModule, RouterTestingModule, ReactiveFormsModule],
      providers: [
        { provide: ActivatedRoute, useValue: {params: of({ id: 1 })}},
        { provide: TrainerService, useValue: trainerServiceSpy },
        { provide: AuthService, useValue: authServiceSpy },
      ]
    });
    fixture = TestBed.createComponent(UpdateTrainingProgramDetailsComponent);
    component = fixture.componentInstance;
    trainerSpy = TestBed.inject(TrainerService) as jasmine.SpyObj<TrainerService>;
    authSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router);
    const userId = "1";
    authSpy.getUserId.and.returnValue(of(userId));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set user id on init', () => {
    // Arrange
    const userId = "1";
    const mockForm = {
      startDate: '2000-01-01',
      endDate: '2000-02-01',
      startTime: 11,
      endTime: 12,
      modePreference: 'hybrid',
      targetAudience: 'test'
    }
    authSpy.getUserId.and.returnValue(of(userId));
    component.programDetailForm.setValue(mockForm);

    // Act
    fixture.detectChanges();

    // Assert
    expect(component.userId).toEqual(Number(userId))
  })

  it('should set duration when start time and end time are correct', () => {
    // Arrange
    let startTime: number = 11;
    let endTime: number = 12
    let expectedDuration = endTime - startTime + ' hours';
    component.programDetailForm.patchValue({startTime: startTime});
    component.programDetailForm.patchValue({endTime: endTime});

    // Act
    component.calculateDuration();

    // Assert
    expect(component.duration).toEqual(expectedDuration.toString())
  })

  it('should set duration 0 when invalid start time and end time', () => {
    // Arrange
    let startTime: number = 13;
    let endTime: number = 12
    let expectedDuration = '0 hours';
    component.programDetailForm.patchValue({startTime: startTime});
    component.programDetailForm.patchValue({endTime: endTime});

    // Act
    component.calculateDuration();

    // Assert
    expect(component.duration).toEqual(expectedDuration.toString())
  })

  it('should navigate to training topics if training program details are updated successfully', () => {
    // Arrange
    spyOn(window, 'alert');
    spyOn(router, 'navigate');
    const mockForm = {
      startDate: '2000-01-01',
      endDate: '2000-02-01',
      startTime: 11,
      endTime: 12,
      modePreference: 'hybrid',
      targetAudience: 'test'
    }
    let mockResponse: ApiResponse<string> = {
      data: '',
      success: true,
      message: 'Success'
    }
    
    trainerSpy.updateTrainingProgramDetails.and.returnValue(of(mockResponse));
    component.programDetailForm.setValue(mockForm);

    // Act
    component.onSubmit();

    // Assert
    expect(trainerSpy.updateTrainingProgramDetails).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/training-topics'])
  });

  it('should alert error message when updating program detail fails', () => {
    // Arrange
    spyOn(window, 'alert');
    const mockForm = {
      startDate: '2000-01-01',
      endDate: '2000-02-01',
      startTime: 11,
      endTime: 12,
      modePreference: 'hybrid',
      targetAudience: 'test'
    }
    let mockResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Failure'
    }
    
    trainerSpy.updateTrainingProgramDetails.and.returnValue(of(mockResponse));
    component.programDetailForm.setValue(mockForm);

    // Act
    component.onSubmit();

    // Assert
    expect(trainerSpy.updateTrainingProgramDetails).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message);
  })

  it('should alert error message when update details returns http error', () => {
    // Arrange
    spyOn(window, 'alert');
    const mockForm = {
      startDate: '2000-01-01',
      endDate: '2000-02-01',
      startTime: 11,
      endTime: 12,
      modePreference: 'hybrid',
      targetAudience: 'test'
    }
    let mockError = {
      error: {
        message: 'Failure'
      }
    };
    
    trainerSpy.updateTrainingProgramDetails.and.returnValue(throwError(mockError));
    component.programDetailForm.setValue(mockForm);

    // Act
    component.onSubmit();

    // Assert
    expect(trainerSpy.updateTrainingProgramDetails).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message);
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
    const mockForm = {
      startDate: '2000-01-01',
      endDate: '2000-02-01',
      startTime: 10,
      endTime: 16,
      modePreference: 'online',
      targetAudience: 'Test'
    }
    const duration = (mockForm.endTime - mockForm.startTime) + ' hours';
    spyOn(console, 'log');
    trainerSpy.getTrainingProgramDetails.and.returnValue(of(mockResponse));

    // Act
    component.loadProgramDetails()

    // Assert
    expect(trainerSpy.getTrainingProgramDetails).toHaveBeenCalled();
    expect(component.programData).toEqual(mockResponse.data);
    expect(component.startTime).toEqual(mockForm.startTime);
    expect(component.endTime).toEqual(mockForm.endTime);
    expect(component.duration).toEqual(duration.toString())
    expect(component.loading).toBeFalse();
    expect(console.log).toHaveBeenCalledWith(component.programDetailForm);
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
