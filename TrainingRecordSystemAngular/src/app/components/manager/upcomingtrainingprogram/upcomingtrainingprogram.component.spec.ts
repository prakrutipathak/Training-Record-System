import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpcomingtrainingprogramComponent } from './upcomingtrainingprogram.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ManagerService } from 'src/app/services/manager.service';
import { AdminService } from 'src/app/services/admin.service';
import { AuthService } from 'src/app/services/auth-service.service';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { Job } from 'src/app/models/job.model';
import { of, throwError } from 'rxjs';
import { UpcomingTraining } from 'src/app/models/upcomingtraining.model';

describe('UpcomingtrainingprogramComponent', () => {
  let component: UpcomingtrainingprogramComponent;
  let fixture: ComponentFixture<UpcomingtrainingprogramComponent>;
  let managerService : jasmine.SpyObj<ManagerService>;
  let adminService : jasmine.SpyObj<AdminService>;


  beforeEach(() => {
    const managerServicespy = jasmine.createSpyObj('ManagerService',['loadTrainingProgram']);
    const adminServicespy = jasmine.createSpyObj('AdminService',['getAllJobs']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [UpcomingtrainingprogramComponent],
      providers : [
        {
          provide : ManagerService, useValue : managerServicespy
        },
        {
          provide : AdminService, useValue : adminServicespy
        }
      
      ]
    });
    fixture = TestBed.createComponent(UpcomingtrainingprogramComponent);
    component = fixture.componentInstance;
    managerService = TestBed.inject(ManagerService) as jasmine.SpyObj<ManagerService>;
    adminService = TestBed.inject(AdminService) as jasmine.SpyObj<AdminService>;
    //fixture.detectChanges();
  });
  const mockJobs : Job[] = [
    {
      jobId : 1,
      jobName : "test"
    },
    {
      jobId : 2,
      jobName : "develop"
    }
  ]
  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
  //----------------ngoninit------------------
  it('should load jobs data oninit', () => {
    // Arrange
    const mockResponse: ApiResponse<Job[]> = { success: true, data: mockJobs, message: '' };
    adminService.getAllJobs.and.returnValue(of(mockResponse));
    const upcomingTraining : UpcomingTraining[] = [
      {
        trainerName : "test",
        topicName : "test",
        jobName : "test",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      },
      {
        trainerName : "test 1",
        topicName : "test 1",
        jobName : "test 1",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      }
    ]
    const mockResponse1: ApiResponse<UpcomingTraining[]> = { success: true, data: upcomingTraining, message: '' };
    managerService.loadTrainingProgram.and.returnValue(of(mockResponse1));
    // Act
    //fixture.detectChanges(); // ngOnInit is called here
    component.ngOnInit();

    // Assert
    expect(adminService.getAllJobs).toHaveBeenCalled();
    expect(component.allJobs).toEqual(mockJobs);
  });
  //-------------------load jobs------------
  it('should load jobs data', () => {
    // Arrange
    const mockResponse: ApiResponse<Job[]> = { success: true, data: mockJobs, message: '' };
    adminService.getAllJobs.and.returnValue(of(mockResponse));

    // Act
    //fixture.detectChanges(); // ngOnInit is called here
    component.getAllJobs();

    // Assert
    expect(adminService.getAllJobs).toHaveBeenCalled();
    expect(component.allJobs).toEqual(mockJobs);
  });

  it('should handle errpr on init', () => {
    // Arrange
    const mockResponse: ApiResponse<Job[]> = { success: false, data: mockJobs, message: 'error fetching data' };
    adminService.getAllJobs.and.returnValue(of(mockResponse));
    spyOn(window,"alert")

    // Act
    component.getAllJobs();

    // Assert
    expect(adminService.getAllJobs).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith("error fetching data");
  });

  it('should handle http error', () => {
    // Arrange
    const mockError = {error :{message : "Error fetching data"}};
    adminService.getAllJobs.and.returnValue(throwError(mockError));
    spyOn(console,"error");
    spyOn(window,"alert");

    // Act
    component.getAllJobs();

    // Assert
    expect(adminService.getAllJobs).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith("Failed to fetch jobs",mockError.error.message);
    expect(window.alert).toHaveBeenCalledWith("Error fetching data");
  });

  //----------------loadTrainingProgram------------------
  it('should load Training data', () => {
    // Arrange
    const upcomingTraining : UpcomingTraining[] = [
      {
        trainerName : "test",
        topicName : "test",
        jobName : "test",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      },
      {
        trainerName : "test 1",
        topicName : "test 1",
        jobName : "test 1",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      }
    ]
    const mockResponse: ApiResponse<UpcomingTraining[]> = { success: true, data: upcomingTraining, message: '' };
    managerService.loadTrainingProgram.and.returnValue(of(mockResponse));

    // Act
    component.loadTrainingProgram();

    // Assert
    expect(managerService.loadTrainingProgram).toHaveBeenCalled();
    expect(component.upcomingTraining).toEqual(upcomingTraining);
  });
  it('should handle error response on load Training data', () => {
    // Arrange
    const mockResponse: ApiResponse<UpcomingTraining[]> = { success: false, data: {} as UpcomingTraining[], message: 'error' };
    managerService.loadTrainingProgram.and.returnValue(of(mockResponse));
    spyOn(console,'error')

    // Act
    component.loadTrainingProgram();

    // Assert
    expect(managerService.loadTrainingProgram).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch data',mockResponse.message)
  });
  it('should handle HTTP error on Training data', () => {
    // Arrange
    const mockError = { error: { message: 'Failed to add trainer' } };
    managerService.loadTrainingProgram.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    // Act
    component.loadTrainingProgram();

    // Assert
    expect(managerService.loadTrainingProgram).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching data',mockError);
    
  });
  //----------------loadTrainingProgramByJobId------------------
  it('should load Training data', () => {
    // Arrange
    const upcomingTraining : UpcomingTraining[] = [
      {
        trainerName : "test",
        topicName : "test",
        jobName : "test",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      },
      {
        trainerName : "test 1",
        topicName : "test 1",
        jobName : "test 1",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      }
    ]
    const mockResponse: ApiResponse<UpcomingTraining[]> = { success: true, data: upcomingTraining, message: '' };
    managerService.loadTrainingProgram.and.returnValue(of(mockResponse));

    // Act
    component.loadTrainingProgramByJobId(1);

    // Assert
    expect(managerService.loadTrainingProgram).toHaveBeenCalled();
    expect(component.upcomingTraining).toEqual(upcomingTraining);
  });
  it('should handle error response on load Training data', () => {
    // Arrange
    const mockResponse: ApiResponse<UpcomingTraining[]> = { success: false, data: {} as UpcomingTraining[], message: 'error' };
    managerService.loadTrainingProgram.and.returnValue(of(mockResponse));
    spyOn(console,'error')

    // Act
    component.loadTrainingProgramByJobId(1);

    // Assert
    expect(managerService.loadTrainingProgram).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to fetch data',mockResponse.message)
  });
  it('should handle HTTP error on Training data', () => {
    // Arrange
    const mockError = { error: { message: 'Failed to add trainer' } };
    managerService.loadTrainingProgram.and.returnValue(throwError(mockError));
    spyOn(console,'error')

    // Act
    component.loadTrainingProgramByJobId(1);

    // Assert
    expect(managerService.loadTrainingProgram).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Error fetching data',mockError);
    
  });
  
  // ----------------- onJobChange ------------------------
  it('should load training programs on job change', () => {
    // Arrange
    const upcomingTraining : UpcomingTraining[] = [
      {
        trainerName : "test",
        topicName : "test",
        jobName : "test",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      },
      {
        trainerName : "test 1",
        topicName : "test 1",
        jobName : "test 1",
         startDate:'2024-08-12',
        endDate:'2024-08-31'
      }
    ]
    const mockResponse: ApiResponse<UpcomingTraining[]> = { 
      success: true, 
      data: upcomingTraining, 
      message: '' 
    };
    const jobId = 42;
    const event = { target: { value: jobId } }
    spyOn(console, 'log');

    managerService.loadTrainingProgram.and.returnValue(of(mockResponse));

    // Act
    component.onJobChange(event)

    // Assert
    expect(console.log).toHaveBeenCalledWith('Selected Job Number:', jobId);
    expect(managerService.loadTrainingProgram).toHaveBeenCalled();
    expect(component.upcomingTraining).toEqual(mockResponse.data);
    expect(component.loading).toBeFalse();
  })





});
