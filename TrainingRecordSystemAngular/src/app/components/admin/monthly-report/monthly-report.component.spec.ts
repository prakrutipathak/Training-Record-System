import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthlyReportComponent } from './monthly-report.component';
import { ChangeDetectorRef } from '@angular/core';
import { AdminService } from 'src/app/services/admin.service';
import { HttpClientModule } from '@angular/common/http';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { MonthlyAdminReport } from 'src/app/models/monthly-admin-report.model';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { Trainer } from 'src/app/models/trainer.model';

describe('MonthlyReportComponent', () => {
  let component: MonthlyReportComponent;
  let fixture: ComponentFixture<MonthlyReportComponent>;

  let adminServiceSpy: jasmine.SpyObj<AdminService>;
  let cdrSpy: jasmine.SpyObj<ChangeDetectorRef>;

  const mockReport: MonthlyAdminReport[] = [
    { topicName: 'topic 1', startDate: '2024-01-01', endDate: '2024-06-01', duration: 6, modePreference: 'online', totalParticipateNo: 15},
    { topicName: 'topic 2', startDate: '2024-02-05', endDate: '2024-03-01', duration: 1, modePreference: 'offline', totalParticipateNo: 5},

  ];

  const mocktrainer : Trainer[] = [
    {
      userId: 5, loginId: 'login 1', firstName: 'firstname 1', lastName: 'lastname 1', email: 'email 1', role: 2, loginbit: false, jobId: 1,
      job: {jobId : 1, jobName:'job 1'},
      
    },
    {
      userId: 6, loginId: 'login 2', firstName: 'firstname 2', lastName: 'lastname 2', email: 'email 2', role: 2, loginbit: false, jobId: 2,
      job: {jobId : 2, jobName:'job 2'},
      
    },
  ];

  beforeEach(() => {

    adminServiceSpy = jasmine.createSpyObj('AdminService', ['monthlyAdminReport','getAllTrainer']);
    cdrSpy = jasmine.createSpyObj('ChangeDetectorRef', ['detectChanges']);

    TestBed.configureTestingModule({
      imports: [HttpClientModule,FormsModule],
      declarations: [MonthlyReportComponent],
      providers: [
        { provide: AdminService, useValue: adminServiceSpy },
        { provide: ChangeDetectorRef, useValue: cdrSpy }
      ],
    });
    fixture = TestBed.createComponent(MonthlyReportComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  //monthlyAdminReport
  it('should load monthly report successfully', () => {
    // Arrange
    var userId = 1;
    var month = 6;
    var year=2024;
    const mockResponse: ApiResponse<MonthlyAdminReport[]> = { success: true, data: mockReport, message: '' };
    adminServiceSpy.monthlyAdminReport.and.returnValue(of(mockResponse));


    // Act
    component.loadMonthlyAdminReport(userId,month,year);

    // Assert
    expect(adminServiceSpy.monthlyAdminReport).toHaveBeenCalled();
    expect(component.reportDetails).toEqual(mockResponse.data);  
    expect(component.loading).toBeFalse();
  });

  it('should handle unsuccessful response when loading monthly report', () => {
    // Arrange
    var userId = 1;
    var month = 6;
    var year=2024;
    const mockResponse: ApiResponse<MonthlyAdminReport[]> = { success: false, data: [], message: 'Error message' };
    adminServiceSpy.monthlyAdminReport.and.returnValue(of(mockResponse));
    spyOn(console, 'error');


    // Act
    component.loadMonthlyAdminReport(userId,month,year);

    // Assert
    expect(adminServiceSpy.monthlyAdminReport).toHaveBeenCalled();
    expect(component.reportDetails).toEqual(mockResponse.data);  
    expect(console.error).toHaveBeenCalledWith('Failed to fetch report',mockResponse.message);
    expect(component.loading).toBeFalse();
  });

  it('should handle error when loading monthly report', () => {
    // Arrange
    var userId = 1;
    var month = 6;
    var year=2024;
    const mockError = { error : { message: 'Failed to fetch report'}}
    adminServiceSpy.monthlyAdminReport.and.returnValue(throwError(mockError));
    spyOn(console, 'error');

    // Act
    component.loadMonthlyAdminReport(userId,month,year);

    // Assert
    expect(adminServiceSpy.monthlyAdminReport).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith(mockError.error.message);
    expect(component.loading).toBeFalse();
  });

  //getAllTrainer
  it('should load all trainer successfully', () => {
    // Arrange
   
    const mockResponse: ApiResponse<Trainer[]> = { success: true, data: mocktrainer, message: '' };
    adminServiceSpy.getAllTrainer.and.returnValue(of(mockResponse));


    // Act
    component.loadTrainers();

    // Assert
    expect(adminServiceSpy.getAllTrainer).toHaveBeenCalled();
    expect(component.trainers).toEqual(mockResponse.data);  
    expect(component.loading).toBeFalse();
  });

  it('should handle unsuccessful response when loading monthly report', () => {
    // Arrange

    const mockResponse: ApiResponse<Trainer[]> = { success: false, data: [], message: 'Error message' };
    adminServiceSpy.getAllTrainer.and.returnValue(of(mockResponse));
    spyOn(console, 'error');


    // Act
    component.loadTrainers();

    // Assert
    expect(adminServiceSpy.getAllTrainer).toHaveBeenCalled();
    expect(component.trainers).toEqual(mockResponse.data);  
    expect(console.error).toHaveBeenCalledWith('Failed to fetch trainers',mockResponse.message);
    expect(component.loading).toBeFalse();
  });

  it('should handle error when loading monthly report', () => {
    // Arrange
   
    const mockError = { error : { message: 'Failed to fetch report'}}
    adminServiceSpy.getAllTrainer.and.returnValue(throwError(mockError));
    spyOn(console, 'error');

    // Act
    component.loadTrainers();

    // Assert
    expect(adminServiceSpy.getAllTrainer).toHaveBeenCalled();
    expect(console.error).toHaveBeenCalledWith(mockError.error.message);
    expect(component.loading).toBeFalse();
  });

});
