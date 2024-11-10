import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';

import { TrainerListComponent } from './trainer-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { AdminService } from 'src/app/services/admin.service';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { of, throwError } from 'rxjs';
import { Trainer } from 'src/app/models/trainer.model';

describe('TrainerListComponent', () => {
  let component: TrainerListComponent;
  let fixture: ComponentFixture<TrainerListComponent>;
  let adminService : AdminService;
  let router : Router;
  beforeEach(() => {
    
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [TrainerListComponent],
      providers:[AdminService]
    });
    fixture = TestBed.createComponent(TrainerListComponent);
    component = fixture.componentInstance;
    adminService =TestBed.inject(AdminService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize component correctly', () => {
    // Test ngOnInit
    spyOn(component, 'loadTrainerCount');
    component.ngOnInit();
    expect(component.loadTrainerCount).toHaveBeenCalled();
  });

  it('should fetch trainer count', fakeAsync(() => {
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: 10,
      message: 'trainer count fetched successfully'
    };

    spyOn(adminService, 'getTotalTrainerCount').and.returnValue(of(mockApiResponse));

    component.loadTrainerCount();
    tick();

    expect(component.totalItems).toEqual(mockApiResponse.data);
    expect(component.totalPages).toEqual(Math.ceil(mockApiResponse.data / component.pageSize));
  }));

  it('should handle error while fetching trainer count', fakeAsync(() => {
    const errorMessage = 'Error fetching trainer count';

    spyOn(adminService, 'getTotalTrainerCount').and.returnValue(throwError(errorMessage));

    component.loadTrainerCount();
    tick();

    expect(component.totalItems).toEqual(0);
    expect(component.totalPages).toEqual(0); 
    
  }));

  it('should fetch trainer with pagination', fakeAsync(() => {
    const mockTrainer: Trainer[] = [
      {
        userId:1,
        loginId:'test',
        firstName:'firstName1',
        lastName:'lastname1',
        email:'test@gmail.com',
        role:2,
        loginbit:true,
        jobId:1,
        job:{jobId:1,jobName:'job1'}
      }
    ];

    const mockApiResponse: ApiResponse<Trainer[]> = {
      success: true,
      data: mockTrainer,
      message: 'Trainers fetched successfully'
    };

    spyOn(adminService, 'getAllTrainerWithPagination').and.returnValue(of(mockApiResponse));

    component.loadTrainer(2);
    tick();
    fixture.detectChanges(); // Detect changes after async operation

    expect(component.trainer);
  }));

  it('should handle error while fetching trainer with pagination', fakeAsync(() => {
    const errorMessage = 'Error fetching trainer';

    spyOn(adminService, 'getAllTrainerWithPagination').and.returnValue(throwError(errorMessage));

    component.loadTrainer(2);
    tick();

    expect(component.trainer).toBeDefined(); // Ensure data is not set on error
   
  }));

  it('should change page', () => {
    const mockTrainer: Trainer[] = [
      {
        userId:1,
        loginId:'test',
        firstName:'firstName1',
        lastName:'lastname1',
        email:'test@gmail.com',
        role:2,
        loginbit:true,
        jobId:1,
        job:{jobId:1,jobName:'job1'}
      }
    ];
    const mockApiResponse: ApiResponse<Trainer[]> = {
      success: true,
      data: mockTrainer,
      message: ''
    };

    spyOn(adminService, 'getAllTrainerWithPagination').and.returnValue(throwError(mockApiResponse));
    spyOn(adminService, 'getTotalTrainerCount').and.returnValue(throwError(mockApiResponse));

    component.changePage(2);

    expect(component.pageNumber).toBe(2);
  });

  it('should change page size', () => {
    const mockTrainer: Trainer[] = [
      {
        userId:1,
        loginId:'test',
        firstName:'firstName1',
        lastName:'lastname1',
        email:'test@gmail.com',
        role:2,
        loginbit:true,
        jobId:1,
        job:{jobId:1,jobName:'job1'}
      }
    ];
    const mockApiResponse: ApiResponse<Trainer[]> = {
      success: true,
      data: mockTrainer,
      message: ''
    };

    spyOn(adminService, 'getAllTrainerWithPagination').and.returnValue(throwError(mockApiResponse));
    spyOn(adminService, 'getTotalTrainerCount').and.returnValue(throwError(mockApiResponse));

    component.changePageSize(10);

    expect(component.pageSize).toBe(10);
    expect(component.pageNumber).toBe(1);
  });

});
