import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParticipantComponent } from './participant.component';
import { AdminService } from 'src/app/services/admin.service';
import { Router } from '@angular/router';
import { Trainer } from 'src/app/models/trainer.model';
import { ManagerService } from 'src/app/services/manager.service';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { Participants } from 'src/app/models/participants.model';
import { of, throwError } from 'rxjs';

describe('ParticipantComponent', () => {
  let component: ParticipantComponent;
  let fixture: ComponentFixture<ParticipantComponent>;
  let managerService : jasmine.SpyObj<ManagerService>;
  let router: Router;

  const mockTrainer : Trainer[] = [
    {
      firstName : "test",
      lastName  : "test",
      loginbit : true,
      loginId : "test",
      email : "S@gmail.com",
      userId : 1,
      job : {
        jobId : 1,
        jobName : "test"
      },
      jobId : 1,
      role : 1
    }
  ]

  const mockParticipant : Participants[] = [
    {
      participantId : 1,
      firstName : "test",
      lastName  : "test",
      email : "S@gmail.com",
    
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
      participantId : 2,
      firstName : "test",
      lastName  : "test",
      email : "S1@gmail.com",
     
      userId : 2,
      jobId : 2,
     
      user : {
        userId : 2,
        firstName : "test",
        lastName : "test"
      },
      job : {
        jobId : 2,
        jobName : "test"
      },
     
    }
  ]

  beforeEach(() => {
    const managerServicespy = jasmine.createSpyObj('ManagerService',['getAllPartiipantByManagerId']);
    TestBed.configureTestingModule({
      declarations: [ParticipantComponent],
      imports : [HttpClientTestingModule,RouterTestingModule],
      providers : [
        {
          provide : ManagerService, useValue : managerServicespy
        },
      
      ]
    });
    fixture = TestBed.createComponent(ParticipantComponent);
    component = fixture.componentInstance;
    managerService = TestBed.inject(ManagerService) as jasmine.SpyObj<ManagerService>;
    router = TestBed.inject(Router);
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load Participant details', () => {
    // Arrange
    const mockResponse: ApiResponse<Participants[]> = { success: true, data: mockParticipant, message: '' };
    managerService.getAllPartiipantByManagerId.and.returnValue(of(mockResponse));

    // Act
    component.loadParticipant(1);

    // Assert
    expect(component.participants.length).toBe(2);
    expect(managerService.getAllPartiipantByManagerId).toHaveBeenCalled();
    expect(component.participants).toEqual(mockParticipant);
  });
  it('should handle response error', () => {
    // Arrange
    const mockResponse: ApiResponse<Participants[]> = { success: false, data: mockParticipant, message: 'error' };
    managerService.getAllPartiipantByManagerId.and.returnValue(of(mockResponse));
    spyOn(console,"error");

    // Act
    component.loadParticipant(1);

    // Assert
    expect(component.participants.length).toBe(0);
    expect(managerService.getAllPartiipantByManagerId).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledOnceWith("Failed to participants trainer",mockResponse.message);
  });

  it('should handle Http error', () => {
    // Arrange
    const mockError = { message: 'Error fetching product' };
    managerService.getAllPartiipantByManagerId.and.returnValue(throwError(mockError));
    spyOn(console,"error");

    // Act
    component.loadParticipant(1);

    // Assert
    expect(component.participants.length).toBe(0);
    expect(managerService.getAllPartiipantByManagerId).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledOnceWith("Error fetching participants",mockError);
  });
});
