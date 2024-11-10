import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';

import { ParticipantsListComponent } from './participants-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { TrainerService } from 'src/app/services/trainer.service';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { of, throwError } from 'rxjs';
import { Participants } from 'src/app/models/participants.model';
import { Nominate } from 'src/app/models/nominate.model';

describe('ParticipantsListComponent', () => {
  let component: ParticipantsListComponent;
  let fixture: ComponentFixture<ParticipantsListComponent>;
  let trainerService : TrainerService;
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports : [HttpClientTestingModule,RouterTestingModule,FormsModule],
      declarations: [ParticipantsListComponent],
      providers:[TrainerService]
    });
    fixture = TestBed.createComponent(ParticipantsListComponent);
    component = fixture.componentInstance;
    trainerService = TestBed.inject(TrainerService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch participants count', fakeAsync(() => {
    const mockApiResponse: ApiResponse<number> = {
      success: true,
      data: 10,
      message: 'participants count fetched successfully'
    };

    spyOn(trainerService, 'getTotalParticipantsCount').and.returnValue(of(mockApiResponse));

    component.loadParticipantCount();
    tick();

    expect(component.totalItems).toEqual(mockApiResponse.data);
    expect(component.totalPages).toEqual(Math.ceil(mockApiResponse.data / component.pageSize));
  }));

  it('should fetch participants count', fakeAsync(() => {
    // Arrange
    const mockApiResponse: ApiResponse<number> = {
      success: false,
      data: 0,
      message: 'Failed to fetch participant count'
    };

    spyOn(console, 'error')
    spyOn(trainerService, 'getTotalParticipantsCount').and.returnValue(of(mockApiResponse));

    // Act
    component.loadParticipantCount();

    // Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch participants count', mockApiResponse.message)
    expect(component.loading).toBeFalse();
  }));

  it('should handle error while fetching participants count', fakeAsync(() => {
    const errorMessage = 'Error fetching participants count';

    spyOn(trainerService, 'getTotalParticipantsCount').and.returnValue(throwError(errorMessage));

    component.loadParticipantCount();
    tick();

    expect(component.totalItems).toEqual(0);
    expect(component.totalPages).toEqual(0); 
    
  }));

  it('should fetch participants with pagination', fakeAsync(() => {
    const mockTrainer: Nominate[] = [
      // {
      //   firstName: 'firstName1',
      //   lastName: 'lastname1',
      //   email: 'test@gmail.com',
       
      //   userId: 1,
      //   jobId: 1,
       
      //   job: { jobId: 1, jobName: 'job1' },
      //   user: { userId: 1, firstName: 'firstName1', lastName: 'lastname1' },
       
      //   participantId: 0
      // }
    ];

    const mockApiResponse: ApiResponse<Nominate[]> = {
      success: true,
      data: mockTrainer,
      message: 'participants fetched successfully'
    };

    spyOn(trainerService, 'getAllParticipantsWithPagination').and.returnValue(of(mockApiResponse));

    component.loadParticipants(2,'default');
    tick();
    fixture.detectChanges(); // Detect changes after async operation

    expect(component.participants);
  }));

  it('should fetch participants with pagination', fakeAsync(() => {
    // Arrange
    const mockApiResponse: ApiResponse<Nominate[]> = {
      success: false,
      data: [],
      message: 'Failed to fetch participants'
    };

    spyOn(console, 'error');
    spyOn(trainerService, 'getAllParticipantsWithPagination').and.returnValue(of(mockApiResponse));

    // Act
    component.loadParticipants(2, 'default');

    // Assert
    expect(console.error).toHaveBeenCalledWith('Failed to fetch participants: ', mockApiResponse.message);
  }));

  it('should handle error while fetching participants with pagination', fakeAsync(() => {
    const errorMessage = 'Error fetching participants';

    spyOn(trainerService, 'getAllParticipantsWithPagination').and.returnValue(throwError(errorMessage));

    component.loadParticipants(2,'default');
    tick();

    expect(component.participants).toBeDefined(); // Ensure data is not set on error
   
  }));

  

  it('should change page', () => {
    const mockTrainer: Participants[] = [
      // {
      //   firstName: 'firstName1',
      //   lastName: 'lastname1',
      //   email: 'test@gmail.com',
      //   modePreference: 'Online',
      //   isNominated: true,
      //   userId: 1,
      //   jobId: 1,
      //   topicId: 1,
      //   job: { jobId: 1, jobName: 'job1' },
      //   user: { userId: 1, firstName: 'firstName1', lastName: 'lastname1' },
      //   topic: { topicId: 1, topicName: 'testing' },
      //   participantId: 0
      // }
    ];
    const mockApiResponse: ApiResponse<Participants[]> = {
      success: true,
      data: mockTrainer,
      message: ''
    };

    spyOn(trainerService, 'getAllParticipantsWithPagination').and.returnValue(throwError(mockApiResponse));
    spyOn(trainerService, 'getTotalParticipantsCount').and.returnValue(throwError(mockApiResponse));

    component.changePage(2);

    expect(component.pageNumber).toBe(2);
  });

  it('should change page size', () => {
    const mockTrainer: Participants[] = [
      // {
      //   firstName: 'firstName1',
      //   lastName: 'lastname1',
      //   email: 'test@gmail.com',
      //   modePreference: 'Online',
      //   isNominated: true,
      //   userId: 1,
      //   jobId: 1,
      //   topicId: 1,
      //   job: { jobId: 1, jobName: 'job1' },
      //   user: { userId: 1, firstName: 'firstName1', lastName: 'lastname1' },
      //   topic: { topicId: 1, topicName: 'testing' },
      //   participantId: 0
      // }
    ];
    const mockApiResponse: ApiResponse<Participants[]> = {
      success: true,
      data: mockTrainer,
      message: ''
    };

    spyOn(trainerService, 'getAllParticipantsWithPagination').and.returnValue(throwError(mockApiResponse));
    spyOn(trainerService, 'getTotalParticipantsCount').and.returnValue(throwError(mockApiResponse));

    component.changePageSize(10);

    expect(component.pageSize).toBe(10);
    expect(component.pageNumber).toBe(1);
  });

  it('should toggle sort order', () => {
    const mockTrainer: Participants[] = [
      // {
      //   firstName: 'firstName1',
      //   lastName: 'lastname1',
      //   email: 'test@gmail.com',
      //   modePreference: 'Online',
      //   isNominated: true,
      //   userId: 1,
      //   jobId: 1,
      //   topicId: 1,
      //   job: { jobId: 1, jobName: 'job1' },
      //   user: { userId: 1, firstName: 'firstName1', lastName: 'lastname1' },
      //   topic: { topicId: 1, topicName: 'testing' },
      //   participantId: 0
      // }
    ];
    const mockApiResponse: ApiResponse<Participants[]> = {
      success: true,
      data: mockTrainer,
      message: ''
    };

    spyOn(trainerService, 'getAllParticipantsWithPagination').and.returnValue(throwError(mockApiResponse));
    spyOn(trainerService, 'getTotalParticipantsCount').and.returnValue(throwError(mockApiResponse));

    fixture.detectChanges(); // Trigger ngOnInit

    component.toggleSort();
    expect(component.sortName).toBe('asc');
    
    component.toggleSort();
    expect(component.sortName).toBe('desc');
    
    component.toggleSort();
    expect(component.sortName).toBe('default');
  });

  // ------------ nextPage -------------------
  it('should go to next page if page is not last page', () => {
    // Arrange
    let pageNumber = 1;
    let expectedPageNumber = 2;
    component.pageNumber = 1;
    component.totalPages = 10;
    component.sortName = "asc";
    const mockTrainer: Nominate[] = [];

    const mockApiResponse: ApiResponse<Nominate[]> = {
      success: true,
      data: mockTrainer,
      message: 'participants fetched successfully'
    };

    spyOn(trainerService, 'getAllParticipantsWithPagination').and.returnValue(of(mockApiResponse));

    // Act
    component.nextPage()

    // Assert
    expect(component.pageNumber).toEqual(expectedPageNumber);
    expect(trainerService.getAllParticipantsWithPagination).toHaveBeenCalled();
  });

  it('should not go to next page if page is last page', () => {
    // Arrange
    let expectedPageNumber = 10;
    component.pageNumber = 10;
    component.totalPages = 10;

    // Act
    component.nextPage()

    // Assert
    expect(component.pageNumber).toEqual(expectedPageNumber);
  });

  // -------------- previousPage -----------------------------
  it('should go to previous page if page is not first page', () => {
    // Arrange
    let expectedPageNumber = 1;
    component.pageNumber = 2;
    component.sortName = "asc";
    const mockTrainer: Nominate[] = [];

    const mockApiResponse: ApiResponse<Nominate[]> = {
      success: true,
      data: mockTrainer,
      message: 'participants fetched successfully'
    };

    spyOn(trainerService, 'getAllParticipantsWithPagination').and.returnValue(of(mockApiResponse));

    // Act
    component.previousPage()

    // Assert
    expect(component.pageNumber).toEqual(expectedPageNumber);
    expect(trainerService.getAllParticipantsWithPagination).toHaveBeenCalled();
  });

  it('should not go to previous page if page is first page', () => {
    // Arrange
    let expectedPageNumber = 1;
    component.pageNumber = 1;

    // Act
    component.nextPage()

    // Assert
    expect(component.pageNumber).toEqual(expectedPageNumber);
  });

});
