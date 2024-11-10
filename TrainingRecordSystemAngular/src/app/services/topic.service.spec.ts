import { TestBed } from '@angular/core/testing';

import { TopicService } from './topic.service';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Job } from '../models/job.model';
import { ApiResponse } from '../models/ApiResponse{T}.model';
import { Topic } from '../models/topic.model';
import { LoadTrainingProgramDetail } from '../models/trainingprogramdetails.model';

describe('TopicService', () => {
  let service: TopicService;
  let httpMock : HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers : [TopicService]
    });
    service = TestBed.inject(TopicService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });


  it('should be created', () => {
   expect(service).toBeTruthy();
  });

  it('should fetch topic by job id successfully', () => {
    //Arrange 
    const jobId = 1;
    const Participants : Topic[] =[ 
      {
        jobId : 1,
        topicId : 1,
        topicName : "test"
      },
      {
        jobId :2,
        topicName : "test2",
        topicId : 2
      }
    ]

    const mockApiResposne: ApiResponse<Topic[]> = {
      success: true,
      data: Participants,
      message: '',
    };
    //Act
    service.getTopicsByJobId(jobId).subscribe((response) => {
      expect(response).toBe(mockApiResposne);
      expect(response.data).toEqual(mockApiResposne.data);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Topic/GetTopicsByJobId?jobId=" + jobId);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResposne);
  });
  it('should handle error for topic by job id', () => {
    //Arrange 
    const jobId = 1;

    const mockApiResposne: ApiResponse<Topic[]> = {
      success: false,
      data: {} as Topic[],
      message: '',
    };
    //Act
    service.getTopicsByJobId(jobId).subscribe((response) => {
      expect(response).toBe(mockApiResposne);
      expect(response.data).toEqual(mockApiResposne.data);
    });

    const req = httpMock.expectOne("http://localhost:5038/api/Topic/GetTopicsByJobId?jobId=" + jobId);
    expect(req.request.method).toBe('GET');
    req.flush(mockApiResposne);
  });
  it('should handle HTTP error for topic by job id', () => {
    //Arrange 
    const jobId = 1;

    const mockHttpError = {
      status: 500,
      statusText: 'Internal server error.',
    };
    //Act
    service.getTopicsByJobId(jobId).subscribe({
      next: () => fail('should have failed with 500 error'),
      error: (error) => {
        //Assert
        expect(error.status).toEqual(500);
        expect(error.statusText).toBe('Internal server error.');
      },
    });
    const req = httpMock.expectOne("http://localhost:5038/api/Topic/GetTopicsByJobId?jobId=" + jobId);
    expect(req.request.method).toBe('GET');
    req.flush({},mockHttpError);
  });

  // --------------- get trainer topics by job id ------------------
  it('should load trainer topics by job id successfully', () => {
    // Arrange
    const jobId = 1;
    const mockDetail: LoadTrainingProgramDetail[] = [
      {
        trainerProgramDetailId: 1,
        startDate: new Date('2001-01-01'),
        trainerTopicId: 1,
        trainerTopic: {
          trainerTopicId: 1,
          userId: 1,
          topicId: 1,
          jobId: 1,
          user: {
            userId: 1,
            firstName: 'Test'
          },
          topic: {
            topicId: 1,
            topicName: 'Topic 1',
            jobId: 1
          },
          job: {
            jobId: 1,
            jobName: 'Job 1'
          },
          isTrainingScheduled: false
        }
      },
      {
        trainerProgramDetailId: 2,
        startDate: new Date('2001-01-01'),
        trainerTopicId: 2,
        trainerTopic: {
          trainerTopicId: 2,
          userId: 1,
          topicId: 2,
          jobId: 1,
          user: {
            userId: 1,
            firstName: 'Test'
          },
          topic: {
            topicId: 2,
            topicName: 'Topic 2',
            jobId: 2
          },
          job: {
            jobId: 1,
            jobName: 'Job 1'
          },
          isTrainingScheduled: false
        }
      },
    ]
    const apiUrl = 'http://localhost:5038/api/Topic/GetTrainerTopicsByJobId?jobId=' + jobId;

    const mockResponse : ApiResponse<LoadTrainingProgramDetail[]> = {
      data: mockDetail,
      success: true,
      message: 'Success'
    }

    service.getTrainerTopicsByJobId(jobId).subscribe((response) => {
      expect(response).toBe(mockResponse)
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle error when trainer topics by job id fails', () => {
    // Arrange
    const jobId = 1;
    const apiUrl = 'http://localhost:5038/api/Topic/GetTrainerTopicsByJobId?jobId=' + jobId;

    const mockResponse : ApiResponse<LoadTrainingProgramDetail[]> = {
      data: [],
      success: false,
      message: 'Failed to fetch trainer topics'
    }

    service.getTrainerTopicsByJobId(jobId).subscribe((response) => {
      expect(response).toBe(mockResponse)
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle http error when trainer topics by job id throws error', () => {
    // Arrange
    const jobId = 1;
    const apiUrl = 'http://localhost:5038/api/Topic/GetTrainerTopicsByJobId?jobId=' + jobId;
    const errorMessage = 'Failed to get trainer topics'

    service.getTrainerTopicsByJobId(jobId).subscribe(
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

  // ------------------ get trainer by topic id --------------------
  it('should load trainer topics by topic id successfully', () => {
    // Arrange
    const topicId = 1;
    const mockDetail: LoadTrainingProgramDetail[] = [
      {
        trainerProgramDetailId: 1,
        startDate: new Date('2001-01-01'),
        trainerTopicId: 1,
        trainerTopic: {
          trainerTopicId: 1,
          userId: 1,
          topicId: 1,
          jobId: 1,
          user: {
            userId: 1,
            firstName: 'Test'
          },
          topic: {
            topicId: 1,
            topicName: 'Topic 1',
            jobId: 1
          },
          job: {
            jobId: 1,
            jobName: 'Job 1'
          },
          isTrainingScheduled: false
        }
      },
      {
        trainerProgramDetailId: 2,
        startDate: new Date('2001-01-01'),
        trainerTopicId: 2,
        trainerTopic: {
          trainerTopicId: 2,
          userId: 1,
          topicId: 2,
          jobId: 1,
          user: {
            userId: 1,
            firstName: 'Test'
          },
          topic: {
            topicId: 2,
            topicName: 'Topic 2',
            jobId: 2
          },
          job: {
            jobId: 1,
            jobName: 'Job 1'
          },
          isTrainingScheduled: false
        }
      },
    ]
    const apiUrl = 'http://localhost:5038/api/Topic/GetTrainerByTopicId?topicId=' + topicId;

    const mockResponse : ApiResponse<LoadTrainingProgramDetail[]> = {
      data: mockDetail,
      success: true,
      message: 'Success'
    }

    service.getTrainerByTopicId(topicId).subscribe((response) => {
      expect(response).toBe(mockResponse)
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle error when trainer topics by topic id fails', () => {
    // Arrange
    const topicId = 1;
    const apiUrl = 'http://localhost:5038/api/Topic/GetTrainerByTopicId?topicId=' + topicId;

    const mockResponse : ApiResponse<LoadTrainingProgramDetail[]> = {
      data: [],
      success: false,
      message: 'Failed to fetch trainer topics'
    }

    service.getTrainerByTopicId(topicId).subscribe((response) => {
      expect(response).toBe(mockResponse)
    })

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should handle http error when trainer topics by topic id throws error', () => {
    // Arrange
    const topicId = 1;
    const apiUrl = 'http://localhost:5038/api/Topic/GetTrainerByTopicId?topicId=' + topicId;
    const errorMessage = 'Failed to get trainer topics'

    service.getTrainerByTopicId(topicId).subscribe(
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
});
