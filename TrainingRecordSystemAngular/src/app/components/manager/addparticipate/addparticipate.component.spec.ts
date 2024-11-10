import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddparticipateComponent } from './addparticipate.component';
import { ManagerService } from 'src/app/services/manager.service';
import { AuthService } from 'src/app/services/auth-service.service';
import { Router } from '@angular/router';
import { AdminService } from 'src/app/services/admin.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { of, throwError } from 'rxjs';
import { Job } from 'src/app/models/job.model';

describe('AddparticipateComponent', () => {
  let component: AddparticipateComponent;
  let fixture: ComponentFixture<AddparticipateComponent>;
  let managerSpy : jasmine.SpyObj<ManagerService>;
  let authSpy : jasmine.SpyObj<AuthService>;
  let adminSpy:jasmine.SpyObj<AdminService>;
  let router : Router;

  beforeEach(() => {
    managerSpy = jasmine.createSpyObj('ManagerService',['addParticipate']);
    authSpy = jasmine.createSpyObj('AuthService',['getUserId']);
    adminSpy=jasmine.createSpyObj('AdminService',['getAllJobs']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule, ReactiveFormsModule],
      declarations: [AddparticipateComponent],
      providers : [
        {
          provide : ManagerService,useValue : managerSpy,
        },
        { provide: AuthService, useValue: authSpy },
        { provide: AdminService, useValue: adminSpy }
       
      ]
    });
    fixture = TestBed.createComponent(AddparticipateComponent);
    component = fixture.componentInstance;
    //fixture.detectChanges();
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
  // get all job
  //----------------on init----------
  it('should load jobs data', () => {
    // Arrange
    const mockResponse: ApiResponse<Job[]> = { success: true, data: mockJobs, message: '' };
    adminSpy.getAllJobs.and.returnValue(of(mockResponse));
    authSpy.getUserId.and.returnValue(of('testUserId'));
    // Act
    fixture.detectChanges(); 
    

    // Assert
    expect(adminSpy.getAllJobs).toHaveBeenCalled();
    expect(component.allJobs).toEqual(mockJobs);
  });

  it('should handle error on init on load job', () => {
    // Arrange
    const mockResponse: ApiResponse<Job[]> = { success: false, data: mockJobs, message: 'error fetching data' };
    adminSpy.getAllJobs.and.returnValue(of(mockResponse));
    authSpy.getUserId.and.returnValue(of('testUserId'));
    spyOn(window,"alert")

    // Act
    fixture.detectChanges();

    // Assert
    expect(adminSpy.getAllJobs).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith("error fetching data");
  });
  it('should handle http error while load job', () => {
    // Arrange
    const mockError = {error :{message : "Error fetching data"}};
    adminSpy.getAllJobs.and.returnValue(throwError(mockError));
    authSpy.getUserId.and.returnValue(of('testUserId'));
    spyOn(console,"error");
    spyOn(window,"alert");

    // Act
    fixture.detectChanges();

    // Assert
    expect(adminSpy.getAllJobs).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith("Failed to fetch jobs",mockError.error.message);
    expect(window.alert).toHaveBeenCalledWith("Error fetching data");
  });
// add partcipant
  it('should add partcipant successfully and navigate to participantsByManager',()=>{
    //Arrange
    const mockform={
      firstName: "firstName",
      jobId: 1,
      lastName:"lastName",
      email:"email@gmail.com"
   
    }
    spyOn(router, 'navigate');

    const mockResponse: ApiResponse<string> = { success: true, data: '', message: '' };
    authSpy.getUserId.and.returnValue(of('testUserId')); // Mock getUserId
    adminSpy.getAllJobs.and.returnValue(of({ success: true, data: [],message:'' })); 
    fixture.detectChanges();

    //Act
    component.addParticipateForm.setValue(mockform); // Set form value
    managerSpy.addParticipate.and.returnValue(of(mockResponse));
    component.onSubmit();

    
   // Assert
  expect(managerSpy.addParticipate).toHaveBeenCalled();
  expect(router.navigate).toHaveBeenCalled();


  })
  it('should handle http error while add participant', () => {
    // Arrange
    const mockError = {error :{message : 'Error'}};
     adminSpy.getAllJobs.and.returnValue(of({ success: true, data: [],message:'' })); 
     authSpy.getUserId.and.returnValue(of('testUserId'));
    managerSpy.addParticipate.and.returnValue(throwError(mockError));
    const mockform={
      firstName: "firstName",
      jobId: 1,
      lastName:"lastName",
      email:"email@gmail.com"
   
    }
    spyOn(window,"alert");

    // Act
    fixture.detectChanges();
    component.addParticipateForm.setValue(mockform); // Set form value
    component.onSubmit();


    // Assert
    expect(managerSpy.addParticipate).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message);
  });
  it('should handle error when add partcicipant fails', () => {
    // Arrange
    spyOn(window, 'alert');
  
    const mockform={
      firstName: "firstName",
      jobId: 1,
      lastName:"lastName",
      email:"email@gmail.com"
   
    }
    const mockResponse: ApiResponse<string> = { success: false, data: '', message: 'Error' };
    
    authSpy.getUserId.and.returnValue(of('testUserId')); // Mock getUserId
    adminSpy.getAllJobs.and.returnValue(of({ success: true, data: [],message:'' })); 
    fixture.detectChanges(); 

    // Act
    component.addParticipateForm.setValue(mockform);// Set form value
    managerSpy.addParticipate.and.returnValue(of(mockResponse));
    component.onSubmit(); // Call onSubmit method

    // Assert
    expect(managerSpy.addParticipate).toHaveBeenCalled(); 
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message); 
  });
  
});
