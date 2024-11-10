import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTrainerComponent } from './add-trainer.component';
import { Router } from '@angular/router';
import { AdminService } from 'src/app/services/admin.service';
import { ParticipantsListComponent } from '../../trainer/participants-list/participants-list.component';
import { TrainerListComponent } from '../trainer-list/trainer-list.component';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { Job } from 'src/app/models/job.model';
import { of, throwError } from 'rxjs';
import { AddTrainer } from 'src/app/models/add-trainer.model';

describe('AddTrainerComponent', () => {
  let component: AddTrainerComponent;
  let fixture: ComponentFixture<AddTrainerComponent>;
  let adminSpy : jasmine.SpyObj<AdminService>;
  let router : Router;

  beforeEach(() => {
    const adminServieSpy = jasmine.createSpyObj('AdminService',["addTrainer","getAllJobs"]);
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,FormsModule,ReactiveFormsModule,RouterTestingModule.withRoutes([{path:'trainer-list',component : TrainerListComponent}])],
      declarations: [AddTrainerComponent],
      providers : [
        {
          provide : AdminService,useValue : adminServieSpy
        },
      ]
    });
    fixture = TestBed.createComponent(AddTrainerComponent);
    component = fixture.componentInstance;
    adminSpy = TestBed.inject(AdminService) as jasmine.SpyObj<AdminService>;
    router = TestBed.inject(Router);
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

  //----------------on init----------
  it('should load jobs data', () => {
    // Arrange
    const mockResponse: ApiResponse<Job[]> = { success: true, data: mockJobs, message: '' };
    adminSpy.getAllJobs.and.returnValue(of(mockResponse));

    // Act
    //fixture.detectChanges(); // ngOnInit is called here
    component.ngOnInit();

    // Assert
    expect(adminSpy.getAllJobs).toHaveBeenCalled();
    expect(component.allJobs).toEqual(mockJobs);
  });

  it('should handle errpr on init', () => {
    // Arrange
    const mockResponse: ApiResponse<Job[]> = { success: false, data: mockJobs, message: 'error fetching data' };
    adminSpy.getAllJobs.and.returnValue(of(mockResponse));
    spyOn(window,"alert")

    // Act
    component.ngOnInit();

    // Assert
    expect(adminSpy.getAllJobs).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith("error fetching data");
  });

  it('should handle http error', () => {
    // Arrange
    const mockError = {error :{message : "Error fetching data"}};
    adminSpy.getAllJobs.and.returnValue(throwError(mockError));
    spyOn(console,"error");
    spyOn(window,"alert");

    // Act
    component.ngOnInit();

    // Assert
    expect(adminSpy.getAllJobs).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith("Failed to fetch jobs",mockError.error.message);
    expect(window.alert).toHaveBeenCalledWith("Error fetching data");
  });

  //----------------add trainer------------
  it('should add produt suessfully and nevigate to trainers list',()=>{
    //Arrange
    const mocktrainer = {
      firstName : "test",
      lastName : "Test",
      loginId : "test",
      email : "email@gmail.com",
      jobId : 1,
    };
    const mockJobs: Job[] = [
      {
        jobId: 1,
        jobName: 'Job 1'
      },
      {
        jobId: 2,
        jobName: 'Job 2'
      },
    ]
    const mockJobResponse: ApiResponse<Job[]> = {
      data: mockJobs,
      success: true,
      message: ''
    }
    spyOn(router, 'navigate');
    spyOn(console, 'log');

    const mockResponse: ApiResponse<string> = { success: true, data: '', message: 'trainer added successfully' };
    adminSpy.getAllJobs.and.returnValue(of(mockJobResponse));
    adminSpy.addTrainer.and.returnValue(of(mockResponse));
    
    fixture.detectChanges();
    component.trainerForm.setValue(mocktrainer)

    //Act
    
    component.onSubmit();

    //Assert
    expect(adminSpy.addTrainer).toHaveBeenCalledWith(mocktrainer);
    expect(adminSpy.getAllJobs).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/trainer-list']);
    expect(console.log).toHaveBeenCalledWith('completed');
  })

  it('should handle error when add trainer fails', () => {
    //Arrange
    const mocktrainer = {
      firstName : "test",
      lastName : "Test",
      loginId : "test",
      email : "email@gmail.com",
      jobId : 1,
    };
    const mockJobs: Job[] = [
      {
        jobId: 1,
        jobName: 'Job 1'
      },
      {
        jobId: 2,
        jobName: 'Job 2'
      },
    ]
    const mockJobResponse: ApiResponse<Job[]> = {
      data: mockJobs,
      success: true,
      message: ''
    }
    spyOn(window, 'alert');
    spyOn(console, 'log');

    const mockResponse: ApiResponse<string> = { success: false, data: '', message: 'Error occurred' };
    adminSpy.getAllJobs.and.returnValue(of(mockJobResponse));
    adminSpy.addTrainer.and.returnValue(of(mockResponse));
    
    fixture.detectChanges();
    component.trainerForm.setValue(mocktrainer)

    //Act
    
    component.onSubmit();

    //Assert
    expect(adminSpy.addTrainer).toHaveBeenCalledWith(mocktrainer);
    expect(adminSpy.getAllJobs).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message);
    expect(console.log).toHaveBeenCalledWith('completed');
  })


  it('should handle error when http errpr', () => {
    //Arrange
    const mocktrainer = {
      firstName : "test",
      lastName : "Test",
      loginId : "test",
      email : "email@gmail.com",
      jobId : 1,
    };
    const mockJobs: Job[] = [
      {
        jobId: 1,
        jobName: 'Job 1'
      },
      {
        jobId: 2,
        jobName: 'Job 2'
      },
    ]
    const mockJobResponse: ApiResponse<Job[]> = {
      data: mockJobs,
      success: true,
      message: ''
    }
    spyOn(window, 'alert');
    spyOn(console, 'error')
    spyOn(console, 'log');

    const mockError = {
      error: {
        message: 'Error occurred'
      }
    }
    adminSpy.getAllJobs.and.returnValue(of(mockJobResponse));
    adminSpy.addTrainer.and.returnValue(throwError(mockError));
    
    fixture.detectChanges();
    component.trainerForm.setValue(mocktrainer)

    //Act
    
    component.onSubmit();

    //Assert
    expect(adminSpy.addTrainer).toHaveBeenCalledWith(mocktrainer);
    expect(adminSpy.getAllJobs).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith('Failed to add trainer', mockError.error.message);
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message)
    expect(console.log).toHaveBeenCalledWith('completed');
  })
});
