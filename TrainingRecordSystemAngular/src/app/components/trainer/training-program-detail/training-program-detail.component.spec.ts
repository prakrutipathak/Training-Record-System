import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrainingProgramDetailComponent } from './training-program-detail.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { AddTrainingProgramDetail } from 'src/app/models/add-training-program-detail.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { TrainerService } from 'src/app/services/trainer.service';

describe('TrainingProgramDetailComponent', () => {
  let component: TrainingProgramDetailComponent;
  let fixture: ComponentFixture<TrainingProgramDetailComponent>;
  let router: Router;
  let trainerSpy: jasmine.SpyObj<TrainerService>;

  beforeEach(() => {
    const trainerServiceSpy = jasmine.createSpyObj('TrainerService', ['addTrainingProgramDetail'])
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ReactiveFormsModule, RouterTestingModule.withRoutes([])],
      declarations: [TrainingProgramDetailComponent],
      providers: [
        { provide: ActivatedRoute, useValue: {params: of({ id: 1 })}},
        { provide: TrainerService, useValue: trainerServiceSpy },
      ]
    });
    fixture = TestBed.createComponent(TrainingProgramDetailComponent);
    component = fixture.componentInstance;
    trainerSpy = TestBed.inject(TrainerService) as jasmine.SpyObj<TrainerService>;
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to training topics if training program details added successfully', () => {
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
    
    trainerSpy.addTrainingProgramDetail.and.returnValue(of(mockResponse));
    component.programDetailForm.setValue(mockForm);

    // Act
    component.onSubmit();

    // Assert
    expect(trainerSpy.addTrainingProgramDetail).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/training-topics'])
  });

  it('should alert error message when adding program detail fails', () => {
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
    
    trainerSpy.addTrainingProgramDetail.and.returnValue(of(mockResponse));
    component.programDetailForm.setValue(mockForm);

    // Act
    component.onSubmit();

    // Assert
    expect(trainerSpy.addTrainingProgramDetail).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message);
  })

  it('should alert error message when add details returns http error', () => {
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
    
    trainerSpy.addTrainingProgramDetail.and.returnValue(throwError(mockError));
    component.programDetailForm.setValue(mockForm);

    // Act
    component.onSubmit();

    // Assert
    expect(trainerSpy.addTrainingProgramDetail).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message);
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
});
