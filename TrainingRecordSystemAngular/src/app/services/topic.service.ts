import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/ApiResponse{T}.model';
import { Topic } from '../models/topic.model';
import { TrainingTopic } from '../models/trainingTopic.model';
import { LoadTrainingProgramDetail } from '../models/trainingprogramdetails.model';

@Injectable({
  providedIn: 'root'
})
export class TopicService {

  private apiUrl = "http://localhost:5038/api/Topic/"

  constructor(private http: HttpClient) { }

  getTopicsByJobId(jobId: number): Observable<ApiResponse<Topic[]>> {
    return this.http.get<ApiResponse<Topic[]>>(this.apiUrl + 'GetTopicsByJobId?jobId=' + jobId);
  }
  getTrainerTopicsByJobId(jobId: number): Observable<ApiResponse<LoadTrainingProgramDetail[]>> {
    return this.http.get<ApiResponse<LoadTrainingProgramDetail[]>>(this.apiUrl + 'GetTrainerTopicsByJobId?jobId=' + jobId);
  }
  getTrainerByTopicId(topicId: number): Observable<ApiResponse<LoadTrainingProgramDetail[]>> {
    return this.http.get<ApiResponse<LoadTrainingProgramDetail[]>>(this.apiUrl + 'GetTrainerByTopicId?topicId=' + topicId);
  }
}
