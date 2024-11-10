import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AdminService } from 'src/app/services/admin.service';
import { ChangeDetectorRef } from '@angular/core';
import { DaterangeBasedReport } from 'src/app/models/daterange-based-report.model';
import { DateRangeReportComponent } from './date-range-report.component';
import { Job } from 'src/app/models/job.model';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { of, throwError } from 'rxjs';

describe('DateRangeReportComponent', () => {
  let component: DateRangeReportComponent;
  let fixture: ComponentFixture<DateRangeReportComponent>;

  let adminServiceSpy: jasmine.SpyObj<AdminService>;
  let cdrSpy: jasmine.SpyObj<ChangeDetectorRef>;
  
  const mockReport: DaterangeBasedReport[] = [
    { topicName: 'topic 1', trainerName:'trainer 1' ,startDate: '2024-01-01', endDate: '2024-06-01', duration: 6, modePreference: 'online', totalParticipateNo: 15},
    { topicName: 'topic 2', trainerName:'trainer 2' ,startDate: '2024-02-05', endDate: '2024-03-01', duration: 1, modePreference: 'offline', totalParticipateNo: 5},

  ];

  const mockjob  : Job [] = [
    {jobId : 1, jobName:' job 1'},
    {jobId : 2, jobName:' job 2'},
    
  ];

  beforeEach(() => {

    adminServiceSpy = jasmine.createSpyObj('AdminService', ['daterangeBasedReport','getAllJobs']);
    cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);

    TestBed.configureTestingModule({
      imports: [HttpClientModule,FormsModule],
      declarations: [DateRangeReportComponent],
      providers: [
        { provide: AdminService, useValue: adminServiceSpy },
        { provide: ChangeDetectorRef, useValue: cdrSpy }
      ],
    });
    fixture = TestBed.createComponent(DateRangeReportComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  // daterangeBasedReport
  it('should load date base report successfully', () => {
    // Arrange
    var jobId = 1;
    var startDate = '2024-01-01';
    var endDate = '2024-06-01';
    const mockResponse: ApiResponse<DaterangeBasedReport[]> = { success: true, data: mockReport, message: '' };
    adminServiceSpy.daterangeBasedReport.and.returnValue(of(mockResponse));


    // Act
    component.loadDaterangeBasedReport(jobId,startDate,endDate);

    // Assert
    expect(adminServiceSpy.daterangeBasedReport).toHaveBeenCalled();
    expect(component.reportDetails).toEqual(mockResponse.data);  
    expect(component.loading).toBeFalse();
  });

  it('should handle unsuccessful response when loading date base report', () => {
    // Arrange
    var jobId = 1;
    var startDate = '2024-01-01';
    var endDate = '2024-06-01';
    const mockResponse: ApiResponse<DaterangeBasedReport[]> = { success: false, data: [], message: 'Error message' };
    adminServiceSpy.daterangeBasedReport.and.returnValue(of(mockResponse));
    spyOn(console, 'error');


    // Act
    component.loadDaterangeBasedReport(jobId,startDate,endDate);

    // Assert
    expect(adminServiceSpy.daterangeBasedReport).toHaveBeenCalled();
    expect(component.reportDetails).toEqual(mockResponse.data);  
    expect(console.error).toHaveBeenCalledWith('Failed to fetch report',mockResponse.message);
    expect(component.loading).toBeFalse();
  });

  it('should handle error when loading date base report', () => {
    // Arrange
    var jobId = 1;
    var startDate = '2024-01-01';
    var endDate = '2024-06-01';
    const mockError = { error : { message: 'Failed to fetch report'}}
    adminServiceSpy.daterangeBasedReport.and.returnValue(throwError(mockError));
    spyOn(console, 'error');

    // Act
    component.loadDaterangeBasedReport(jobId,startDate,endDate);

    // Assert
    expect(adminServiceSpy.daterangeBasedReport).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith(mockError.error.message);
    expect(component.loading).toBeFalse();
  });

  //getAllJob
  it('should load all jobs successfully', () => {
    // Arrange
   
    const mockResponse: ApiResponse<Job[]> = { success: true, data: mockjob, message: '' };
    adminServiceSpy.getAllJobs.and.returnValue(of(mockResponse));


    // Act
    component.loadjobRoles();

    // Assert
    expect(adminServiceSpy.getAllJobs).toHaveBeenCalled();
    expect(component.jobsroles).toEqual(mockResponse.data);  
    expect(component.loading).toBeFalse();
  });

  it('should handle unsuccessful response when loading all jobs', () => {
    // Arrange

    const mockResponse: ApiResponse<Job[]> = { success: false, data: [], message: 'Error message' };
    adminServiceSpy.getAllJobs.and.returnValue(of(mockResponse));
    spyOn(console, 'error');


    // Act
    component.loadjobRoles();

    // Assert
    expect(adminServiceSpy.getAllJobs).toHaveBeenCalled();
    expect(component.jobsroles).toEqual(mockResponse.data);  
    expect(console.error).toHaveBeenCalledWith('Failed to fetch jobs',mockResponse.message);
    expect(component.loading).toBeFalse();
  });

  it('should handle error when loading all jobs', () => {
    // Arrange
   
    const mockError = { error : { message: 'Failed to fetch report'}}
    adminServiceSpy.getAllJobs.and.returnValue(throwError(mockError));
    spyOn(console, 'error');

    // Act
    component.loadjobRoles();

    // Assert
    expect(adminServiceSpy.getAllJobs).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith(mockError.error.message);
    expect(component.loading).toBeFalse();
  });
});
