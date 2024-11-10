import { TestBed } from '@angular/core/testing';

import { AdminService } from './admin.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Trainer } from '../models/trainer.model';
import { ApiResponse } from '../models/ApiResponse{T}.model';
import { Job } from '../models/job.model';
import { AssignTrainingTopic } from '../models/assign-training-topic.model';
import { MonthlyAdminReport } from '../models/monthly-admin-report.model';
import { DaterangeBasedReport } from '../models/daterange-based-report.model';

describe('AdminService', () => {
  let service: AdminService;
  let httpMock :HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers : [AdminService]
    });
    service = TestBed.inject(AdminService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  // ------------------- add trainer ------------------
  it('should add a Trainer successfully', () => {
    //Arrange
    const addTrainer: Trainer = {
      firstName : "test",
      lastName : "test",
      email : "test@gmail.com",
      loginbit : true,
      loginId : "test",
      jobId : 1,
      job : {
        jobId : 1,
        jobName : "test",
      },
      role : 1,
      userId : 1,  
    };

    const mockSuccessResposne: ApiResponse<string> = {
      success: true,
      data: '',
      message: 'Trainer saved successfully.',
    };

    const apiUrl = 'http://localhost:5038/api/Admin/AddTrainer';

    //Act
    service.addTrainer(addTrainer).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush(mockSuccessResposne);
  });

  it('should handle error response when adding a trainer fails', () => {
    //Arrange
    const addTrainer: Trainer = {
      firstName : "test",
      lastName : "test",
      email : "test@gmail.com",
      loginbit : true,
      loginId : "test",
      jobId : 1,
      job : {
        jobId : 1,
        jobName : "test",
      },
      role : 1,
      userId : 1,  
    };

    const mockSuccessResposne: ApiResponse<string> = {
      success: false,
      data: '',
      message: 'Trainer already exists',
    };

    const apiUrl = 'http://localhost:5038/api/Admin/AddTrainer';

    //Act
    service.addTrainer(addTrainer).subscribe((response) => {
      expect(response).toBe(mockSuccessResposne);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush(mockSuccessResposne);
  });

  it('should handle http error when add trainer throws error', () => {
    //Arrange
    const addTrainer: Trainer = {
      firstName : "test",
      lastName : "test",
      email : "test@gmail.com",
      loginbit : true,
      loginId : "test",
      jobId : 1,
      job : {
        jobId : 1,
        jobName : "test",
      },
      role : 1,
      userId : 1,  
    };

    const mockHttpError = {
      success: false,
      status: 500,
      statusText: 'Internal server error.',
    };

    const apiUrl = 'http://localhost:5038/api/Admin/AddTrainer';

    //Act
    service.addTrainer(addTrainer).subscribe({
      next: () => fail('should have fail with the error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      },
    });


    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');

    req.flush({},mockHttpError);
  });

  //-------------------get all jobs-------------
  it('should fetch all jobs successfully', () => {
    //Arrange
    const mockJobs : Job[] = [
      {
        jobId : 1,
        jobName : "test"
      },
      {
        jobId:2,
        jobName : "test2"
      }
    ]

    const mockApiResponse : ApiResponse<Job[]> = {
      success : true, data : mockJobs,message : ''
    }
    const apiUrl = 'http://localhost:5038/api/Admin/GetAllJobs';

    //Act
    service.getAllJobs().subscribe((response) => {
      expect(response.data.length).toBe(2);
      expect(response.data).toEqual(mockApiResponse.data);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle error response when get all jobs fails', () => {
    //Arrange
    const mockJobs : Job[] = [
      {
        jobId : 1,
        jobName : "test"
      },
      {
        jobId:2,
        jobName : "test2"
      }
    ]

    const mockApiResponse : ApiResponse<Job[]> = {
      success : false, data : {} as Job[],message : 'Error'
    }
    const apiUrl = 'http://localhost:5038/api/Admin/GetAllJobs';

    //Act
    service.getAllJobs().subscribe((response) => {
      
      expect(response.message).toEqual(mockApiResponse.message);
      expect(response.success).toEqual(mockApiResponse.success);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResponse);
  });

  it('should handle http error when get all jobs throws error', () => {
    //Arrange
 
    const apiUrl = 'http://localhost:5038/api/Admin/GetAllJobs';

    const errorMessage = 'Failed to load jobs';

    //Act
    service.getAllJobs().subscribe(
      () => fail('expected an error'),
      (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    //Respond With error
    req.flush(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });

  // ------------------- get all trainer ----------------
  it('should get all trainers successfully', () => {
    // Arrange
    const mockTrainers: Trainer[] = [
      {
        userId: 1,
        loginId: 'test1',
        firstName: 'First',
        lastName: 'Last',
        email: 'test@example.com',
        role: 1,
        loginbit: true,
        jobId: 1,
        job: {
          jobId: 1,
          jobName: 'Job 1'
        }
      },
      {
        userId: 2,
        loginId: 'test2',
        firstName: 'First',
        lastName: 'Last',
        email: 'test2@example.com',
        role: 2,
        loginbit: true,
        jobId: 2,
        job: {
          jobId: 2,
          jobName: 'Job 2'
        }
      },
    ]

    const mockResponse: ApiResponse<Trainer[]> = {
      data: mockTrainers,
      success: true,
      message: 'Found'
    }

    const apiUrl = 'http://localhost:5038/api/Admin/GetAllTrainer';

    service.getAllTrainer().subscribe((response) => {
      // Assert
      expect(response.data).toEqual(mockTrainers);
      expect(response.data.length).toEqual(mockTrainers.length)
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle error response when get all trainers fails', () => {
    // Arrange
    const mockResponse: ApiResponse<Trainer[]> = {
      data: [],
      success: false,
      message: 'Found'
    }

    const apiUrl = 'http://localhost:5038/api/Admin/GetAllTrainer';

    service.getAllTrainer().subscribe((response) => {
      // Assert
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle http error when get all trainer throws error', () => {
    // Arrange
    const apiUrl = 'http://localhost:5038/api/Admin/GetAllTrainer';

    const errorMessage = 'Failed to load trainers';

    service.getAllTrainer().subscribe(
      () => fail('expected an error'),
      (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });

  // ------------------ get trainer by login id -------------
  it('should load trainer by login id successfully', () => {
    // Arrange
    const userId = '1';
    const mockTrainer: Trainer = {
      userId: 1,
      loginId: 'test',
      firstName: 'First',
      lastName: 'Last',
      email: 'test@example.com',
      role: 1,
      loginbit: true,
      jobId: 1,
      job: {
        jobId: 1,
        jobName: 'Job 1'
      }
    }

    const apiUrl = 'http://localhost:5038/api/Admin/GetTrainerByLoginId/' + userId;

    const mockResponse: ApiResponse<Trainer> = {
      data: mockTrainer,
      success: true,
      message: 'Found'
    };

    service.getTrainerByLoginId(userId).subscribe((response) => {
      expect(response.data).toEqual(mockTrainer);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle error when get trainer by login id fails', () => {
    // Arrange
    const userId = '1';

    const apiUrl = 'http://localhost:5038/api/Admin/GetTrainerByLoginId/' + userId;

    const mockResponse: ApiResponse<Trainer> = {
      data: {
        userId: 0,
        loginId: '',
        firstName: '',
        lastName: '',
        email: '',
        role: 0,
        loginbit: false,
        jobId: 0,
        job: {
          jobId: 0,
          jobName: ''
        }
      },
      success: false,
      message: 'Found'
    };

    service.getTrainerByLoginId(userId).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle http error when get trainer by login id throws error', () => {
    // Arrange
    const userId = '1';
    const apiUrl = 'http://localhost:5038/api/Admin/GetTrainerByLoginId/' + userId;
    const errorMessage = 'Failed to load trainers';

    service.getTrainerByLoginId(userId).subscribe(
      () => fail('expected an error'),
      (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    );

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  })
  
  // ------------------ assign topic to trainer -------------
  it('should assign topic to trainer successfully', () => {
    // Arrange
    const assignTrainingTopic: AssignTrainingTopic = {
      userId: 1,
      topicId: 2
    }
    const apiUrl = 'http://localhost:5038/api/Admin/AssignTopicToTrainer';

    const mockResponse: ApiResponse<string> = {
      data: 'Success',
      success: true,
      message: 'Topic assigned successfully'
    };

    service.assignTopicToTrainer(assignTrainingTopic).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should handle error when assign topic fails', () => {
    // Arrange
    const assignTrainingTopic: AssignTrainingTopic = {
      userId: 1,
      topicId: 2
    }
    const apiUrl = 'http://localhost:5038/api/Admin/AssignTopicToTrainer';

    const mockResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Failed to assign topic'
    };

    service.assignTopicToTrainer(assignTrainingTopic).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should handle error when assign topic throws error', () => {
    // Arrange
    const assignTrainingTopic: AssignTrainingTopic = {
      userId: 1,
      topicId: 2
    }
    const apiUrl = 'http://localhost:5038/api/Admin/AssignTopicToTrainer';
    const errorMessage = 'Failed to assign topic';

    service.assignTopicToTrainer(assignTrainingTopic).subscribe(
      () => fail('expected an error'),
      (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    )

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });

  // ----------------- monthly admin report -----------------
  it('should load monthly admin report successfully when month and year are null', () => {
    // Arrange
    const userId = 1;
    const month = null;
    const year = null;
    const apiUrl = 'http://localhost:5038/api/Admin/MonthlyAdminReport?userId=' + userId;
    const mockReport: MonthlyAdminReport[] = [
      {
        topicName: 'Topic 1',
        startDate: '2000-01-01',
        endDate: '2000-02-01',
        duration: 4,
        modePreference: 'hybrid',
        totalParticipateNo: 10
      }
    ]

    const mockResponse: ApiResponse<MonthlyAdminReport[]> = {
      data: mockReport,
      success: true,
      message: 'Success'
    };

    service.monthlyAdminReport(userId, month, year).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle error in monthly admin report when month and year are null', () => {
    // Arrange
    const userId = 1;
    const month = null;
    const year = null;
    const apiUrl = 'http://localhost:5038/api/Admin/MonthlyAdminReport?userId=' + userId;

    const mockResponse: ApiResponse<MonthlyAdminReport[]> = {
      data: [],
      success: false,
      message: 'Failed to find report'
    };

    service.monthlyAdminReport(userId, month, year).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle http error in monthly admin report when month and year are null', () => {
    // Arrange
    const userId = 1;
    const month = null;
    const year = null;
    const apiUrl = 'http://localhost:5038/api/Admin/MonthlyAdminReport?userId=' + userId;
    const errorMessage = 'Failed to assign topic';

    const mockResponse: ApiResponse<MonthlyAdminReport[]> = {
      data: [],
      success: false,
      message: 'Failed to find report'
    };

    service.monthlyAdminReport(userId, month, year).subscribe(
      () => fail('expected an error'),
      (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    )

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });

  it('should load monthly admin report successfully', () => {
    // Arrange
    const userId = 1;
    const month = 12;
    const year = 2001;
    const apiUrl = 'http://localhost:5038/api/Admin/MonthlyAdminReport?userId=' + userId
     + '&month=' + month
     + '&year=' + year;
    const mockReport: MonthlyAdminReport[] = [
      {
        topicName: 'Topic 1',
        startDate: '2000-01-01',
        endDate: '2000-02-01',
        duration: 4,
        modePreference: 'hybrid',
        totalParticipateNo: 10
      }
    ]

    const mockResponse: ApiResponse<MonthlyAdminReport[]> = {
      data: mockReport,
      success: true,
      message: 'Success'
    };

    service.monthlyAdminReport(userId, month, year).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle error in monthly admin report', () => {
    // Arrange
    const userId = 1;
    const month = 12;
    const year = 2001;
    const apiUrl = 'http://localhost:5038/api/Admin/MonthlyAdminReport?userId=' + userId
     + '&month=' + month
     + '&year=' + year;

    const mockResponse: ApiResponse<MonthlyAdminReport[]> = {
      data: [],
      success: false,
      message: 'Failed to find report'
    };

    service.monthlyAdminReport(userId, month, year).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle http error in monthly admin report', () => {
    // Arrange
    const userId = 1;
    const month = 12;
    const year = 2001;
    const apiUrl = 'http://localhost:5038/api/Admin/MonthlyAdminReport?userId=' + userId
     + '&month=' + month
     + '&year=' + year;
    const errorMessage = 'Failed to assign topic';

    const mockResponse: ApiResponse<MonthlyAdminReport[]> = {
      data: [],
      success: false,
      message: 'Failed to find report'
    };

    service.monthlyAdminReport(userId, month, year).subscribe(
      () => fail('expected an error'),
      (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    )

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });

  // ------------------ date range based report ----------------
  it('should load date range report successfully when start date and end date are null', () => {
    // Arrange
    const jobId = 1;
    const startDate = null;
    const endDate = null;
    const apiUrl = 'http://localhost:5038/api/Admin/DaterangeBasedReport?jobId=' + jobId;
    const mockReport: DaterangeBasedReport[] = [
      {
        topicName: 'Topic 1',
        trainerName: 'Trainer 1',
        startDate: '2001-01-01',
        endDate: '2001-02-01',
        duration: 4,
        modePreference: 'online',
        totalParticipateNo: 10
      }
    ]

    const mockResponse: ApiResponse<DaterangeBasedReport[]> = {
      data: mockReport,
      success: true,
      message: 'Success'
    };

    service.daterangeBasedReport(jobId, startDate, endDate).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle error in date range report when start date and end date are null', () => {
    // Arrange
    const jobId = 1;
    const startDate = null;
    const endDate = null;
    const apiUrl = 'http://localhost:5038/api/Admin/DaterangeBasedReport?jobId=' + jobId;

    const mockResponse: ApiResponse<DaterangeBasedReport[]> = {
      data: [],
      success: false,
      message: 'Failed to get report'
    };

    service.daterangeBasedReport(jobId, startDate, endDate).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle http error in date range report when start date and end date are null', () => {
    // Arrange
    const jobId = 1;
    const startDate = null;
    const endDate = null;
    const apiUrl = 'http://localhost:5038/api/Admin/DaterangeBasedReport?jobId=' + jobId;
    const errorMessage = 'Failed to assign topic';

    service.daterangeBasedReport(jobId, startDate, endDate).subscribe(
      () => fail('expected an error'),
      (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    )

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });

  it('should load date range report successfully', () => {
    // Arrange
    const jobId = 1;
    const startDate = '2001-01-01';
    const endDate = '2001-02-01';
    const apiUrl = 'http://localhost:5038/api/Admin/DaterangeBasedReport?jobId=' + jobId + 
    '&startDate=' + startDate +
    '&endDate=' + endDate;
    const mockReport: DaterangeBasedReport[] = [
      {
        topicName: 'Topic 1',
        trainerName: 'Trainer 1',
        startDate: '2001-01-01',
        endDate: '2001-02-01',
        duration: 4,
        modePreference: 'online',
        totalParticipateNo: 10
      }
    ]

    const mockResponse: ApiResponse<DaterangeBasedReport[]> = {
      data: mockReport,
      success: true,
      message: 'Success'
    };

    service.daterangeBasedReport(jobId, startDate, endDate).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle error in date range report', () => {
    // Arrange
    const jobId = 1;
    const startDate = '2001-01-01';
    const endDate = '2001-02-01';
    const apiUrl = 'http://localhost:5038/api/Admin/DaterangeBasedReport?jobId=' + jobId + 
    '&startDate=' + startDate +
    '&endDate=' + endDate;

    const mockResponse: ApiResponse<DaterangeBasedReport[]> = {
      data: [],
      success: false,
      message: 'Failed to get report'
    };

    service.daterangeBasedReport(jobId, startDate, endDate).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle http error in date range report', () => {
    // Arrange
    const jobId = 1;
    const startDate = '2001-01-01';
    const endDate = '2001-02-01';
    const apiUrl = 'http://localhost:5038/api/Admin/DaterangeBasedReport?jobId=' + jobId + 
    '&startDate=' + startDate +
    '&endDate=' + endDate;
    const errorMessage = 'Failed to assign topic';

    service.daterangeBasedReport(jobId, startDate, endDate).subscribe(
      () => fail('expected an error'),
      (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    )

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });

  // ------------------ unassign topic ----------------------
  it('should unassign topic successfully', () => {
    // Arrange
    const userId = 1;
    const topicId = 2;
    const apiUrl = 'http://localhost:5038/api/Admin/UnassignTopic?userId=' + userId + '&topicId=' + topicId;

    const mockResponse: ApiResponse<string> = {
      data: 'Success',
      success: true,
      message: 'Topic unassigned successfully'
    };

    service.unassignTopic(userId, topicId).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockResponse);
  });

  it('should handle error when unassign topic fails', () => {
    // Arrange
    const userId = 1;
    const topicId = 2;
    const apiUrl = 'http://localhost:5038/api/Admin/UnassignTopic?userId=' + userId + '&topicId=' + topicId;

    const mockResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Failed to unassign topic'
    };

    service.unassignTopic(userId, topicId).subscribe((response) => {
      expect(response.message).toEqual(mockResponse.message);
      expect(response.success).toEqual(mockResponse.success);
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('DELETE');
    req.flush(mockResponse);
  });

  it('should handle error when unassign topic throws error', () => {
    // Arrange
    const userId = 1;
    const topicId = 2;
    const apiUrl = 'http://localhost:5038/api/Admin/UnassignTopic?userId=' + userId + '&topicId=' + topicId;
    const errorMessage = 'Failed to assign topic';

    service.unassignTopic(userId, topicId).subscribe(
      () => fail('expected an error'),
      (error) => {
        expect(error.status).toBe(500);
        expect(error.statusText).toBe('Internal Server Error');
      }
    )

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('DELETE');
    req.flush(errorMessage, {
      status: 500,
      statusText: 'Internal Server Error',
    });
  });
});
